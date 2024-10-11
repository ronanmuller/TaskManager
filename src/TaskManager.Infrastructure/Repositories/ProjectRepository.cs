using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class ProjectRepository(ReadContext readContext, WriteContext writeContext)
        : Repository<Project>(writeContext, readContext), IProjectRepository
    {
        private readonly ReadContext _readContext = readContext;
        private readonly WriteContext _writeContext = writeContext;

        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId, int skip, int take)
        {
            return await _readContext.Projects
                .Where(p => p.UserId == userId && !p.IsDeleted) // Verifique se o projeto não está excluído
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task DeleteLogicalProjectsByIdAsync(int projectId)
        {
            var project = await _writeContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new KeyNotFoundException("Projeto não encontrado para exclusão.");
            }

            project.IsDeleted = true;

            await _writeContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int projectId)
        {
            return await _readContext.Projects.AnyAsync(p => p.Id == projectId && !p.IsDeleted); 
        }
    }
}