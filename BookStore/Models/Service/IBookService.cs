using BookStore.Models.Data;
using System.Linq.Expressions;

namespace BookStore.Models.Service
{
    public interface IBookService : IBaseService<Book>
    {
        List<Book> GetBookActiveInCategoryActive(Expression<Func<Book, bool>> expresstion);
        List<Book> GetAllBooks();


    }
}
