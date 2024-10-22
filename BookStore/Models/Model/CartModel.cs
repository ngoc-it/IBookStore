using BookStore.Models.Data;

namespace BookStore.Models.Model
{
    public class CartModel
    {
        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
    }
    public class CartItemModel : Cart
    {
        public string BookName { get; set; }
        public string BookImage { get; set; }
        public int Price { get; set; }
        public int? PriceOriginal { get; set; }
        public int Quantity { get; set; }
        public int MaxQuantity { get; set; }
        public int TotalMoney
        {
            get
            {
                return Quantity * Price;
            }
            set { }
        }
        public string? ErrorMessage { get; set; }
    }
}
