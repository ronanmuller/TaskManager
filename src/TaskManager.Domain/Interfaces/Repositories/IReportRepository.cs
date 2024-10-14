using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<IQueryable<Tasks>> GetTasksReportAsync(DateTime dateFrom, DateTime dateTo, int? userId);
    }
}