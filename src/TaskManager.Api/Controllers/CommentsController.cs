using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.TaskComments;
using TaskManager.Application.MediatorR.Queries.TaskComments;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateCommentCommand
            {
                Comment = createCommentDto.Comment,
                TaskId = createCommentDto.TaskId
            };

            var commentId = await mediator.Send(command);
            return Ok(commentId);
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<IEnumerable<TaskCommentDto>>> GetCommentsByTaskId(int taskId)
        {
            var comments = await mediator.Send(new GetCommentsByTaskIdQuery(taskId));
            return Ok(comments);
        }
    }
}