using MediatR;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Commands.Tasks
{
    public class CreateTaskCommand(CreateTaskDto taskDto) : IRequest<TaskDto>
    {
        public CreateTaskDto TaskDto { get; init; } = taskDto;
    }
}

