using TaskManager.Application.Dto;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<TaskReportDto>> GetPerformanceReportAsync(int averageDays);
        IEnumerable<TaskReportDto> GenerateTaskReport(IEnumerable<Tasks> tasks);
    }
}
