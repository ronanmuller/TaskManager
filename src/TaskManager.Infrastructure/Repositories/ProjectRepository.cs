using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly ReadContext _readContext;
        private readonly WriteContext _writeContext;

        public ProjectRepository(ReadContext readContext, WriteContext writeContext)
            : base(writeContext, readContext)
        {
            _readContext = readContext;
            _writeContext = writeContext;
        }

        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId, int skip, int take)
        {
            return await _readContext.Projects
                .Where(p => p.UserId == userId && p.IsDeleted == false)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task DeleteLogicalProjectsByIdAsync(int projectId)
        {
            var project = await _readContext.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new KeyNotFoundException("Projeto não encontrado para exclusão.");
            }

            // Anexa o projeto ao _writeContext para que ele possa ser modificado
            _writeContext.Projects.Attach(project);

            // Marca o projeto como excluído
            project.IsDeleted = true;

            await _writeContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int projectId)
        {
            return await _readContext.Projects.AnyAsync(p => p.Id == projectId);
        }
    }
}