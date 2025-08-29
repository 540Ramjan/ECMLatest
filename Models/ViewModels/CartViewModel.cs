namespace Illiyeen.Models.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal TotalAmount => CartItems.Sum(item => item.TotalPrice);
        public int TotalItems => CartItems.Sum(item => item.Quantity);
    }

    public class CheckoutViewModel
    {
        public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string ShippingCity { get; set; } = string.Empty;
        public string ShippingPostalCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
