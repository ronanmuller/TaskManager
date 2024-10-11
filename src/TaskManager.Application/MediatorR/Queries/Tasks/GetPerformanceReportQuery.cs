using MediatR;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Queries.Tasks;

public record GetPerformanceReportQuery(int AverageDays) : IRequest<IEnumerable<TaskReportDto>>;