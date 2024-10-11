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
        var tasks = await reportService.GetPerformanceReportAsync(request.AverageDays);

        // Cria a visão de relatorio para o retorno
        return mapper.Map<IEnumerable<TaskReportDto>>(tasks);
    }
}