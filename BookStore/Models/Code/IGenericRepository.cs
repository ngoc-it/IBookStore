using System.Linq.Expressions;

namespace BookStore.Models.Code
{
    public interface IGenericRepository <T>
    {
        void Add(T entity); //Thêm
        void Update(T entity); //Cập nhật
        void AddRange(List<T> entity); //Danh sách thực thể cần thêm
        void UpdateRange(List<T> entity); //Cập nhật danh sách thực thể
        Task Delete(T entity); //Xóa thực thể
        Task DeleteRange(List<T> entity); //Xóa danh sách thực thể
        Task<T> Get(Expression<Func<T, bool>> expresstion); //tìm kiếm thực thể
        Task<List<T>> GetList(Expression<Func<T, bool>> expresstion);  //trả về danh sách thực thể cần tìm
        Task<T> GetById(int id); //Lấy thực thể theo Id
        IQueryable<T> GetDbSet(); //Lấy tập hợp các thực thể
        Task<int> Count(Expression<Func<T, bool>> expresstion); //Đếm số lượng thực thể phù hợp yêu cầu
        Task<bool> Exist(Expression<Func<T, bool>> expresstion); //Xem có tồn tại hay không
        Task<int> SaveChangeAsync(); //Số lượng bản ghi được lưu
    }
}
