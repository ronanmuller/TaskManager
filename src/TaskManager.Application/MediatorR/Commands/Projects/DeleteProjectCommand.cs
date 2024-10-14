
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.MediatorR.Commands.Projects
{
    [ExcludeFromCodeCoverage]
    public class DeleteProjectCommand(int projectId) : IRequest
    {
        public int ProjectId { get; set; } = projectId;
    }
}