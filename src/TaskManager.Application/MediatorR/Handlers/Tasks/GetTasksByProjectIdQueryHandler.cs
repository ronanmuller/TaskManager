using MediatR;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Queries.Tasks;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.MediatorR.Handlers.Tasks;

public class GetTasksByProjectIdQueryHandler(ITaskService taskService) : IRequestHandler<GetTasksByProjectIdQuery, IEnumerable<TaskDto>>
{
    public async Task<IEnumerable<TaskDto>> Handle(GetTasksByProjectIdQuery request, CancellationToken cancellationToken)
    {
        return await taskService.GetTasksByProjectIdAsync(request.ProjectId, request.Skip, request.Take);
    }
}