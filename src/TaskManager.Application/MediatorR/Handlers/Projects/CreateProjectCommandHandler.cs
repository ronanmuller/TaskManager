using MediatR;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.Projects;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.MediatorR.Handlers.Projects
{
    public class CreateProjectCommandHandler(IProjectService projectService)
        : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var createdProject = await projectService.CreateProjectAsync(request.ProjectDto);
            return createdProject; 
        }
    }
}