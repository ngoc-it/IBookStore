using System.ComponentModel.DataAnnotations;
using static BookStore.Constant.Enumerations;

namespace BookStore.Models.Model
{
    public class CartConfirmModel
    {
        public string OrderCode { get; set; }
        public int DeliveryId { get; set; }
        public int? VoucherId { get; set; }
        public int ShipCost { get; set; }
        public int Discount { get; set; }
        public int TotalMoney { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Người nhận không được để trống")]
        public string CustomerName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được để trống")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Địa chỉ không được để trống")]
        public string CustomerAddress { get; set; }
        public string? OrderNote { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
