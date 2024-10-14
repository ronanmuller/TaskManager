using Moq;
using TaskManager.Application.MediatorR.Commands.Projects;
using TaskManager.Application.MediatorR.Handlers.Projects;
using TaskManager.Application.Services.Interfaces;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.HandlersTests.Projects
{
    public class DeleteProjectCommandHandlerTests
    {
        private readonly Mock<IProjectService> _projectServiceMock;
        private readonly DeleteProjectCommandHandler _handler;

        public DeleteProjectCommandHandlerTests()
        {
            _projectServiceMock = new Mock<IProjectService>();
            _handler = new DeleteProjectCommandHandler(_projectServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallDeleteProjectAsync_WhenCommandIsHandled()
        {
            // Arrange
            var projectId = 1; // ID do projeto a ser excluído
            var command = new DeleteProjectCommand(projectId);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _projectServiceMock.Verify(service => service.DeleteProjectAsync(projectId), Times.Once);
        }
    }
}