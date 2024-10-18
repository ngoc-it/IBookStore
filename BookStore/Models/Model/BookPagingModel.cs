using BookStore.Models.Data;

namespace BookStore.Models.Model
{ //tìm kiếm sách
    public class BookPagingModel
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public PagingModel<Book> Paging { get; set; } //phân trang sách
    }
}
