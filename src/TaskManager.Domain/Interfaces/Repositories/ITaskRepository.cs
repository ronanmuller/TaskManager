using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IRepository<Tasks>
    {
        Task<bool> HasPendingTasksAsync(int projectId);
    }

}
