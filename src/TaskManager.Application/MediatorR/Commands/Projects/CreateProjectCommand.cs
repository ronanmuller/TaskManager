using System.Diagnostics.CodeAnalysis;
using MediatR;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Commands.Projects
{
    [ExcludeFromCodeCoverage]
    public class CreateProjectCommand(CreateProjectDto projectDto) : IRequest<ProjectDto>
    {
        public CreateProjectDto ProjectDto { get; } = projectDto;
    }
}