namespace TaskManager.Domain.Interfaces.Repositories
{
    public interface IProjectTaskRepository
    {
        Task RemoveByProjectIdAsync(int projectId);
    }
}