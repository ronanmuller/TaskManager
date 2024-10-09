using TaskManager.Application.Dto;

namespace TaskManager.Application.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjectsByUserIdAsync(int userId);

        Task<ProjectDto> CreateProjectAsync(ProjectDto project);

        Task DeleteProjectAsync(int projectId);
    }
}