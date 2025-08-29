-- ILLIYEEN E-Commerce Database Schema for SQL Server
-- Premium Bengali Fashion E-Commerce Platform

USE master;
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'IlliyeenDB')
BEGIN
    CREATE DATABASE IlliyeenDB;
END
GO

USE IlliyeenDB;
GO

-- Enable required features
IF NOT EXISTS (SELECT 1 FROM sys.configurations WHERE name = 'contained database authentication' AND value = 1)
BEGIN
    EXEC sp_configure 'contained database authentication', 1;
    RECONFIGURE;
END
GO

-- Create Categories table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
BEGIN
    CREATE TABLE Categories (
        Id int IDENTITY(1,1) NOT NULL,
        Name nvarchar(100) NOT NULL,
        Slug nvarchar(100) NOT NULL,
        Description nvarchar(500) NULL,
        ImageUrl nvarchar(max) NULL,
        IsActive bit NOT NULL DEFAULT 1,
        CreatedAt datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT PK_Categories PRIMARY KEY (Id),
        CONSTRAINT UQ_Categories_Slug UNIQUE (Slug)
    );
    
    CREATE INDEX IX_Categories_Slug ON Categories(Slug);
    CREATE INDEX IX_Categories_IsActive ON Categories(IsActive);
END
GO

-- Create Products table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
BEGIN
    CREATE TABLE Products (
        Id int IDENTITY(1,1) NOT NULL,
        Name nvarchar(200) NOT NULL,
        Description nvarchar(2000) NOT NULL,
        Price decimal(18,2) NOT NULL,
        CategoryId int NOT NULL,
        Stock int NOT NULL DEFAULT 0,
        ImageUrl nvarchar(max) NULL,
        Features nvarchar(500) NULL,
        Brand nvarchar(100) NULL,
        Size nvarchar(50) NULL,
        Color nvarchar(50) NULL,
        Material nvarchar(50) NULL,
        IsActive bit NOT NULL DEFAULT 1,
        IsFeatured bit NOT NULL DEFAULT 0,
        CreatedAt datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt datetime2(7) NULL,
        CONSTRAINT PK_Products PRIMARY KEY (Id),
        CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE,
        CONSTRAINT CHK_Products_Price CHECK (Price >= 0),
        CONSTRAINT CHK_Products_Stock CHECK (Stock >= 0)
    );
    
    CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
    CREATE INDEX IX_Products_IsActive ON Products(IsActive);
    CREATE INDEX IX_Products_IsFeatured ON Products(IsFeatured);
    CREATE INDEX IX_Products_Price ON Products(Price);
    CREATE INDEX IX_Products_CreatedAt ON Products(CreatedAt);
END
GO

-- Create AspNetRoles table (Identity Framework)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AspNetRoles' AND xtype='U')
BEGIN
    CREATE TABLE AspNetRoles (
        Id nvarchar(450) NOT NULL,
        Name nvarchar(256) NULL,
        NormalizedName nvarchar(256) NULL,
        ConcurrencyStamp nvarchar(max) NULL,
        CONSTRAINT PK_AspNetRoles PRIMARY KEY (Id)
    );
    
    CREATE UNIQUE INDEX RoleNameIndex ON AspNetRoles(NormalizedName) WHERE NormalizedName IS NOT NULL;
END
GO

