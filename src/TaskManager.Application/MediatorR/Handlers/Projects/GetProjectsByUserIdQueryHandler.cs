using AutoMapper;
using MediatR;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Queries.Projects;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.MediatorR.Handlers.Projects;

public class GetProjectsByUserIdQueryHandler(IProjectService projectService, IMapper mapper) : IRequestHandler<GetProjectsByUserIdQuery, IEnumerable<ProjectDto>>
{
    public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var projects = await projectService.GetProjectsByUserIdAsync(request.UserId, request.Skip, request.Take);

        return mapper.Map<IEnumerable<ProjectDto>>(projects);
    }
}