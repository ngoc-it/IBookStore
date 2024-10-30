using static BookStore.Constant.Enumerations;

namespace BookStore.Models.Data
{
    public class Order : BaseData
    {
        public int UserId { get; set; }
        public string OrderCode { get; set; }
        public int DeliveryId { get; set; }
        public int ShipCost { get; set; }
        public int? VoucherId { get; set; }
        public int Discount { get; set; }
        public int TotalMoney { get; set; }
        public OrderStatus Status { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerAddress { get; set; }
        public string? OrderNote { get; set; }
        public PaymentType PaymentType { get; set; }
        public string PaymentName
        {
            get
            {
                switch (PaymentType)
                {
                    case PaymentType.Cod:
                        return "Thanh toán khi nhận hàng";
                    case PaymentType.Online:
                        return "Thanh toán Online";
                    default:
                        return "";
                }
            }
            set { }
        }
        public string? CancelReason { get; set; }
    }
}
