using BookStore.Models.Code;
using BookStore.Models.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace BookStore.Models.Service
{
    public class BaseService<T> : IBaseService<T> where T : BaseData
    {
        protected readonly IGenericRepository<T> _baseRepo;
        private readonly ILogger<T> _logger;
        public BaseService(IGenericRepository<T> baseRepo, ILogger<T> logger)
        {
            _baseRepo = baseRepo;
            _logger = logger;
        }
        public async virtual Task<int> Count(Expression<Func<T, bool>> expresstion)
        {
            return await _baseRepo.Count(expresstion); 
        }

        public async virtual Task Delete(int id)
        {
            var entity = await _baseRepo.GetById(id);
            if (entity != null)
            {
                await _baseRepo.Delete(entity);
                await _baseRepo.SaveChangeAsync();

                await this.AfterSaveDeleteEntity(entity);
            }
        }

        public async virtual Task<bool> Exist(Expression<Func<T, bool>> expresstion)
        {
            return await _baseRepo.Exist(expresstion);
        }

        public async virtual Task<T> Get(Expression<Func<T, bool>> expresstion)
        {
            return await _baseRepo.Get(expresstion);
        }

        public virtual List<T> GetAll() // Lấy toàn bộ dữ liệu
        {
            var entities = _baseRepo.GetDbSet().ToList();

            return entities.ToList();
        }

        public virtual IQueryable<T> GetDbSet()
        {
            var entities = _baseRepo.GetDbSet();

            return entities;
        }

        public async virtual Task<T> GetEntityById(int id)
        {
            return await _baseRepo.GetById(id);
        }

        public async virtual Task<List<T>> GetList(Expression<Func<T, bool>> expresstion)
        {
            return await _baseRepo.GetList(expresstion);
        }

        public async virtual Task Insert(T entity)
        {
            await this.PrepareBeforeInsert(entity);

            _baseRepo.Add(entity);
            await _baseRepo.SaveChangeAsync();

            await this.AfterSaveInsertEntity(entity);
        }

        public async virtual Task InsertMulti(List<T> entities)
        {
            if (entities.Any())
            {
                foreach (T entity in entities)
                {
                    await this.PrepareBeforeInsert(entity);
                }

                _baseRepo.AddRange(entities);
                await _baseRepo.SaveChangeAsync();

                foreach (T entity in entities)
                {
                    await this.AfterSaveInsertEntity(entity);
                }
            }
        }

        public async virtual Task Update(T entity)
        {
            this.PrepareBeforeUpdate(entity);

            _baseRepo.Update(entity);
            await _baseRepo.SaveChangeAsync();

            await this.AfterSaveUpdateEntity(entity);
        }
        public async virtual Task PrepareBeforeInsert(T entity)
        {
            entity.Id = 0;
            entity.CreatedDate = DateTime.Now;
        }
        public virtual void PrepareBeforeUpdate(T entity)
        {
            entity.UpdatedDate = DateTime.Now;
        }
        public virtual async Task AfterSaveInsertEntity(T entity)
        {

        }
        public virtual async Task AfterSaveUpdateEntity(T entity)
        {

        }
        public virtual async Task AfterSaveDeleteEntity(T entity)
        {

        }

    }
}
