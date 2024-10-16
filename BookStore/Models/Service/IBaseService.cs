using System.Linq.Expressions;

namespace BookStore.Models.Service
{
    public interface IBaseService<T>
    {
        // Lấy đối tượng IQueryable<T> để thực hiện truy vấn trên tập hợp dữ liệu
        IQueryable<T> GetDbSet();

        // Lấy tất cả các đối tượng từ cơ sở dữ liệu
        List<T> GetAll();

        // Lấy một đối tượng theo ID
        Task<T> GetEntityById(int id);

        // Lấy một đối tượng 
        Task<T> Get(Expression<Func<T, bool>> expresstion);

        // Lấy danh sách đối tượng 
        Task<List<T>> GetList(Expression<Func<T, bool>> expresstion);

        // Kiểm tra xem có tồn tại
        Task<bool> Exist(Expression<Func<T, bool>> expresstion);

        // Đếm số lượng đối tượng
        Task<int> Count(Expression<Func<T, bool>> expresstion);

        // Thêm một đối tượng vào cơ sở dữ liệu
        Task Insert(T entity);

        // Thêm nhiều đối tượng vào cơ sở dữ liệu
        Task InsertMulti(List<T> entities);

        // Cập nhật một đối tượng trong cơ sở dữ liệu
        Task Update(T entity);

        // Xóa một đối tượng theo ID
        Task Delete(int id);
    }
}
