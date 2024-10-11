using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository(ReadContext readContext, WriteContext writeContext) 
        : Repository<Tasks>(writeContext, readContext), ITaskRepository
    {
        private readonly ReadContext _readContext = readContext;
        private readonly WriteContext _writeContext = writeContext;

        public async Task<bool> HasPendingTasksAsync(int projectId)
        {
            return await _readContext.Tasks
                .AnyAsync(t => t.ProjectId == projectId &&
                               (t.Status == TaskState.Pending || t.Status == TaskState.InProgress));
        }

        public async Task<IEnumerable<Tasks>> GetTasksByProjectIdAsync(int projectId, int skip, int take)
        {
            return await _readContext.Tasks
                .AsNoTracking() 
                .Where(t => t.ProjectId == projectId) // Filtra tarefas pelo ID do projeto
                .Skip(skip).Take(take).ToListAsync(); 
        }

        public async Task<int> CountTasksByProjectIdAsync(int projectId)
        {
            return await _readContext.Tasks.CountAsync(t => t.ProjectId == projectId);
        }
    }
}
