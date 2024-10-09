using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly ReadContext _context;

        public ProjectRepository(ReadContext context) : base(context) 
        {
            _context = context; 
        }

        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
    }
}