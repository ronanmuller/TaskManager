
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId, int skip, int take);
        Task DeleteLogicalProjectsByIdAsync(int id);

        Task<bool> ExistsAsync(int projectId);

    }

}
