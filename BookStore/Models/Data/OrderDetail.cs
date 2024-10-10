namespace BookStore.Models.Data
{
    public class OrderDetail : BaseData
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string BookImage { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public int PriceBuy { get; set; }
    }
}
