using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Illiyeen.Models;
using Illiyeen.Models.ViewModels;
using Illiyeen.Services;

namespace Illiyeen.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        //private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;

        public CartController(ApplicationDbContext context, UserManager<User> userManager, /*IPaymentService paymentService,*/ IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            //_paymentService = paymentService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                    .ThenInclude(p => p.Category)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var viewModel = new CartViewModel
            {
                CartItems = cartItems
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var userId = _userManager.GetUserId(User);
            var product = await _context.Products.FindAsync(productId);

            if (product == null || !product.IsActive || product.Stock < quantity)
            {
                return Json(new { success = false, message = "Product not available" });
            }

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.UpdatedAt = DateTime.Now;
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = userId!,
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedAt = DateTime.Now
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            var cartCount = await _context.CartItems
                .Where(c => c.UserId == userId)
                .SumAsync(c => c.Quantity);

            return Json(new { success = true, cartCount });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (cartItem == null)
            {
                return Json(new { success = false, message = "Item not found" });
            }

            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
                cartItem.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = _userManager.GetUserId(User);
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            var viewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalAmount = cartItems.Sum(c => c.TotalPrice),
                ShippingAddress = user?.Address ?? "",
                ShippingCity = user?.City ?? "",
                ShippingPostalCode = user?.PostalCode ?? "",
                PhoneNumber = user?.PhoneNumber ?? ""
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index");
            }

            var order = new Order
            {
                UserId = userId!,
                TotalAmount = cartItems.Sum(c => c.TotalPrice),
                ShippingAddress = model.ShippingAddress,
                ShippingCity = model.ShippingCity,
                ShippingPostalCode = model.ShippingPostalCode,
                PhoneNumber = model.PhoneNumber,
                PaymentMethod = model.PaymentMethod,
                Notes = model.Notes,
                CreatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Add order items
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price
                };
                _context.OrderItems.Add(orderItem);

                // Update product stock
                cartItem.Product.Stock -= cartItem.Quantity;
            }

            // Clear cart
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            // Process payment if not Cash on Delivery
            if (model.PaymentMethod != "Cash on Delivery")
            {
                //var paymentResult = await _paymentService.ProcessPaymentAsync(order, model.PaymentMethod);
                //if (paymentResult.Success)
                //{
                //    order.PaymentStatus = "Paid";
                //    order.TransactionId = paymentResult.TransactionId;
                //    await _context.SaveChangesAsync();
                //}
            }

            // Send order confirmation email
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _emailService.SendOrderConfirmationEmailAsync(user!.Email, order);
            }
            catch
            {
                // Log error but don't fail order
            }

            return RedirectToAction("OrderConfirmation", new { id = order.Id });
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var userId = _userManager.GetUserId(User);
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
