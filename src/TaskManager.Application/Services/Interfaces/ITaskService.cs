using TaskManager.Application.Dto;

namespace TaskManager.Application.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(int projectId, int skip, int take);
        Task<TaskDto> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto);
    }
}