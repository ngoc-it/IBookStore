namespace BookStore.Models.Data
{
    public class Delivery : BaseData
    { 
        public int DeliveryId { get; set; } //mã vận chuyển
        public string DeliveryName { get; set; } //Tên 
        public int Cost { get; set; } //phí vận chuyển
        public bool IsActive { get; set; } //trạng thái
    }
}
