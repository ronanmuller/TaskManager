using Microsoft.EntityFrameworkCore.Storage;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories.UnitOfWork
{
    public class UnitOfWork(WriteContext writeContext, ReadContext readContext) : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;

        public ITaskRepository Tasks { get; private set; } = new TaskRepository(readContext, writeContext);
        public IProjectRepository Projects { get; private set; } = new ProjectRepository(readContext, writeContext);
        public ITaskUpdateHistoryRepository TaskUpdateHistories { get; private set; } = new TaskUpdateHistoryRepository(readContext, writeContext);

        public async Task BeginTransactionAsync()
        {
            _transaction = await writeContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                _transaction.Dispose(); 
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose(); 
                _transaction = null; 
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await writeContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                RollbackAsync().Wait(); 
            }
            writeContext.Dispose();
            readContext.Dispose(); 
        }
    }
}
