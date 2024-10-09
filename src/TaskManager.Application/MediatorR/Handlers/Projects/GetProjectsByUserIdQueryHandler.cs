using MediatR;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Queries.Projects;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.MediatorR.Handlers.Projects
{
    public class GetProjectsByUserIdQueryHandler(IProjectService projectService) : IRequestHandler<GetProjectsByUserIdQuery, IEnumerable<ProjectDto>>
    {
        public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var projects = await projectService.GetProjectsByUserIdAsync(request.UserId);

            return projects.Select(p => new ProjectDto { Id = p.Id, Name = p.Name, UserId = p.UserId });
        }
    }
}