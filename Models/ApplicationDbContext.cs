using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Illiyeen.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Product configuration
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Cart configuration
            builder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.UserId);

            builder.Entity<CartItem>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId);

            // Order configuration
            builder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            // OrderItem configuration
            builder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // Review configuration
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            builder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId);

            // Configure DateTime properties for PostgreSQL UTC
            builder.Entity<Product>()
                .Property(p => p.CreatedAt)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            builder.Entity<Product>()
                .Property(p => p.UpdatedAt)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null);

            builder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            builder.Entity<User>()
                .Property(u => u.LastLoginAt)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null);

            // Seed data will be handled manually in DatabaseInitializer
        }

        public async Task SeedDataAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed categories if they don't exist
            if (!Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Men's Fashion", Slug = "mens-fashion", Description = "Premium panjabis, kurtas, traditional Bengali menswear" },
                    new Category { Name = "Women's Fashion", Slug = "womens-fashion", Description = "Elegant sarees, kurtis, contemporary designs" },
                    new Category { Name = "Accessories", Slug = "accessories", Description = "Premium bags, shoes, lifestyle accessories" },
                    new Category { Name = "Watches", Slug = "watches", Description = "Luxury timepieces including Sahara and Platinum collections" },
                    new Category { Name = "Home & Lifestyle", Slug = "home-lifestyle", Description = "Premium home decor and lifestyle products" }
                };

                Categories.AddRange(categories);
                await SaveChangesAsync();
            }

            // Seed products if they don't exist
            if (!Products.Any())
            {
                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Premium Bengali Panjabi",
                        Description = "Traditional embroidery with authentic Bengali craftsmanship",
                        Price = 4500,
                        CategoryId = 1,
                        Stock = 25,
                        ImageUrl = "https://via.placeholder.com/400x400/d4af37/000000?text=Premium+Panjabi",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Luxury Silk Kurta",
                        Description = "Golden threadwork with premium silk fabric",
                        Price = 8900,
                        CategoryId = 1,
                        Stock = 15,
                        ImageUrl = "https://via.placeholder.com/400x400/d4af37/000000?text=Silk+Kurta",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Elegant Silk Saree",
                        Description = "Traditional motifs with contemporary elegance",
                        Price = 12500,
                        CategoryId = 2,
                        Stock = 20,
                        ImageUrl = "https://via.placeholder.com/400x400/d4af37/000000?text=Silk+Saree",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Designer Kurti Set",
                        Description = "Complete set with palazzo pants",
                        Price = 3800,
                        CategoryId = 2,
                        Stock = 30,
                        ImageUrl = "https://via.placeholder.com/400x400/d4af37/000000?text=Kurti+Set",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Premium Leather Handbag",
                        Description = "Luxury craftsmanship with elegant design",
                        Price = 6500,
                        CategoryId = 3,
                        Stock = 12,
                        ImageUrl = "https://via.placeholder.com/400x400/d4af37/000000?text=Leather+Handbag",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Luxury Gold Watch",
                        Description = "Swiss movement with premium gold plating",
                        Price = 25000,
                        CategoryId = 4,
                        Stock = 8,
                        ImageUrl = "https://via.placeholder.com/400x400/d4af37/000000?text=Gold+Watch",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                Products.AddRange(products);
                await SaveChangesAsync();
            }
        }
    }

    public static class DatabaseInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Create roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            // Create admin user
            if (await userManager.FindByEmailAsync("admin@illiyeen.com") == null)
            {
                var adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@illiyeen.com",
                    FirstName = "Admin",
                    LastName = "User",
                    PhoneNumber = "01700000000",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, "admin123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Create customer user
            if (await userManager.FindByEmailAsync("customer@illiyeen.com") == null)
            {
                var customerUser = new User
                {
                    UserName = "customer",
                    Email = "customer@illiyeen.com",
                    FirstName = "Customer",
                    LastName = "User",
                    PhoneNumber = "01700000001",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(customerUser, "customer123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");
                }
            }

            // Note: Sample data seeding temporarily disabled for initial testing
        }
    }
}
