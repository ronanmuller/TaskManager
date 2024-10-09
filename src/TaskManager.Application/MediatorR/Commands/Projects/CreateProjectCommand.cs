using MediatR;
using TaskManager.Application.Dto;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.MediatorR.Commands.Projects
{
    public class CreateProjectCommand(string name, int userId) : IRequest<Project>, IRequest<ProjectDto>
    {
        public string Name { get; set; } = name;
        public int UserId { get; set; } = userId;
    }
}