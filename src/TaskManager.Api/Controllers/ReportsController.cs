using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.MediatorR.Queries.Tasks;

namespace TaskManager.Api.Controllers;

[Authorize(Policy = "RequireManagerRole")] // Apenas gerentes podem acessar os relatórios
[ApiController]
[Route("api/reports")]
public class ReportsController(IMediator mediator, ILogger<ReportsController> logger) : ControllerBase
{
    [HttpGet("performance")]
    public async Task<IActionResult> GetPerformanceReport(
        [FromQuery] int? userId,
        [FromQuery] string dateFrom,
        [FromQuery] string dateTo,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10)
    {
        logger.LogInformation("Solicitação de relatório de desempenho recebida.");

        var reportQuery = new GetPerformanceReportQuery(
            dateFrom,
            dateTo,
            userId,
            skip,
            take
        );

        var report = await mediator.Send(reportQuery);

        return Ok(report);
    }
}