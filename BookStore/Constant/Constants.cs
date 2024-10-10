namespace BookStore.Constant
{
    // Lớp chứa các hằng số chung cho trạng thái
    public class Constants
    {
        public const string None = "None";       // Trạng thái không có giá trị, dùng khi chưa có kết quả
        public const string Success = "Success"; // Trạng thái thành công, dùng khi một thao tác hoàn tất thành công
        public const string Error = "Error";     // Trạng thái lỗi, dùng khi một thao tác gặp lỗi
    }

        // Lớp chứa các hằng số về vai trò của người dùng trong hệ thống
        public class Role
        {
            public const string Admin = "Admin";   // Vai trò quản trị viên, có quyền cao nhất

            public const string User = "User";     // Vai trò người dùng thông thường, chỉ có quyền truy cập hạn chế
        }

    }
