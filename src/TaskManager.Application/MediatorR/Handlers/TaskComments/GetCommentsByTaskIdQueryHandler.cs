using MediatR;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Queries.TaskComments;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.MediatorR.Handlers.TaskComments
{
    public class GetCommentsByTaskIdQueryHandler(ITaskCommentService taskCommentService)
        : IRequestHandler<GetCommentsByTaskIdQuery, IEnumerable<TaskCommentDto>>
    {
        public async Task<IEnumerable<TaskCommentDto>> Handle(GetCommentsByTaskIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await taskCommentService.GetCommentsByTaskIdAsync(request.TaskId);
            return comments; // Retorna os comentário
        }
    }
}