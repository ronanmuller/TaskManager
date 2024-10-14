using AutoMapper;
using TaskManager.Application.Dto;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Services;

public class TaskCommentService(ITaskCommentRepository commentRepository, IUserService userService, IMapper mapper): ITaskCommentService
{
    public async Task<TaskCommentDto> CreateCommentAsync(int taskId, CreateCommentDto addCommentDto)
    {
        var comment = new TaskComment
        {
            TaskId = taskId,
            Comment = addCommentDto.Comment,
            CreatedAt = DateTime.UtcNow,
            UserId = userService.GetCurrentUserId() // Obtendo o UserId do usuário atual
        };

        await commentRepository.AddCommentAsync(comment);

        return new TaskCommentDto
        {
            Id = comment.Id,
        };
    }

    public async Task<IEnumerable<TaskCommentDto>> GetCommentsByTaskIdAsync(int taskId)
    {
        var comments = await commentRepository.GetCommentsByTaskIdAsync(taskId);
        return mapper.Map<IEnumerable<TaskCommentDto>>(comments);
    }
}