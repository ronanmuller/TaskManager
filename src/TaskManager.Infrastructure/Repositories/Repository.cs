using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly WriteContext _writeContext;
        private readonly ReadContext _readContext;
        private readonly DbSet<T> _dbSet;

        protected Repository(WriteContext writeContext, ReadContext readContext)
        {
            _writeContext = writeContext;
            _readContext = readContext;
            _dbSet = writeContext.Set<T>(); 
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _readContext.Set<T>().FindAsync(id); 
        }

        public async Task<IEnumerable<T?>> GetAllAsync()
        {
            return await _readContext.Set<T>().ToListAsync(); 
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _writeContext.SaveChangesAsync(); 
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _writeContext.SaveChangesAsync(); 
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null) _dbSet.Remove(entity);
            await _writeContext.SaveChangesAsync(); 
        }
    }
}