using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Illiyeen.Models;

namespace Illiyeen.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts(int page = 1, int limit = 12, string? search = null, int? categoryId = null)
        {
            var query = _context.Products.Include(p => p.Category).Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            var totalProducts = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.ImageUrl,
                    p.Stock,
                    Category = p.Category.Name,
                    p.AverageRating,
                    p.ReviewCount
                })
                .ToListAsync();

            return Ok(new
            {
                products,
                totalProducts,
                currentPage = page,
                totalPages = (int)Math.Ceiling(totalProducts / (double)limit)
            });
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .Where(p => p.Id == id && p.IsActive)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.ImageUrl,
                    p.Stock,
                    p.Features,
                    p.Brand,
                    p.Size,
                    p.Color,
                    p.Material,
                    Category = new { p.Category.Id, p.Category.Name, p.Category.Slug },
                    p.AverageRating,
                    p.ReviewCount,
                    Reviews = p.Reviews.Select(r => new
                    {
                        r.Id,
                        r.Rating,
                        r.Comment,
                        r.CreatedAt,
                        User = new { r.User.FirstName, r.User.LastName }
                    }).OrderByDescending(r => r.CreatedAt)
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Slug,
                    c.Description,
                    c.ImageUrl,
                    ProductCount = c.Products.Count(p => p.IsActive)
                })
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet("categories/{slug}/products")]
        public async Task<IActionResult> GetCategoryProducts(string slug, int page = 1, int limit = 12)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);

            if (category == null)
            {
                return NotFound();
            }

            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == category.Id && p.IsActive);

            var totalProducts = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.ImageUrl,
                    p.Stock,
                    p.AverageRating,
                    p.ReviewCount
                })
                .ToListAsync();

            return Ok(new
            {
                category = new { category.Id, category.Name, category.Slug, category.Description },
                products,
                totalProducts,
                currentPage = page,
                totalPages = (int)Math.Ceiling(totalProducts / (double)limit)
            });
        }
    }
}
