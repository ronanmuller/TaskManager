using Moq;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Handlers.Tasks;
using TaskManager.Application.MediatorR.Queries.Tasks;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Enums;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.HandlersTests.Tasks;

public class GetTasksByProjectIdQueryHandlerTests
{
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly GetTasksByProjectIdQueryHandler _handler;

    public GetTasksByProjectIdQueryHandlerTests()
    {
        _mockTaskService = new Mock<ITaskService>();
        _handler = new GetTasksByProjectIdQueryHandler(_mockTaskService.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTasks_WhenValidProjectIdIsProvided()
    {
        // Arrange
        var projectId = 1;
        var skip = 0;
        var take = 10;
        var query = new GetTasksByProjectIdQuery(projectId, skip, take);

        var expectedTasks = new List<TaskDto>
        {
            new TaskDto { Id = 1, ProjectId = projectId, Title = "Task 1", Description = "Description 1", DueDate = DateTime.UtcNow.AddDays(5), Status = TaskState.Completed, Priority = TaskPriority.High },
            new TaskDto { Id = 2, ProjectId = projectId, Title = "Task 2", Description = "Description 2", DueDate = DateTime.UtcNow.AddDays(10), Status = TaskState.Pending, Priority = TaskPriority.Medium }
        };

        _mockTaskService.Setup(x => x.GetTasksByProjectIdAsync(projectId, skip, take))
            .ReturnsAsync(expectedTasks);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTasks.Count, result.Count());
        Assert.Equal(expectedTasks[0].Title, result.ElementAt(0).Title);
        Assert.Equal(expectedTasks[1].Title, result.ElementAt(1).Title);

        _mockTaskService.Verify(x => x.GetTasksByProjectIdAsync(projectId, skip, take), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoTasksFound()
    {
        // Arrange
        var projectId = 1;
        var skip = 0;
        var take = 10;
        var query = new GetTasksByProjectIdQuery(projectId, skip, take);

        _mockTaskService.Setup(x => x.GetTasksByProjectIdAsync(projectId, skip, take))
            .ReturnsAsync(new List<TaskDto>()); // Retorna uma lista vazia

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result); // Verifica se a lista retornada está vazia

        _mockTaskService.Verify(x => x.GetTasksByProjectIdAsync(projectId, skip, take), Times.Once);
    }
}
