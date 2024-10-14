using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TaskManager.Application.Dto;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Application.Services.Interfaces;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.ServicesTests
{
    public class TaskCommentServiceTests
    {
        private readonly Mock<ITaskCommentRepository> _mockCommentRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TaskCommentService _taskCommentService;

        public TaskCommentServiceTests()
        {
            _mockCommentRepository = new Mock<ITaskCommentRepository>();
            _mockUserService = new Mock<IUserService>();
            _mockMapper = new Mock<IMapper>();
            _taskCommentService = new TaskCommentService(_mockCommentRepository.Object, _mockUserService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateCommentAsync_ShouldReturnTaskCommentDto_WhenCommentIsCreatedSuccessfully()
        {
            // Arrange
            int taskId = 1;
            var addCommentDto = new CreateCommentDto { Comment = "This is a comment" };
            var userId = 123;

            _mockUserService.Setup(u => u.GetCurrentUserId()).Returns(userId);
            _mockCommentRepository.Setup(repo => repo.AddCommentAsync(It.IsAny<TaskComment>()))
                .Callback<TaskComment>(comment => comment.Id = 1); // Simula o retorno do ID gerado

            // Act
            var result = await _taskCommentService.CreateCommentAsync(taskId, addCommentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockCommentRepository.Verify(repo => repo.AddCommentAsync(It.IsAny<TaskComment>()), Times.Once);
        }

        [Fact]
        public async Task GetCommentsByTaskIdAsync_ShouldReturnListOfTaskCommentDto_WhenCommentsExist()
        {
            // Arrange
            int taskId = 1;
            var comments = new List<TaskComment>
            {
                new TaskComment { Id = 1, TaskId = taskId, Comment = "Comment 1" },
                new TaskComment { Id = 2, TaskId = taskId, Comment = "Comment 2" }
            };

            _mockCommentRepository.Setup(repo => repo.GetCommentsByTaskIdAsync(taskId))
                .ReturnsAsync(comments);
            _mockMapper.Setup(m => m.Map<IEnumerable<TaskCommentDto>>(It.IsAny<IEnumerable<TaskComment>>()))
                .Returns(new List<TaskCommentDto>
                {
                    new TaskCommentDto { Id = 1 },
                    new TaskCommentDto { Id = 2 }
                });

            // Act
            var result = await _taskCommentService.GetCommentsByTaskIdAsync(taskId);

            // Assert
            var commentList = result.ToList();
            Assert.NotNull(commentList);
            Assert.Equal(2, commentList.Count);
            Assert.Equal(1, commentList[0].Id);
            Assert.Equal(2, commentList[1].Id);
            _mockCommentRepository.Verify(repo => repo.GetCommentsByTaskIdAsync(taskId), Times.Once);
        }

        [Fact]
        public async Task GetCommentsByTaskIdAsync_ShouldReturnEmptyList_WhenNoCommentsExist()
        {
            // Arrange
            int taskId = 1;

            _mockCommentRepository.Setup(repo => repo.GetCommentsByTaskIdAsync(taskId))
                .ReturnsAsync(new List<TaskComment>());

            // Act
            var result = await _taskCommentService.GetCommentsByTaskIdAsync(taskId);

            // Assert
            var commentList = result.ToList();
            Assert.NotNull(commentList);
            Assert.Empty(commentList);
            _mockCommentRepository.Verify(repo => repo.GetCommentsByTaskIdAsync(taskId), Times.Once);
        }
    }
}
