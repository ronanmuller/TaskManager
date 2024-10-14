using MediatR;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Queries.Projects
{
    [ExcludeFromCodeCoverage]
    public class GetProjectsByUserIdQuery(int userId, int skip, int take) : IRequest<IEnumerable<ProjectDto>>
    {
        public int UserId { get; private set; } = userId;
        public int Skip { get; private set; } = skip;
        public int Take { get; private set; } = take;
    }
}