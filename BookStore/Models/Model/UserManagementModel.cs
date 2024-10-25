using BookStore.Models.Data;

namespace BookStore.Models.Model
{
    public class UserManagementModel : User
    {
        public int TotalOrder { get; set; }
/*        public int TotalMoney { get; set; }*/

    }
    public class UserPagingModel
    {
        public string? Keyword { get; set; }  // Từ khóa tìm kiếm (có thể là null)
        public PagingModel<UserManagementModel> Paging { get; set; }  // Thông tin phân trang cho danh sách người dùng
    }
}
