using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class ReportRepository(ReadContext readContext, WriteContext writeContext)
        : Repository<Tasks>(writeContext, readContext), IReportRepository
    {
        private readonly ReadContext _readContext = readContext ?? throw new ArgumentNullException(nameof(readContext));

        public async Task<IQueryable<Tasks>> GetTasksReportAsync(DateTime dateFrom, DateTime dateTo, int? userId)
        {
            var query = _readContext.Tasks
                .Include(t => t.Project)
                .Where(t => t.Status == TaskState.Completed && t.DueDate >= dateFrom && t.DueDate <= dateTo);

            // Filtro opcional por usuário
            if (userId.HasValue)
            {
                query = query.Where(t => t.Project.UserId == userId.Value);
            }

            return await Task.FromResult(query); // Retorna um IQueryable de forma assíncrona
        }
    }
}