-- Create AspNetUsers table (Identity Framework)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AspNetUsers' AND xtype='U')
BEGIN
    CREATE TABLE AspNetUsers (
        Id nvarchar(450) NOT NULL,
        FirstName nvarchar(100) NOT NULL,
        LastName nvarchar(100) NOT NULL,
        Address nvarchar(200) NULL,
        City nvarchar(50) NULL,
        PostalCode nvarchar(10) NULL,
        CreatedAt datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
        LastLoginAt datetime2(7) NULL,
        UserName nvarchar(256) NULL,
        NormalizedUserName nvarchar(256) NULL,
        Email nvarchar(256) NULL,
        NormalizedEmail nvarchar(256) NULL,
        EmailConfirmed bit NOT NULL DEFAULT 0,
        PasswordHash nvarchar(max) NULL,
        SecurityStamp nvarchar(max) NULL,
        ConcurrencyStamp nvarchar(max) NULL,
        PhoneNumber nvarchar(max) NULL,
        PhoneNumberConfirmed bit NOT NULL DEFAULT 0,
        TwoFactorEnabled bit NOT NULL DEFAULT 0,
        LockoutEnd datetimeoffset(7) NULL,
        LockoutEnabled bit NOT NULL DEFAULT 0,
        AccessFailedCount int NOT NULL DEFAULT 0,
        CONSTRAINT PK_AspNetUsers PRIMARY KEY (Id)
    );
    
    CREATE UNIQUE INDEX UserNameIndex ON AspNetUsers(NormalizedUserName) WHERE NormalizedUserName IS NOT NULL;
    CREATE INDEX EmailIndex ON AspNetUsers(NormalizedEmail);
END
GO

-- Create AspNetUserRoles table (Identity Framework)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AspNetUserRoles' AND xtype='U')
BEGIN
    CREATE TABLE AspNetUserRoles (
        UserId nvarchar(450) NOT NULL,
        RoleId nvarchar(450) NOT NULL,
        CONSTRAINT PK_AspNetUserRoles PRIMARY KEY (UserId, RoleId),
        CONSTRAINT FK_AspNetUserRoles_AspNetRoles FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE,
        CONSTRAINT FK_AspNetUserRoles_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
    );
    
    CREATE INDEX IX_AspNetUserRoles_RoleId ON AspNetUserRoles(RoleId);
END
GO

-- Create other Identity tables
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AspNetUserClaims' AND xtype='U')
BEGIN
    CREATE TABLE AspNetUserClaims (
        Id int IDENTITY(1,1) NOT NULL,
        UserId nvarchar(450) NOT NULL,
        ClaimType nvarchar(max) NULL,
        ClaimValue nvarchar(max) NULL,
        CONSTRAINT PK_AspNetUserClaims PRIMARY KEY (Id),
        CONSTRAINT FK_AspNetUserClaims_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
    );
    
    CREATE INDEX IX_AspNetUserClaims_UserId ON AspNetUserClaims(UserId);
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AspNetUserLogins' AND xtype='U')
BEGIN
    CREATE TABLE AspNetUserLogins (
        LoginProvider nvarchar(450) NOT NULL,
        ProviderKey nvarchar(450) NOT NULL,
        ProviderDisplayName nvarchar(max) NULL,
        UserId nvarchar(450) NOT NULL,
        CONSTRAINT PK_AspNetUserLogins PRIMARY KEY (LoginProvider, ProviderKey),
        CONSTRAINT FK_AspNetUserLogins_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
    );
    
    CREATE INDEX IX_AspNetUserLogins_UserId ON AspNetUserLogins(UserId);
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AspNetUserTokens' AND xtype='U')
BEGIN
    CREATE TABLE AspNetUserTokens (
        UserId nvarchar(450) NOT NULL,
        LoginProvider nvarchar(450) NOT NULL,
        Name nvarchar(450) NOT NULL,
        Value nvarchar(max) NULL,
        CONSTRAINT PK_AspNetUserTokens PRIMARY KEY (UserId, LoginProvider, Name),
        CONSTRAINT FK_AspNetUserTokens_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
    );
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AspNetRoleClaims' AND xtype='U')
BEGIN
    CREATE TABLE AspNetRoleClaims (
        Id int IDENTITY(1,1) NOT NULL,
        RoleId nvarchar(450) NOT NULL,
        ClaimType nvarchar(max) NULL,
        ClaimValue nvarchar(max) NULL,
        CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY (Id),
        CONSTRAINT FK_AspNetRoleClaims_AspNetRoles FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
    );
    
    CREATE INDEX IX_AspNetRoleClaims_RoleId ON AspNetRoleClaims(RoleId);
END
GO

