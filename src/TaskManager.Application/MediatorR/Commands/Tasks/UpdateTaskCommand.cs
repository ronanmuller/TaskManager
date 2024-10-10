using MediatR;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Commands.Tasks
{
    public class UpdateTaskCommand(int taskId, UpdateTaskDto taskDto) : IRequest<TaskDto>
    {
        public int TaskId { get; init; } = taskId; 
        public UpdateTaskDto TaskDto { get; init; } = taskDto; 
    }
}