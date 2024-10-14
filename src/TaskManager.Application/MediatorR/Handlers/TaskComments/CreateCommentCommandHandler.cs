using MediatR;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.TaskComments;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.MediatorR.Handlers.TaskComments
{
    public class CreateCommentCommandHandler(ITaskCommentService taskCommentService)
        : IRequestHandler<CreateCommentCommand, int>
    {
        public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var createCommentDto = new CreateCommentDto
            {
                Comment = request.Comment,
                TaskId = request.TaskId
            };

            var commentDto = await taskCommentService.CreateCommentAsync(request.TaskId, createCommentDto);
            return commentDto.Id;
        }
    }
}