-- Create CartItems table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CartItems' AND xtype='U')
BEGIN
    CREATE TABLE CartItems (
        Id int IDENTITY(1,1) NOT NULL,
        UserId nvarchar(450) NOT NULL,
        ProductId int NOT NULL,
        Quantity int NOT NULL DEFAULT 1,
        CreatedAt datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt datetime2(7) NULL,
        CONSTRAINT PK_CartItems PRIMARY KEY (Id),
        CONSTRAINT FK_CartItems_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
        CONSTRAINT FK_CartItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
        CONSTRAINT CHK_CartItems_Quantity CHECK (Quantity > 0)
    );
    
    CREATE INDEX IX_CartItems_UserId ON CartItems(UserId);
    CREATE INDEX IX_CartItems_ProductId ON CartItems(ProductId);
    CREATE UNIQUE INDEX UQ_CartItems_UserProduct ON CartItems(UserId, ProductId);
END
GO

-- Create Orders table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Orders' AND xtype='U')
BEGIN
    CREATE TABLE Orders (
        Id int IDENTITY(1,1) NOT NULL,
        UserId nvarchar(450) NOT NULL,
        TotalAmount decimal(18,2) NOT NULL,
        Status nvarchar(50) NOT NULL DEFAULT 'Pending',
        ShippingAddress nvarchar(200) NOT NULL,
        ShippingCity nvarchar(50) NOT NULL,
        ShippingPostalCode nvarchar(10) NOT NULL,
        PhoneNumber nvarchar(15) NOT NULL,
        PaymentMethod nvarchar(50) NULL,
        PaymentStatus nvarchar(100) NOT NULL DEFAULT 'Pending',
        TransactionId nvarchar(100) NULL,
        Notes nvarchar(500) NULL,
        CreatedAt datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt datetime2(7) NULL,
        CONSTRAINT PK_Orders PRIMARY KEY (Id),
        CONSTRAINT FK_Orders_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
        CONSTRAINT CHK_Orders_TotalAmount CHECK (TotalAmount >= 0),
        CONSTRAINT CHK_Orders_Status CHECK (Status IN ('Pending', 'Confirmed', 'Processing', 'Shipped', 'Delivered', 'Cancelled')),
        CONSTRAINT CHK_Orders_PaymentStatus CHECK (PaymentStatus IN ('Pending', 'Paid', 'Failed', 'Refunded'))
    );
    
    CREATE INDEX IX_Orders_UserId ON Orders(UserId);
    CREATE INDEX IX_Orders_Status ON Orders(Status);
    CREATE INDEX IX_Orders_PaymentStatus ON Orders(PaymentStatus);
    CREATE INDEX IX_Orders_CreatedAt ON Orders(CreatedAt);
    CREATE INDEX IX_Orders_TransactionId ON Orders(TransactionId);
END
GO

-- Create OrderItems table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OrderItems' AND xtype='U')
BEGIN
    CREATE TABLE OrderItems (
        Id int IDENTITY(1,1) NOT NULL,
        OrderId int NOT NULL,
        ProductId int NOT NULL,
        Quantity int NOT NULL,
        Price decimal(18,2) NOT NULL,
        CONSTRAINT PK_OrderItems PRIMARY KEY (Id),
        CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
        CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE NO ACTION,
        CONSTRAINT CHK_OrderItems_Quantity CHECK (Quantity > 0),
        CONSTRAINT CHK_OrderItems_Price CHECK (Price >= 0)
    );
    
    CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
    CREATE INDEX IX_OrderItems_ProductId ON OrderItems(ProductId);
END
GO

