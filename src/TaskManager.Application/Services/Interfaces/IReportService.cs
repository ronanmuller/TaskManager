using TaskManager.Application.Dto;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<TaskReportDto>> GetPerformanceReportAsync(string dateFrom, string dateTo, int? userId,
            int skip, int take);

        Task<IEnumerable<TaskReportDto>> GenerateTaskReportAsync(IQueryable<Tasks> tasks, string dateFrom,
            string dateTo, int skip, int take);
    }
}