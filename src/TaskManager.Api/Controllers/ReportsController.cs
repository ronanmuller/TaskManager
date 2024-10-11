using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.MediatorR.Queries.Tasks;

namespace TaskManager.Api.Controllers;

[Authorize(Policy = "RequireManagerRole")] //Conforme o requisito so os gerentes acessam relatorios
[ApiController]
[Route("api/reports")]
public class ReportsController(IMediator mediator, ILogger<ReportsController> logger) : ControllerBase
{
    [HttpGet("performance")]
    public async Task<IActionResult> GetPerformanceReport([FromQuery]int avarageDays)
    {
        logger.LogInformation("Solicitação de relatório de desempenho recebida.");
        var report = await mediator.Send(new GetPerformanceReportQuery(avarageDays));

        return Ok(report); 
    }
}

//Estou assumindo que 30 dias é um range de data é pode ser flexivel. Por isso os parametros
//Estou assumindo que a role manager existe. Creie a policy RequireManagerRole e vou criar uma simulacao so para uso
