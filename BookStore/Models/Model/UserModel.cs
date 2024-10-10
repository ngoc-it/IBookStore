using BookStore.Models.Data;
using System.ComponentModel.DataAnnotations;
using static BookStore.Constant.Enumerations;

namespace BookStore.Models.Model
{
    public class UserModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class RegisterModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập không được để trống")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ không được để trống")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên không được để trống")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email không được để trống")]

        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được để trống")]
   
        [DataType(DataType.Password)]
        public string Password { get; set; }

       [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
        public string? DienThoai { get; set; }
    }
    public class UserInfomationModel : BaseData
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập không được để trống")]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ không được để trống")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên không được để trống")]
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email không được để trống")]
        
        public string Email { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDay { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string? DienThoai { get; set; }

        public GenderEnum? Gender { get; set; }
        public string? Infomation { get; set; }
    }
}
