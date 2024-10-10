using TaskManager.Application.Dto;

namespace TaskManager.Application.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjectsByUserIdAsync(int userId, int skip, int take);

        Task<ProjectDto> CreateProjectAsync(CreateProjectDto project);

        Task DeleteProjectAsync(int projectId);
    }
}