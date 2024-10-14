using AutoMapper;
using MediatR;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Queries.Tasks;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.MediatorR.Handlers.Tasks;

public class GetPerformanceReportHandler(IReportService reportService, IMapper mapper)
    : IRequestHandler<GetPerformanceReportQuery, IEnumerable<TaskReportDto>>
{
    public async Task<IEnumerable<TaskReportDto>> Handle(GetPerformanceReportQuery request, CancellationToken cancellationToken)
    {
        var tasks = await reportService.GetPerformanceReportAsync(
            request.DateFrom,
            request.DateTo,
            request.UserId,
            request.Skip,
            request.Take
        );

        return mapper.Map<IEnumerable<TaskReportDto>>(tasks);
    }
}