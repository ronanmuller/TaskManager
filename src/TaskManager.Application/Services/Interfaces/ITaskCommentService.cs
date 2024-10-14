using System.Threading.Tasks;
using System.Collections.Generic;
using TaskManager.Application.Dto;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services.Interfaces
{
    public interface ITaskCommentService
    {
        Task<TaskCommentDto> CreateCommentAsync(int taskId, CreateCommentDto addCommentDto);
        Task<IEnumerable<TaskCommentDto>> GetCommentsByTaskIdAsync(int taskId);
    }
}