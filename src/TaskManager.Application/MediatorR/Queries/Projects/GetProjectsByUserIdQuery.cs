using MediatR;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Queries.Projects
{
    public class GetProjectsByUserIdQuery(int userId) : IRequest<IEnumerable<ProjectDto>>
    {
        public int UserId { get; private set; } = userId;
    }
}