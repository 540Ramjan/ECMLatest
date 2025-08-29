using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Illiyeen.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    ShippingAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShippingCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShippingPostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Pending"),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.CheckConstraint("CHK_Orders_Status", "[Status] IN ('Pending', 'Confirmed', 'Processing', 'Shipped', 'Delivered', 'Cancelled')");
                    table.CheckConstraint("CHK_Orders_PaymentStatus", "[PaymentStatus] IN ('Pending', 'Paid', 'Failed', 'Refunded')");
                    table.CheckConstraint("CHK_Orders_TotalAmount", "[TotalAmount] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Features = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Material = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.CheckConstraint("CHK_Products_Price", "[Price] >= 0");
                    table.CheckConstraint("CHK_Products_Stock", "[Stock] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.CheckConstraint("CHK_CartItems_Quantity", "[Quantity] > 0");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.CheckConstraint("CHK_OrderItems_Price", "[Price] >= 0");
                    table.CheckConstraint("CHK_OrderItems_Quantity", "[Quantity] > 0");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.CheckConstraint("CHK_Reviews_Rating", "[Rating] >= 1 AND [Rating] <= 5");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, "Premium panjabis, kurtas, traditional Bengali menswear", "Men's Fashion", "mens-fashion" },
                    { 2, "Elegant sarees, kurtis, contemporary designs", "Women's Fashion", "womens-fashion" },
                    { 3, "Premium bags, shoes, lifestyle accessories", "Accessories", "accessories" },
                    { 4, "Luxury timepieces including Sahara and Platinum collections", "Watches", "watches" },
                    { 5, "Premium home decor and lifestyle products", "Home & Lifestyle", "home-lifestyle" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "IsFeatured", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, 1, "Traditional embroidery with authentic Bengali craftsmanship", "https://via.placeholder.com/400x400/d4af37/000000?text=Premium+Panjabi", true, "Premium Bengali Panjabi", 4500m, 25 },
                    { 2, 1, "Golden threadwork with premium silk fabric", "https://via.placeholder.com/400x400/d4af37/000000?text=Silk+Kurta", true, "Luxury Silk Kurta", 8900m, 15 },
                    { 3, 2, "Traditional motifs with contemporary elegance", "https://via.placeholder.com/400x400/d4af37/000000?text=Silk+Saree", true, "Elegant Silk Saree", 12500m, 20 },
                    { 4, 2, "Complete set with palazzo pants", "https://via.placeholder.com/400x400/d4af37/000000?text=Kurti+Set", true, "Designer Kurti Set", 3800m, 30 },
                    { 5, 3, "Luxury craftsmanship with elegant design", "https://via.placeholder.com/400x400/d4af37/000000?text=Leather+Handbag", true, "Premium Leather Handbag", 6500m, 12 },
                    { 6, 4, "Swiss movement with premium gold plating", "https://via.placeholder.com/400x400/d4af37/000000?text=Gold+Watch", true, "Luxury Gold Watch", 25000m, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId",
                table: "CartItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsActive",
                table: "Categories",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedAt",
                table: "Orders",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentStatus",
                table: "Orders",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TransactionId",
                table: "Orders",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedAt",
                table: "Products",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsActive",
                table: "Products",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsFeatured",
                table: "Products",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Price",
                table: "Products",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Rating",
                table: "Reviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            // Create additional performance indexes
            migrationBuilder.Sql(@"
                CREATE NONCLUSTERED INDEX IX_Products_Name_Description 
                ON Products(Name, Description);
                
                CREATE NONCLUSTERED INDEX IX_Orders_UserId_Status 
                ON Orders(UserId, Status);
                
                CREATE NONCLUSTERED INDEX IX_Reviews_ProductId_Rating 
                ON Reviews(ProductId, Rating);
                
                CREATE UNIQUE NONCLUSTERED INDEX UQ_CartItems_UserProduct 
                ON CartItems(UserId, ProductId);
                
                CREATE UNIQUE NONCLUSTERED INDEX UQ_Reviews_UserProduct 
                ON Reviews(UserId, ProductId);
            ");

            // Create stored procedures
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetProductsByCategory
                    @CategoryId int,
                    @PageNumber int = 1,
                    @PageSize int = 12,
                    @SortBy nvarchar(50) = 'name',
                    @SearchTerm nvarchar(100) = ''
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    DECLARE @Offset int = (@PageNumber - 1) * @PageSize;
                    
                    SELECT 
                        p.*,
                        c.Name as CategoryName,
                        c.Slug as CategorySlug,
                        ISNULL(AVG(CAST(r.Rating as float)), 0) as AverageRating,
                        COUNT(r.Id) as ReviewCount
                    FROM Products p
                    INNER JOIN Categories c ON p.CategoryId = c.Id
                    LEFT JOIN Reviews r ON p.Id = r.ProductId
                    WHERE 
                        p.IsActive = 1 
                        AND (@CategoryId IS NULL OR p.CategoryId = @CategoryId)
                        AND (@SearchTerm = '' OR p.Name LIKE '%' + @SearchTerm + '%' OR p.Description LIKE '%' + @SearchTerm + '%')
                    GROUP BY p.Id, p.Name, p.Description, p.Price, p.CategoryId, p.Stock, p.ImageUrl, p.Features, p.Brand, p.Size, p.Color, p.Material, p.IsActive, p.IsFeatured, p.CreatedAt, p.UpdatedAt, c.Name, c.Slug
                    ORDER BY 
                        CASE WHEN @SortBy = 'name' THEN p.Name END ASC,
                        CASE WHEN @SortBy = 'price_asc' THEN p.Price END ASC,
                        CASE WHEN @SortBy = 'price_desc' THEN p.Price END DESC,
                        CASE WHEN @SortBy = 'newest' THEN p.CreatedAt END DESC,
                        CASE WHEN @SortBy = '' THEN p.IsFeatured END DESC,
                        p.CreatedAt DESC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;
                    
                    -- Get total count
                    SELECT COUNT(*) as TotalCount
                    FROM Products p
                    WHERE 
                        p.IsActive = 1 
                        AND (@CategoryId IS NULL OR p.CategoryId = @CategoryId)
                        AND (@SearchTerm = '' OR p.Name LIKE '%' + @SearchTerm + '%' OR p.Description LIKE '%' + @SearchTerm + '%');
                END
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetOrdersByUser
                    @UserId nvarchar(450),
                    @PageNumber int = 1,
                    @PageSize int = 10
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    DECLARE @Offset int = (@PageNumber - 1) * @PageSize;
                    
                    SELECT 
                        o.*,
                        COUNT(oi.Id) as ItemCount,
                        SUM(oi.Quantity) as TotalQuantity
                    FROM Orders o
                    LEFT JOIN OrderItems oi ON o.Id = oi.OrderId
                    WHERE o.UserId = @UserId
                    GROUP BY o.Id, o.UserId, o.TotalAmount, o.Status, o.ShippingAddress, o.ShippingCity, o.ShippingPostalCode, o.PhoneNumber, o.PaymentMethod, o.PaymentStatus, o.TransactionId, o.Notes, o.CreatedAt, o.UpdatedAt
                    ORDER BY o.CreatedAt DESC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;
                    
                    -- Get total count
                    SELECT COUNT(*) as TotalCount
                    FROM Orders
                    WHERE UserId = @UserId;
                END
            ");

            // Create views
            migrationBuilder.Sql(@"
                CREATE VIEW vw_ProductStats
                AS
                SELECT 
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Stock,
                    c.Name as CategoryName,
                    ISNULL(AVG(CAST(r.Rating as float)), 0) as AverageRating,
                    COUNT(r.Id) as ReviewCount,
                    ISNULL(SUM(oi.Quantity), 0) as TotalSold,
                    ISNULL(SUM(oi.Quantity * oi.Price), 0) as TotalRevenue
                FROM Products p
                INNER JOIN Categories c ON p.CategoryId = c.Id
                LEFT JOIN Reviews r ON p.Id = r.ProductId
                LEFT JOIN OrderItems oi ON p.Id = oi.ProductId
                LEFT JOIN Orders o ON oi.OrderId = o.Id AND o.Status IN ('Confirmed', 'Processing', 'Shipped', 'Delivered')
                WHERE p.IsActive = 1
                GROUP BY p.Id, p.Name, p.Price, p.Stock, c.Name
            ");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_SalesReport
                AS
                SELECT 
                    YEAR(o.CreatedAt) as Year,
                    MONTH(o.CreatedAt) as Month,
                    COUNT(o.Id) as OrderCount,
                    SUM(o.TotalAmount) as TotalRevenue,
                    AVG(o.TotalAmount) as AverageOrderValue,
                    COUNT(DISTINCT o.UserId) as UniqueCustomers
                FROM Orders o
                WHERE o.Status IN ('Confirmed', 'Processing', 'Shipped', 'Delivered')
                GROUP BY YEAR(o.CreatedAt), MONTH(o.CreatedAt)
            ");

            // Create functions
            migrationBuilder.Sql(@"
                CREATE FUNCTION CalculateProductAverageRating(@ProductId int)
                RETURNS decimal(3,2)
                AS
                BEGIN
                    DECLARE @AverageRating decimal(3,2);
                    
                    SELECT @AverageRating = AVG(CAST(Rating as decimal(3,2)))
                    FROM Reviews
                    WHERE ProductId = @ProductId;
                    
                    RETURN ISNULL(@AverageRating, 0);
                END
            ");

            // Create triggers
            migrationBuilder.Sql(@"
                CREATE TRIGGER tr_Products_UpdatedAt
                ON Products
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    UPDATE Products
                    SET UpdatedAt = GETUTCDATE()
                    FROM Products p
                    INNER JOIN inserted i ON p.Id = i.Id;
                END
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER tr_Orders_UpdatedAt
                ON Orders
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    UPDATE Orders
                    SET UpdatedAt = GETUTCDATE()
                    FROM Orders o
                    INNER JOIN inserted i ON o.Id = i.Id;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop triggers
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS tr_Products_UpdatedAt");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS tr_Orders_UpdatedAt");

            // Drop functions
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS CalculateProductAverageRating");

            // Drop views
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_ProductStats");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_SalesReport");

            // Drop stored procedures
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetProductsByCategory");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetOrdersByUser");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
