using Moq;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Handlers.TaskComments;
using TaskManager.Application.MediatorR.Queries.TaskComments;
using TaskManager.Application.Services.Interfaces;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.HandlersTests.TaskComments
{
    public class GetCommentsByTaskIdQueryHandlerTests
    {
        private readonly Mock<ITaskCommentService> _taskCommentServiceMock;
        private readonly GetCommentsByTaskIdQueryHandler _handler;

        public GetCommentsByTaskIdQueryHandlerTests()
        {
            _taskCommentServiceMock = new Mock<ITaskCommentService>();
            _handler = new GetCommentsByTaskIdQueryHandler(_taskCommentServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnComments_WhenTaskIdIsValid()
        {
            // Arrange
            var taskId = 1;
            var expectedComments = new List<TaskCommentDto>
            {
                new TaskCommentDto { Content = "Comment 1", TaskId = taskId },
                new TaskCommentDto { Content = "Comment 2", TaskId = taskId }
            };

            var query = new GetCommentsByTaskIdQuery(taskId);
            _taskCommentServiceMock
                .Setup(service => service.GetCommentsByTaskIdAsync(taskId))
                .ReturnsAsync(expectedComments);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedComments.Count, result.Count());
            Assert.Equal(expectedComments, result);
            _taskCommentServiceMock.Verify(service => service.GetCommentsByTaskIdAsync(taskId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCommentsExist()
        {
            // Arrange
            var taskId = 2;
            var query = new GetCommentsByTaskIdQuery(taskId);

            _taskCommentServiceMock
                .Setup(service => service.GetCommentsByTaskIdAsync(taskId))
                .ReturnsAsync(new List<TaskCommentDto>()); // Simulando que não há comentários

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _taskCommentServiceMock.Verify(service => service.GetCommentsByTaskIdAsync(taskId), Times.Once);
        }
    }
}
