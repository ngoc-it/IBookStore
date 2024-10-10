using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Data
{
    public class Contact : BaseData
    {
        public int ContactId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; } //Email
        [DataType(DataType.PhoneNumber)]
        public string DienThoai { get; set; }

        [Required(ErrorMessage = "Nội dung cần nhập của bạn")]
        public string Comment { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? NgayCapNhat { get; set; }
    }
}