-- Create Reviews table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Reviews' AND xtype='U')
BEGIN
    CREATE TABLE Reviews (
        Id int IDENTITY(1,1) NOT NULL,
        UserId nvarchar(450) NOT NULL,
        ProductId int NOT NULL,
        Rating int NOT NULL,
        Comment nvarchar(1000) NULL,
        CreatedAt datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt datetime2(7) NULL,
        CONSTRAINT PK_Reviews PRIMARY KEY (Id),
        CONSTRAINT FK_Reviews_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Reviews_Products FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
        CONSTRAINT CHK_Reviews_Rating CHECK (Rating >= 1 AND Rating <= 5)
    );
    
    CREATE INDEX IX_Reviews_UserId ON Reviews(UserId);
    CREATE INDEX IX_Reviews_ProductId ON Reviews(ProductId);
    CREATE INDEX IX_Reviews_Rating ON Reviews(Rating);
    CREATE UNIQUE INDEX UQ_Reviews_UserProduct ON Reviews(UserId, ProductId);
END
GO

-- Insert default roles
IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Admin')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Admin', 'ADMIN', NEWID());
END
GO

IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Customer')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Customer', 'CUSTOMER', NEWID());
END
GO

-- Insert Categories
IF NOT EXISTS (SELECT * FROM Categories WHERE Name = 'Men''s Fashion')
BEGIN
    INSERT INTO Categories (Name, Slug, Description, IsActive) VALUES
    ('Men''s Fashion', 'mens-fashion', 'Premium panjabis, kurtas, traditional Bengali menswear', 1),
    ('Women''s Fashion', 'womens-fashion', 'Elegant sarees, kurtis, contemporary designs', 1),
    ('Accessories', 'accessories', 'Premium bags, shoes, lifestyle accessories', 1),
    ('Watches', 'watches', 'Luxury timepieces including Sahara and Platinum collections', 1),
    ('Home & Lifestyle', 'home-lifestyle', 'Premium home decor and lifestyle products', 1);
END
GO

-- Insert Products
IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Premium Bengali Panjabi')
BEGIN
    DECLARE @MensFashionId int = (SELECT Id FROM Categories WHERE Slug = 'mens-fashion');
    DECLARE @WomensFashionId int = (SELECT Id FROM Categories WHERE Slug = 'womens-fashion');
    DECLARE @AccessoriesId int = (SELECT Id FROM Categories WHERE Slug = 'accessories');
    DECLARE @WatchesId int = (SELECT Id FROM Categories WHERE Slug = 'watches');

    INSERT INTO Products (Name, Description, Price, CategoryId, Stock, ImageUrl, IsActive, IsFeatured) VALUES
    ('Premium Bengali Panjabi', 'Traditional embroidery with authentic Bengali craftsmanship', 4500.00, @MensFashionId, 25, 'https://via.placeholder.com/400x400/d4af37/000000?text=Premium+Panjabi', 1, 1),
    ('Luxury Silk Kurta', 'Golden threadwork with premium silk fabric', 8900.00, @MensFashionId, 15, 'https://via.placeholder.com/400x400/d4af37/000000?text=Silk+Kurta', 1, 1),
    ('Elegant Silk Saree', 'Traditional motifs with contemporary elegance', 12500.00, @WomensFashionId, 20, 'https://via.placeholder.com/400x400/d4af37/000000?text=Silk+Saree', 1, 1),
    ('Designer Kurti Set', 'Complete set with palazzo pants', 3800.00, @WomensFashionId, 30, 'https://via.placeholder.com/400x400/d4af37/000000?text=Kurti+Set', 1, 1),
    ('Premium Leather Handbag', 'Luxury craftsmanship with elegant design', 6500.00, @AccessoriesId, 12, 'https://via.placeholder.com/400x400/d4af37/000000?text=Leather+Handbag', 1, 1),
    ('Luxury Gold Watch', 'Swiss movement with premium gold plating', 25000.00, @WatchesId, 8, 'https://via.placeholder.com/400x400/d4af37/000000?text=Gold+Watch', 1, 1);
END
GO

