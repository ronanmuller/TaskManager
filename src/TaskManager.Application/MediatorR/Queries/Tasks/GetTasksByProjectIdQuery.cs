using MediatR;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Queries.Tasks;

[ExcludeFromCodeCoverage]
public class GetTasksByProjectIdQuery(int projectId, int skip, int take) : IRequest<IEnumerable<TaskDto>>
{
    public int ProjectId { get; private set; } = projectId;
    public int Skip { get; private set; } = skip;
    public int Take { get; private set; } = take;
}