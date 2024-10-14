using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.Tasks;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.HandlersTests.Tasks;

public class CreateTaskCommandTests
{
    [Fact]
    public void CreateTaskCommand_ShouldInitializeTaskDto()
    {
        // Arrange
        var taskDto = new CreateTaskDto
        {
            Title = "Test Task",
            Description = "This is a test task.",
            DueDate = DateTime.UtcNow.AddDays(7),
            // Adicione outros campos necessários
        };

        // Act
        var command = new CreateTaskCommand(taskDto);

        // Assert
        Assert.NotNull(command.TaskDto);
        Assert.Equal(taskDto.Title, command.TaskDto.Title);
        Assert.Equal(taskDto.Description, command.TaskDto.Description);
        Assert.Equal(taskDto.DueDate, command.TaskDto.DueDate);
    }
}