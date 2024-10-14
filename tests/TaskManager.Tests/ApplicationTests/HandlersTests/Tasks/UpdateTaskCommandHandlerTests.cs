using Moq;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.Tasks;
using TaskManager.Application.MediatorR.Handlers.Tasks;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Enums;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.HandlersTests.Tasks
{
    public class UpdateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly UpdateTaskCommandHandler _handler;

        public UpdateTaskCommandHandlerTests()
        {
            _mockTaskService = new Mock<ITaskService>();
            _handler = new UpdateTaskCommandHandler(_mockTaskService.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUpdatedTask_WhenValidUpdateRequestIsProvided()
        {
            // Arrange
            var taskId = 1;
            var updateDto = new UpdateTaskDto
            {
                Title = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.UtcNow.AddDays(5), // Pode ser null, mas aqui estamos definindo um valor
                Status = TaskState.Pending, // Também pode ser null
            };

            var expectedResult = new TaskDto
            {
                Id = taskId,
                Title = updateDto.Title,
                Description = updateDto.Description,
                DueDate = updateDto.DueDate.Value, // Agora é compatível
                Status = updateDto.Status ?? TaskState.Pending, // Use um valor padrão ou trate a nulo
                Priority = TaskPriority.Medium // Inclua prioridade se necessário
            };

            var command = new UpdateTaskCommand(taskId, updateDto);

            _mockTaskService.Setup(x => x.UpdateTaskAsync(taskId, updateDto))
                .ReturnsAsync(expectedResult); // Simula o retorno do DTO atualizado

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Title, result.Title);
            Assert.Equal(expectedResult.Description, result.Description);
            Assert.Equal(expectedResult.Status, result.Status);
            Assert.Equal(expectedResult.Priority, result.Priority); // Verifica prioridade se necessário

            _mockTaskService.Verify(x => x.UpdateTaskAsync(taskId, updateDto), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenTaskUpdateFails()
        {
            // Arrange
            var taskId = 1;
            var updateDto = new UpdateTaskDto
            {
                Title = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.UtcNow.AddDays(5), // Pode ser null
                Status = TaskState.Pending, // Também pode ser null
            };

            var command = new UpdateTaskCommand(taskId, updateDto);

            _mockTaskService.Setup(x => x.UpdateTaskAsync(taskId, updateDto))
                .ThrowsAsync(new Exception("Update failed")); // Simula uma falha na atualização

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Update failed", exception.Message);

            _mockTaskService.Verify(x => x.UpdateTaskAsync(taskId, updateDto), Times.Once);
        }
    }
}
