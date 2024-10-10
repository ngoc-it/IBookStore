namespace BookStore.Models.Data
{
    public class Voucher : BaseData
    {
        public string VoucherId { get; set; }
        public string VoucherName { get; set; }
        public int Quantity { get; set; } //số lượng mã
        public int UsedNumber { get; set; }  //số lượng mã đã dùng
        public int Discount { get; set; }// giảm bao nhiêu
        public int MinAmount { get; set; } //đơn tối thiểu
        public bool IsActive { get; set; } //trạng thái

    }
}
