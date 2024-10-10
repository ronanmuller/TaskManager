using MediatR;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.Tasks;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.MediatorR.Handlers.Tasks;

public class UpdateTaskCommandHandler(ITaskService taskService) : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        return await taskService.UpdateTaskAsync(request.TaskId, request.TaskDto);
    }
}