using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class ProjectTaskRepository(WriteContext context) : Repository<ProjectTask>(context), IProjectTaskRepository
    {
        public async Task RemoveByProjectIdAsync(int projectId)
        {
            var projectTasks = await context.ProjectTasks.Where(pt => pt.ProjectId == projectId).ToListAsync();

            if (projectTasks.Any())
            {
                context.ProjectTasks.RemoveRange(projectTasks);
                await context.SaveChangesAsync();
            }
        }
    }
}