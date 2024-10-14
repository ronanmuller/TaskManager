using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using TaskManager.Api.Controllers;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.Tasks;
using TaskManager.Application.MediatorR.Queries.Tasks;
using TaskManager.Domain.Enums;

namespace TaskManager.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<TasksController>> _loggerMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<TasksController>>();
            _controller = new TasksController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnOkResult_WhenTaskIsCreated()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto
            {
                ProjectId = 1,
                Title = "New Task",
                Description = "Task description",
                DueDate = DateTime.Now.AddDays(7),
                Status = TaskState.Pending,
                Priority = TaskPriority.Medium
            };

            var createdTask = new TaskDto
            {
                Id = 1,
                ProjectId = 1,
                Title = "New Task",
                Description = "Task description",
                DueDate = createTaskDto.DueDate,
                Status = TaskState.Pending,
                Priority = TaskPriority.Medium
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<CreateTaskCommand>(cmd => cmd.TaskDto == createTaskDto), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdTask);

            // Act
            var result = await _controller.Create(createTaskDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var task = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(createdTask.Id, task.Id);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.Create(new CreateTaskDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetTasksByProjectId_ShouldReturnOkResult_WithTasks()
        {
            // Arrange
            int projectId = 1;
            var tasks = new List<TaskDto>
            {
                new TaskDto { Id = 1, ProjectId = projectId, Title = "Task 1", Status = TaskState.Pending },
                new TaskDto { Id = 2, ProjectId = projectId, Title = "Task 2", Status = TaskState.Completed }
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetTasksByProjectIdQuery>(q => q.ProjectId == projectId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetTasksByProjectId(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<TaskDto>>(okResult.Value);
            Assert.Equal(2, returnedTasks.Count());
        }

        [Fact]
        public async Task Update_ShouldReturnOkResult_WhenTaskIsUpdated()
        {
            // Arrange
            int taskId = 1;
            var updateTaskDto = new UpdateTaskDto
            {
                Title = "Updated Task",
                Description = "Updated description",
                DueDate = DateTime.Now.AddDays(3),
                Status = TaskState.InProgress,
            };

            var updatedTask = new TaskDto
            {
                Id = taskId,
                ProjectId = 1,
                Title = "Updated Task",
                Description = "Updated description",
                DueDate = DateTime.Now.AddDays(3),
                Status = TaskState.InProgress,
                Priority = TaskPriority.High
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<UpdateTaskCommand>(cmd => cmd.TaskId == taskId && cmd.TaskDto == updateTaskDto), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedTask);

            // Act
            var result = await _controller.Update(taskId, updateTaskDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var task = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(updatedTask.Id, task.Id);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.Update(1, new UpdateTaskDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
