using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManager.Api.Controllers;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Commands.TaskComments;
using TaskManager.Application.MediatorR.Queries.TaskComments;
using Xunit;

namespace TaskManager.Tests.Controllers
{
    public class CommentsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CommentsController _commentsController;

        public CommentsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _commentsController = new CommentsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateComment_ShouldReturnOk_WithCommentId()
        {
            // Arrange
            var createCommentDto = new CreateCommentDto
            {
                Comment = "Test comment",
                TaskId = 1
            };
            var expectedCommentId = 1;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateCommentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCommentId);

            // Act
            var result = await _commentsController.CreateComment(createCommentDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(expectedCommentId);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateCommentCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateComment_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var createCommentDto = new CreateCommentDto
            {
                Comment = "Test comment",
                TaskId = 1
            };
            _commentsController.ModelState.AddModelError("TaskId", "Required");

            // Act
            var result = await _commentsController.CreateComment(createCommentDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetCommentsByTaskId_ShouldReturnOk_WithComments()
        {
            // Arrange
            var taskId = 1;
            var expectedComments = new List<TaskCommentDto>
            {
                new TaskCommentDto { Id = 1, Content = "Comment 1", TaskId = taskId, CreatedDate = DateTime.UtcNow, UserId = 1 },
                new TaskCommentDto { Id = 2, Content = "Comment 2", TaskId = taskId, CreatedDate = DateTime.UtcNow, UserId = 2 }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCommentsByTaskIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedComments);

            // Act
            var result = await _commentsController.GetCommentsByTaskId(taskId);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var actualComments = okResult.Value.Should().BeAssignableTo<IEnumerable<TaskCommentDto>>().Subject;
            actualComments.Should().HaveCount(expectedComments.Count);
            actualComments.First().Id.Should().Be(expectedComments.First().Id);
            actualComments.First().Content.Should().Be(expectedComments.First().Content);
            actualComments.First().TaskId.Should().Be(expectedComments.First().TaskId);
            actualComments.First().UserId.Should().Be(expectedComments.First().UserId);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetCommentsByTaskIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
