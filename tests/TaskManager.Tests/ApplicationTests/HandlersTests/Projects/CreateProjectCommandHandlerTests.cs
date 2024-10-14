using Moq;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.Projects;
using TaskManager.Application.MediatorR.Handlers.Projects;
using TaskManager.Application.Services.Interfaces;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.HandlersTests.Projects
{
    public class CreateProjectCommandHandlerTests
    {
        private readonly Mock<IProjectService> _projectServiceMock;
        private readonly CreateProjectCommandHandler _handler;

        public CreateProjectCommandHandlerTests()
        {
            _projectServiceMock = new Mock<IProjectService>();
            _handler = new CreateProjectCommandHandler(_projectServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallCreateProjectAsync_WhenCommandIsHandled()
        {
            // Arrange
            var createProjectDto = new CreateProjectDto
            {
                // Inicialize as propriedades necessárias
                Name = "Test Project",
                // Adicione outras propriedades conforme necessário
            };
            var expectedResult = new ProjectDto
            {
                // Inicialize as propriedades esperadas conforme necessário
                Id = 1,
                Name = createProjectDto.Name,
                // Adicione outras propriedades conforme necessário
            };
            var command = new CreateProjectCommand(createProjectDto);

            _projectServiceMock
                .Setup(service => service.CreateProjectAsync(createProjectDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Name, result.Name);
            // Verifique outras propriedades conforme necessário
            _projectServiceMock.Verify(service => service.CreateProjectAsync(createProjectDto), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenServiceReturnsNull()
        {
            // Arrange
            var createProjectDto = new CreateProjectDto
            {
                // Inicialize as propriedades necessárias
                Name = "Test Project",
                // Adicione outras propriedades conforme necessário
            };
            var command = new CreateProjectCommand(createProjectDto);

            _projectServiceMock
                .Setup(service => service.CreateProjectAsync(createProjectDto))
                .ReturnsAsync((ProjectDto)null); // Simulando que o projeto não foi criado

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _projectServiceMock.Verify(service => service.CreateProjectAsync(createProjectDto), Times.Once);
        }
    }
}
