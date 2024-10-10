using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IRepository<Tasks>
    {
        Task<bool> HasPendingTasksAsync(int projectId);
        Task<IEnumerable<Tasks>> GetTasksByProjectIdAsync(int projectId, int skip, int take);
        Task<int> CountTasksByProjectIdAsync(int projectId);
    }

}