-- Create stored procedures for common operations
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetProductsByCategory')
BEGIN
    EXEC('
    CREATE PROCEDURE GetProductsByCategory
        @CategoryId int,
        @PageNumber int = 1,
        @PageSize int = 12,
        @SortBy nvarchar(50) = ''name'',
        @SearchTerm nvarchar(100) = ''''
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
            AND (@SearchTerm = '''' OR p.Name LIKE ''%'' + @SearchTerm + ''%'' OR p.Description LIKE ''%'' + @SearchTerm + ''%'')
        GROUP BY p.Id, p.Name, p.Description, p.Price, p.CategoryId, p.Stock, p.ImageUrl, p.Features, p.Brand, p.Size, p.Color, p.Material, p.IsActive, p.IsFeatured, p.CreatedAt, p.UpdatedAt, c.Name, c.Slug
        ORDER BY 
            CASE WHEN @SortBy = ''name'' THEN p.Name END ASC,
            CASE WHEN @SortBy = ''price_asc'' THEN p.Price END ASC,
            CASE WHEN @SortBy = ''price_desc'' THEN p.Price END DESC,
            CASE WHEN @SortBy = ''newest'' THEN p.CreatedAt END DESC,
            CASE WHEN @SortBy = '''' THEN p.IsFeatured END DESC,
            p.CreatedAt DESC
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;
        
        -- Get total count
        SELECT COUNT(*) as TotalCount
        FROM Products p
        WHERE 
            p.IsActive = 1 
            AND (@CategoryId IS NULL OR p.CategoryId = @CategoryId)
            AND (@SearchTerm = '''' OR p.Name LIKE ''%'' + @SearchTerm + ''%'' OR p.Description LIKE ''%'' + @SearchTerm + ''%'');
    END
    ');
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetOrdersByUser')
BEGIN
    EXEC('
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
    ');
END
GO

-- Create functions for calculations
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'CalculateProductAverageRating')
BEGIN
    EXEC('
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
    ');
END
GO

-- Create triggers for audit trails
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE name = 'tr_Products_UpdatedAt')
BEGIN
    EXEC('
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
    ');
END
GO

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE name = 'tr_Orders_UpdatedAt')
BEGIN
    EXEC('
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
    ');
END
GO

-- Create views for reporting
IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'vw_ProductStats')
BEGIN
    EXEC('
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
    LEFT JOIN Orders o ON oi.OrderId = o.Id AND o.Status IN (''Confirmed'', ''Processing'', ''Shipped'', ''Delivered'')
    WHERE p.IsActive = 1
    GROUP BY p.Id, p.Name, p.Price, p.Stock, c.Name
    ');
END
GO

IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'vw_SalesReport')
BEGIN
    EXEC('
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
    WHERE o.Status IN (''Confirmed'', ''Processing'', ''Shipped'', ''Delivered'')
    GROUP BY YEAR(o.CreatedAt), MONTH(o.CreatedAt)
    ');
END
GO

-- Create indexes for performance optimization
CREATE NONCLUSTERED INDEX IX_Products_Name_Description ON Products(Name, Description);
CREATE NONCLUSTERED INDEX IX_Orders_UserId_Status ON Orders(UserId, Status);
CREATE NONCLUSTERED INDEX IX_Reviews_ProductId_Rating ON Reviews(ProductId, Rating);

-- Create full-text search indexes (if full-text search is enabled)
IF EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = 'IlliyeenCatalog')
BEGIN
    DROP FULLTEXT INDEX ON Products;
    DROP FULLTEXT CATALOG IlliyeenCatalog;
END
GO

IF SERVERPROPERTY('IsFullTextInstalled') = 1
BEGIN
    CREATE FULLTEXT CATALOG IlliyeenCatalog AS DEFAULT;
    CREATE FULLTEXT INDEX ON Products(Name, Description) KEY INDEX PK_Products;
END
GO

-- Grant permissions (adjust as needed for your environment)
-- Note: In production, create specific database users with minimal required permissions

PRINT 'ILLIYEEN Database schema created successfully!';
PRINT 'Database includes:';
PRINT '- Complete table structure with proper relationships';
PRINT '- ASP.NET Core Identity tables';
PRINT '- Sample data for categories and products';
PRINT '- Stored procedures for common operations';
PRINT '- Views for reporting';
PRINT '- Triggers for audit trails';
PRINT '- Performance indexes';
PRINT '- Full-text search support (if available)';
GO
