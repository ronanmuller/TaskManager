
using MediatR;

namespace TaskManager.Application.MediatorR.Commands.Projects
{
    public class DeleteProjectCommand(int projectId) : IRequest
    {
        public int ProjectId { get; set; } = projectId;
    }
}