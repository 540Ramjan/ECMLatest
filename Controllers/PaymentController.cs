using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Illiyeen.Models;
using Illiyeen.Services;

namespace Illiyeen.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly IPaymentService _paymentService;

        public PaymentController(ApplicationDbContext context /*IPaymentService paymentService*/)
        {
            _context = context;
            //_paymentService = paymentService;
        }

        [HttpPost]
        //public async Task<IActionResult> InitiatebKashPayment(int orderId)
        //{
        //    var order = await _context.Orders.FindAsync(orderId);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    var result = await _paymentService.InitiatebKashPaymentAsync(order);
            
        //    if (result.Success)
        //    {
        //        return Json(new { success = true, redirectUrl = result.RedirectUrl });
        //    }

        //    return Json(new { success = false, message = result.ErrorMessage });
        //}

        //[HttpPost]
        //public async Task<IActionResult> InitiateNagadPayment(int orderId)
        //{
        //    var order = await _context.Orders.FindAsync(orderId);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    var result = await _paymentService.InitiateNagadPaymentAsync(order);
            
        //    if (result.Success)
        //    {
        //        return Json(new { success = true, redirectUrl = result.RedirectUrl });
        //    }

        //    return Json(new { success = false, message = result.ErrorMessage });
        //}

        //[HttpGet]
        //public async Task<IActionResult> bKashCallback(string paymentID, string status)
        //{
        //    var result = await _paymentService.VerifybKashPaymentAsync(paymentID);
            
        //    if (result.Success)
        //    {
        //        var order = await _context.Orders.FirstOrDefaultAsync(o => o.TransactionId == paymentID);
        //        if (order != null)
        //        {
        //            order.PaymentStatus = "Paid";
        //            order.Status = "Confirmed";
        //            order.UpdatedAt = DateTime.Now;
        //            await _context.SaveChangesAsync();
        //        }

        //        return RedirectToAction("OrderConfirmation", "Cart", new { id = order?.Id });
        //    }

        //    return RedirectToAction("PaymentFailed");
        //}

        //[HttpGet]
        //public async Task<IActionResult> NagadCallback(string payment_ref_id, string status)
        //{
        //    var result = await _paymentService.VerifyNagadPaymentAsync(payment_ref_id);
            
        //    if (result.Success)
        //    {
        //        var order = await _context.Orders.FirstOrDefaultAsync(o => o.TransactionId == payment_ref_id);
        //        if (order != null)
        //        {
        //            order.PaymentStatus = "Paid";
        //            order.Status = "Confirmed";
        //            order.UpdatedAt = DateTime.Now;
        //            await _context.SaveChangesAsync();
        //        }

        //        return RedirectToAction("OrderConfirmation", "Cart", new { id = order?.Id });
        //    }

        //    return RedirectToAction("PaymentFailed");
        //}

        public IActionResult PaymentFailed()
        {
            return View();
        }
    }
}
