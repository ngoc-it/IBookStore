namespace BookStore.Models.Data
{
    public class Cart : BaseData
    { 
        public int UserId { get; set; } //id user
        public int BookId { get; set; } //Id sách
        public int Quantity { get; set; } //số lượng
    }
}
