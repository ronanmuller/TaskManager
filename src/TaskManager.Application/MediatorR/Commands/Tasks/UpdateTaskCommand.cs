using MediatR;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Commands.Tasks
{
    [ExcludeFromCodeCoverage]
    public class UpdateTaskCommand(int taskId, UpdateTaskDto taskDto) : IRequest<TaskDto>
    {
        public int TaskId { get; init; } = taskId; 
        public UpdateTaskDto TaskDto { get; init; } = taskDto; 
    }
}