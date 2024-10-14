using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using TaskManager.API.Controllers;
using TaskManager.Application.MediatorR.Commands.Projects;
using TaskManager.Application.MediatorR.Queries.Projects;
using TaskManager.Application.Dto;

namespace TaskManager.Tests.Controllers
{
    public class ProjectsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<ProjectsController>> _loggerMock;
        private readonly ProjectsController _controller;

        public ProjectsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<ProjectsController>>();
            _controller = new ProjectsController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnOkResult_WhenProjectIsCreated()
        {
            // Arrange
            var createProjectDto = new CreateProjectDto { Name = "New Project", UserId = 1 };
            var createdProject = new ProjectDto { Id = 1, Name = "New Project", UserId = 1 };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateProjectCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdProject);

            // Act
            var result = await _controller.Create(createProjectDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var project = Assert.IsType<ProjectDto>(okResult.Value);
            Assert.Equal(createdProject.Id, project.Id);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(new CreateProjectDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenProjectIsDeleted()
        {
            // Arrange
            var projectId = 1;
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteProjectCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(projectId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetByUserId_ShouldReturnOkResult_WithProjects()
        {
            // Arrange
            var userId = 1;
            var projects = new List<ProjectDto>
            {
                new ProjectDto { Id = 1, Name = "Project 1", UserId = userId },
                new ProjectDto { Id = 2, Name = "Project 2", UserId = userId }
            };
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetProjectsByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projects);

            // Act
            var result = await _controller.GetByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProjects = Assert.IsType<List<ProjectDto>>(okResult.Value);
            Assert.Equal(2, returnedProjects.Count);
        }

        [Fact]
        public async Task GetByUserId_ShouldReturnBadRequest_WhenPaginationParametersAreInvalid()
        {
            // Act
            var result = await _controller.GetByUserId(1, skip: -1, take: 0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Os parâmetros de paginação são inválidos.", badRequestResult.Value);
        }
    }
}
