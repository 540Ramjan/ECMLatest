using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Illiyeen.Models;
using Illiyeen.Models.ViewModels;

namespace Illiyeen.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId, string? sortBy, int page = 1)
        {
            var query = _context.Products.Include(p => p.Category).Where(p => p.IsActive);

            // Filter by search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            // Filter by category
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // Sort products
            query = sortBy?.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "name" => query.OrderBy(p => p.Name),
                "newest" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderBy(p => p.Name)
            };

            const int pageSize = 12;
            var totalProducts = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();
            var selectedCategory = categoryId.HasValue ? await _context.Categories.FindAsync(categoryId.Value) : null;

            var viewModel = new ProductListViewModel
            {
                Products = products,
                Categories = categories,
                SelectedCategory = selectedCategory,
                SearchQuery = search,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalProducts = totalProducts,
                SortBy = sortBy
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null)
            {
                return NotFound();
            }

            var relatedProducts = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id && p.IsActive)
                .Take(4)
                .ToListAsync();

            var viewModel = new ProductViewModel
            {
                Product = product,
                RelatedProducts = relatedProducts,
                Reviews = product.Reviews.OrderByDescending(r => r.CreatedAt)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Category(string slug)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);

            if (category == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", new { categoryId = category.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Search(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", new { search = q });
        }
    }
}
