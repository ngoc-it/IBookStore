using BookStore.Models.Data;

namespace BookStore.Models.Code
{
    public interface IUserConfig
    {
            User GetUserConfig(); // Lấy thông tin cấu hình của người dùng.
            int GetUserId(); //Id của người dùng

    }
}
