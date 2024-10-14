using MediatR;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Commands.Tasks
{
    [ExcludeFromCodeCoverage]
    public class CreateTaskCommand(CreateTaskDto taskDto) : IRequest<TaskDto>
    {
        public CreateTaskDto TaskDto { get; init; } = taskDto;
    }
}

