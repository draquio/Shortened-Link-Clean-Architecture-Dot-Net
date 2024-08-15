using Microsoft.EntityFrameworkCore;
using ShortenedLinks.Domain.Interfaces.Repositories;
using ShortenedLinks.Infrastructure.Persistence;

namespace ShortenedLinks.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ShortenedLinksDbContext _dbContext;
        public GenericRepository(ShortenedLinksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<T>> GetAll(int page, int pageSize)
        {
            try
            {
                List<T> entities = await _dbContext.Set<T>().Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return entities;
            }
            catch
            {
                throw;
            }
        }
        public async Task<T> GetById(int id)
        {
            try
            {
                T? entity = await _dbContext.Set<T>().FindAsync(id);
                return entity;
            }
            catch
            {
                throw;
            }
        }
        public async Task<T> Create(T entity)
        {
            try
            {
                _dbContext.Set<T>().Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Update(T entity)
        {
            try
            {
                _dbContext.Set<T>().Update(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Delete(T entity)
        {
            try
            {
                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
