using MediatR;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Commands.Projects
{
    public class CreateProjectCommand(CreateProjectDto projectDto) : IRequest<ProjectDto>
    {
        public CreateProjectDto ProjectDto { get; } = projectDto;
    }
}