using Moq;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.TaskComments;
using TaskManager.Application.MediatorR.Handlers.TaskComments;
using TaskManager.Application.Services.Interfaces;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.HandlersTests.TaskComments
{
    public class CreateCommentCommandHandlerTests
    {
        private readonly Mock<ITaskCommentService> _taskCommentServiceMock;
        private readonly CreateCommentCommandHandler _handler;

        public CreateCommentCommandHandlerTests()
        {
            _taskCommentServiceMock = new Mock<ITaskCommentService>();
            _handler = new CreateCommentCommandHandler(_taskCommentServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCommentId_WhenCommentIsCreated()
        {
            // Arrange
            var taskId = 1;
            var commentText = "This is a comment";
            var command = new CreateCommentCommand
            {
                TaskId = taskId,
                Comment = commentText
            };

            var createdCommentDto = new TaskCommentDto
            {
                Id = 123, // ID simulado do comentário criado
                Content = commentText,
                TaskId = taskId
            };

            _taskCommentServiceMock
                .Setup(service => service.CreateCommentAsync(taskId, It.Is<CreateCommentDto>(dto => dto.Comment == commentText)))
                .ReturnsAsync(createdCommentDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(createdCommentDto.Id, result);
            _taskCommentServiceMock.Verify(service => service.CreateCommentAsync(taskId, It.IsAny<CreateCommentDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenServiceFailsToCreateComment()
        {
            // Arrange
            var taskId = 1;
            var command = new CreateCommentCommand
            {
                TaskId = taskId,
                Comment = "This comment will not be created"
            };

            _taskCommentServiceMock
                .Setup(service => service.CreateCommentAsync(taskId, It.IsAny<CreateCommentDto>()))
                .ThrowsAsync(new System.Exception("Service error"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(async () => await _handler.Handle(command, CancellationToken.None));
            _taskCommentServiceMock.Verify(service => service.CreateCommentAsync(taskId, It.IsAny<CreateCommentDto>()), Times.Once);
        }
    }
}
