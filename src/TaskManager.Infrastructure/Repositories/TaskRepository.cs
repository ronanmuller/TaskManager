using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository : Repository<Tasks>, ITaskRepository
    {
        private readonly ReadContext _context;

        public TaskRepository(ReadContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> HasPendingTasksAsync(int projectId)
        {
            return await _context.ProjectTasks
                .AnyAsync(pt => pt.ProjectId == projectId && pt.Task.Status == TaskState.Pending);
        }
    }
}