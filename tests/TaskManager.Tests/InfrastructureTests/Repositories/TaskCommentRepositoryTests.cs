using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Repositories;
using Xunit;

namespace TaskManager.Tests.InfrastructureTests.Repositories
{
    public class TaskCommentRepositoryTests : IDisposable
    {
        private readonly WriteContext _writeContext;
        private readonly ReadContext _readContext;
        private readonly TaskCommentRepository _taskCommentRepository;

        public TaskCommentRepositoryTests()
        {
            // Configuração do banco de dados em memória
            var writeOptions = new DbContextOptionsBuilder<WriteContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var readOptions = new DbContextOptionsBuilder<ReadContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _writeContext = new WriteContext(writeOptions);
            _readContext = new ReadContext(readOptions);
            _taskCommentRepository = new TaskCommentRepository(_readContext, _writeContext);

            SeedDatabase(); // Método para preencher o banco com dados de teste
        }

        private void SeedDatabase()
        {
            var taskComment = new TaskComment
            {
                Id = 1,
                TaskId = 1,
                Comment = "First comment"
            };

            _writeContext.TaskComments.Add(taskComment);
            _writeContext.SaveChanges();
        }

        [Fact]
        public async Task AddCommentAsync_ShouldAddCommentToDatabase()
        {
            // Arrange
            var newComment = new TaskComment
            {
                Id = 2,
                TaskId = 1,
                Comment = "New comment"
            };

            // Act
            await _taskCommentRepository.AddCommentAsync(newComment);

            // Assert
            var comments = await _taskCommentRepository.GetCommentsByTaskIdAsync(1);
            Assert.Equal(2, comments.Count()); // Deve haver 2 comentários
            Assert.Contains(comments, c => c.Comment == "New comment");
        }

        [Fact]
        public async Task GetCommentsByTaskIdAsync_ShouldReturnComments_ForValidTaskId()
        {
            // Arrange
            var taskId = 1;

            // Act
            var comments = await _taskCommentRepository.GetCommentsByTaskIdAsync(taskId);

            // Assert
            Assert.Single(comments); // Deve retornar 1 comentário
            Assert.Equal("First comment", comments.First().Comment);
        }

        [Fact]
        public async Task GetCommentsByTaskIdAsync_ShouldReturnEmpty_WhenNoCommentsFound()
        {
            // Arrange
            var taskId = 999; // ID de tarefa inexistente

            // Act
            var comments = await _taskCommentRepository.GetCommentsByTaskIdAsync(taskId);

            // Assert
            Assert.Empty(comments); // Deve retornar 0 comentários
        }

        public void Dispose()
        {
            // Limpa os bancos de dados após cada teste
            _writeContext.Database.EnsureDeleted();
            _readContext.Database.EnsureDeleted();
        }
    }
}
