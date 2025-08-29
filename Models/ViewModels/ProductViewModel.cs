namespace Illiyeen.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; } = null!;
        public IEnumerable<Product> RelatedProducts { get; set; } = new List<Product>();
        public IEnumerable<Review> Reviews { get; set; } = new List<Review>();
        public bool CanReview { get; set; }
        public Review? UserReview { get; set; }
    }

    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public Category? SelectedCategory { get; set; }
        public string? SearchQuery { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalProducts { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
}
