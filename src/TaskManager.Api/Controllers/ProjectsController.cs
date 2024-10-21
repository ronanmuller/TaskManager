using Microsoft.AspNetCore.Mvc;
using MediatR;
using TaskManager.Application.MediatorR.Commands.Projects;
using TaskManager.Application.MediatorR.Queries.Projects;
using TaskManager.Application.Dto;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController(IMediator mediator, ILogger<ProjectsController> logger) : ControllerBase
    {
        // POST: api/projects
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto projectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            logger.LogInformation("Recebida solicitação para criar um novo projeto.");

            var command = new CreateProjectCommand(projectDto);
            var result = await mediator.Send(command);

            logger.LogInformation("Projeto criado com sucesso com o ID {ProjectId}.", result.Id);

            return Ok(result);
        }

        // DELETE: api/projects/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            logger.LogInformation("Recebida solicitação para excluir o projeto de ID {id}.", id);

            var command = new DeleteProjectCommand(id);
            await mediator.Send(command);

            logger.LogInformation("Projeto de ID {id} excluído com sucesso.", id);

            return NoContent();
        }

        // GET: api/projects/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            logger.LogInformation("Recebida solicitação para listar projetos do usuário de ID {UserId}.", userId);

            if (skip < 0 || take <= 0)
            {
                logger.LogWarning("Parâmetros de paginação inválidos: skip={skip}, take={take}.", skip, take);
                return BadRequest("Os parâmetros de paginação são inválidos.");
            }

            var query = new GetProjectsByUserIdQuery(userId, skip, take);
            var projects = await mediator.Send(query);

            return Ok(projects);
        }
    }
}
