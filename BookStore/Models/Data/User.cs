using BookStore.Constant;
using System.ComponentModel.DataAnnotations;
using static BookStore.Constant.Enumerations;

namespace BookStore.Models.Data
{
    public class User : BaseData
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string UserName { get; set; } //tên đăng nhập

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên không được để trống")]
        public string FirstName { get; set; }//tên

        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ không được để trống")]
        public string LastName { get; set; } //Họ

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email không được để trống")] 
        public string Email { get; set; } //Email
        [Required(ErrorMessage = "Password không được để trống")]
        public string Password { get; set; } //Mật khẩu

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDay { get; set; } //Ngày sinh


        [DataType(DataType.PhoneNumber)]
        public string DienThoai { get; set; }

        public GenderEnum? Gender { get; set; } //giới tính

        public string? Infomation { get; set; } //thông tin
        public bool IsDelete { get; set; } = false; // đã xóa hay chưa
        public bool IsActive { get; set; } = true; //đang hoạt động hay không
        public RoleEnum RoleType { get; set; } = RoleEnum.User;
        public string? RoleName
        {
            // Phương thức getter để lấy giá trị của RoleName
            get
            {
                switch (RoleType)
                {
                    // Nếu RoleType là User, trả về chuỗi "Người dùng"
                    case RoleEnum.User:
                        return "Người dùng";
                        //Nếu RoleType là Admin, trả về chuỗi "Admin"
                    case RoleEnum.Admin:
                        return "Admin";
                    default:
                        return "";
                }
            }
            set { }
        }

    }
}
