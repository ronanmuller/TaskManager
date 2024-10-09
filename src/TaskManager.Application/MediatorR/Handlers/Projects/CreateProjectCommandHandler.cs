using MediatR;
using TaskManager.Application.MediatorR.Commands.Projects;
using TaskManager.Application.Services.Interfaces;
using AutoMapper;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Handlers.Projects
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public CreateProjectCommandHandler(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new ProjectDto
            {
                Name = request.Name,
                UserId = request.UserId
            };

            var createdProject = await _projectService.CreateProjectAsync(project);

            var projectDto = _mapper.Map<ProjectDto>(createdProject);

            return projectDto;
        }
    }
}