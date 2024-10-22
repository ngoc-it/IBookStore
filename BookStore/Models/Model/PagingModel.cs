using BookStore.Models.Data;
using X.PagedList;

namespace BookStore.Models.Model
{
    public class PagingModel<T> where T : class
    {
        public int TotalRecord { get; set; }
        public IPagedList<T> DataPaging { get; set; }
    }
 /*   public class NewsPagingModel
    {
        public string? Keyword { get; set; }
        public PagingModel<News> Paging { get; set; }
    }*/



}
/*using directives: Chỉ định các không gian tên cần thiết cho mô hình này.
PagingModel<T>: Là lớp tổng quát giúp quản lý dữ liệu phân trang cho bất kỳ loại đối tượng nào. Việc sử dụng <T> cho phép lớp này trở nên linh hoạt hơn.
TotalRecord: Lưu trữ tổng số bản ghi, hữu ích cho việc hiển thị thông tin về số lượng mục có sẵn.
DataPaging: Lưu trữ danh sách đã được phân trang và thông tin về phân trang.
NewsPagingModel: Cụ thể hóa mô hình cho tin tức, bao gồm khả năng tìm kiếm và thông tin phân trang.
Keyword: Được sử dụng để lưu trữ từ khóa tìm kiếm, có thể không có giá trị (nullable).
Paging: Lưu trữ thông tin phân trang cho danh sách tin tức, giúp quản lý và truy xuất dữ liệu một cách hiệu quả.*/