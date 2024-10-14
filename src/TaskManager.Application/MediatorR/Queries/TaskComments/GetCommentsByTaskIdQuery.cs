using MediatR;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Queries.TaskComments
{
    [ExcludeFromCodeCoverage]
    public class GetCommentsByTaskIdQuery(int taskId) : IRequest<IEnumerable<TaskCommentDto>>
    {
        public int TaskId { get; } = taskId;
    }
}