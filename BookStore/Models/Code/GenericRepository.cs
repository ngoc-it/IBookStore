using BookStore.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Models.Code
{ // Định nghĩa lớp GenericRepository<T>, nơi T là kiểu tổng quát kế thừa từ BaseData
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseData
    {
        // Các trường dữ liệu để lưu trữ DbSet và DbContext
        protected readonly DbSet<T> _dbSet; //Tập hợp thực thể

        protected readonly IDbContext _dbContext; //ngữ cảnh dữ liệu

        public GenericRepository( IDbContext context) // Constructor nhận vào một IDbContext để khởi tạo _dbSet và _dbContext
        {
            _dbContext = context; //Khởi tạo Db context
            _dbSet = context.Set<T>(); // Khởi tạo _dbSet với kiểu T

        }

        public void Add(T entity) //Phương thức thêm
        {
            entity.Id = 0; //tạo mới 
            entity.CreatedDate = DateTime.Now; //ngày tạo
            entity.UpdatedDate = DateTime.Now; // ngày cập nhật
            _dbSet.Add(entity); //thêm vào dataset
        }

        public void AddRange(List<T> entity) //thêm nhiều thực thể
        {
            if (entity != null && entity.Any()) //kt thực thể có khác null và không rỗng ko
            {
                entity = entity.Select(x => { x.Id = 0; x.CreatedDate = DateTime.Now; x.UpdatedDate = DateTime.Now; return x; }).ToList();
                // Đặt ID về 0 và gán ngày tạo, ngày cập nhật cho mỗi thực thể
            }

            _dbSet.AddRange(entity);
        }

        public async Task<int> Count(Expression<Func<T, bool>> expresstion) //đếm
        {
            return await _dbSet.CountAsync(expresstion);//trả về số lượng
        }

        public async Task Delete(T entity) //xóa thực thể
        {
            _dbSet.Attach(entity); //gắn thực thể vào ngữ cảnh
            await Task.FromResult(_dbSet.Remove(entity).Entity); //xóa thực thể khỏi data set
        }

        public async Task DeleteRange(List<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public async Task<bool> Exist(Expression<Func<T, bool>> expresstion)
        {
            return await _dbSet.AnyAsync(expresstion);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expresstion)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(expresstion);

            if (entity != null)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public async Task<T> GetById(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public IQueryable<T> GetDbSet()
        {
            return _dbContext.Set<T>() as IQueryable<T>;
        }

        public async Task<List<T>> GetList(Expression<Func<T, bool>> expresstion)
        {
            var lisT = await _dbSet.Where(expresstion).ToListAsync();
            return lisT;
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _dbSet.Update(entity);
        }

        public void UpdateRange(List<T> entity)
        {
            if (entity != null && entity.Any())
            {
                entity = entity.Select(x => { x.UpdatedDate = DateTime.Now; return x; }).ToList();
            }
            _dbSet.UpdateRange(entity);
        }
    }
   
}
