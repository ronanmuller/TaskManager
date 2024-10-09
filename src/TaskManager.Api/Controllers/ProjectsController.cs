using Microsoft.AspNetCore.Mvc;
using MediatR;
using TaskManager.Application.MediatorR.Commands.Projects;
using TaskManager.Application.MediatorR.Queries.Projects;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController(IMediator mediator, ILogger<ProjectsController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectCommand command)
        {
            logger.LogInformation("Recebida solicitação para criar um novo projeto.");

            var result = await mediator.Send(command);

            logger.LogInformation("Projeto criado com sucesso.");

            return Ok(result); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            logger.LogInformation("Recebida solicitação para excluir o projeto de ID {id}", id);

            var command = new DeleteProjectCommand(id);
            await mediator.Send(command);

            logger.LogInformation("Projeto de ID {id} excluído com sucesso.", id);

            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetProjectsByUserId(int userId)
        {
            logger.LogInformation("Recebida solicitação para listar projetos do usuário de ID {UserId}", userId);

            var query = new GetProjectsByUserIdQuery(userId);

            var projects = await mediator.Send(query);

            return Ok(projects);
        }
    }
}