using static BookStore.Constant.Enumerations;

namespace BookStore.Models.Data
{
    public class BookReview : BaseData
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public ApproveStatus Status { get; set; }
    }
}
