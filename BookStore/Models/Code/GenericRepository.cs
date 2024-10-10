using BookStore.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Models.Code
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseData
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly IDbContext _dbContext;

        public GenericRepository( IDbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();

        }

        public void Add(T entity)
        {
            entity.Id = 0;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            _dbSet.Add(entity);
        }

        public void AddRange(List<T> entity)
        {
            if (entity != null && entity.Any())
            {
                entity = entity.Select(x => { x.Id = 0; x.CreatedDate = DateTime.Now; x.UpdatedDate = DateTime.Now; return x; }).ToList();
            }

            _dbSet.AddRange(entity);
        }

        public async Task<int> Count(Expression<Func<T, bool>> expresstion)
        {
            return await _dbSet.CountAsync(expresstion);
        }

        public async Task Delete(T entity)
        {
            _dbSet.Attach(entity);
            await Task.FromResult(_dbSet.Remove(entity).Entity);
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
