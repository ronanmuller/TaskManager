using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.Tasks;
using TaskManager.Application.MediatorR.Queries.Tasks;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController(IMediator mediator, ILogger<TasksController> logger) : ControllerBase
{
    // POST: api/tasks
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto taskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        logger.LogInformation("Recebida solicitação para criar uma nova tarefa.");

        var command = new CreateTaskCommand(taskDto);
        var result = await mediator.Send(command);

        logger.LogInformation("Tarefa criada com sucesso com o ID {TaskId}.", result.Id);

        return Ok(result);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetTasksByProjectId(int projectId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        logger.LogInformation("Recebida solicitação para listar tarefas do projeto de ID {ProjectId}.", projectId);

        var query = new GetTasksByProjectIdQuery(projectId, skip, take); 
        var tasks = await mediator.Send(query); 

        return Ok(tasks); 
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto updateTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        logger.LogInformation("Recebida solicitação para atualizar a tarefa de ID {TaskId}.", id);

        var command = new UpdateTaskCommand(id, updateTaskDto); 
        var result = await mediator.Send(command); 

        // Retorna o DTO atualizado
        logger.LogInformation("Tarefa de ID {TaskId} atualizada com sucesso.", result.Id);
        return Ok(result);
    }
}