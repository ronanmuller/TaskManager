using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.Repositories;

public interface IReportRepository
{
    Task<IEnumerable<Tasks>> GetTasksReportAsync(int averageDays);
}