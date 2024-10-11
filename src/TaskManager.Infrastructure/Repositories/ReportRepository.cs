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
        private readonly ReadContext _readContext = readContext;

        public async Task<IEnumerable<Tasks>> GetTasksReportAsync(int averageDays)
        {
            var currentDate = DateTime.UtcNow;
            var lastAverageDays = currentDate.AddDays(-averageDays);

            var res = await _readContext.Tasks
                .Include(t => t.Project)
                .Where(t => t.Status == TaskState.Completed && t.DueDate >= lastAverageDays && t.DueDate <= currentDate)
                .ToListAsync();

            return res;
        }
    }
}