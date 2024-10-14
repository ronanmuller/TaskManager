using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskCommentRepository(ReadContext readContext, WriteContext writeContext)
        : Repository<TaskComment>(writeContext, readContext), ITaskCommentRepository
    {
        private readonly ReadContext _readContext = readContext;
        private readonly WriteContext _writeContext = writeContext;

        public async Task AddCommentAsync(TaskComment comment)
        {
            await _writeContext.TaskComments.AddAsync(comment);
            await _writeContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskComment>> GetCommentsByTaskIdAsync(int taskId)
        {
            return await _readContext.TaskComments
                .AsNoTracking() 
                .Where(c => c.TaskId == taskId)
                .ToListAsync();
        }

    }
}