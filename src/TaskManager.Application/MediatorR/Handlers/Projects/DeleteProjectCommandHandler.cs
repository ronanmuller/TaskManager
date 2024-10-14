using MediatR;
using TaskManager.Application.MediatorR.Commands.Projects;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.MediatorR.Handlers.Projects
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IProjectService _projectService;

        public DeleteProjectCommandHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            await _projectService.DeleteProjectAsync(request.ProjectId);
        }
    }
}