using MediatR;
using TaskManager.Application.MediatorR.Commands.Projects;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.MediatorR.Handlers.Projects
{
    public class DeleteProjectCommandHandler(IProjectService projectService) : IRequestHandler<DeleteProjectCommand>
    {
        public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            await projectService.DeleteProjectAsync(request.ProjectId);
        }
    }
}