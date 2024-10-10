using MediatR;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.Tasks;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.MediatorR.Handlers.Tasks;

public class CreateTaskCommandHandler(ITaskService taskService) : IRequestHandler<CreateTaskCommand, TaskDto>
{
    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var createdTask = await taskService.CreateTaskAsync(request.TaskDto);
        return createdTask; 
    }
}