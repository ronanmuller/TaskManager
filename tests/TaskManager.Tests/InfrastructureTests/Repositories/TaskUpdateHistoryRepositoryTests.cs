using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Repositories;
using Xunit;

namespace TaskManager.Tests.InfrastructureTests.Repositories
{
    public class TaskUpdateHistoryRepositoryTests
    {
        private readonly ReadContext _readContext;
        private readonly WriteContext _writeContext;
        private readonly TaskUpdateHistoryRepository _repository;

        public TaskUpdateHistoryRepositoryTests()
        {
            // Configura o contexto para usar um banco de dados em memória
            var optionsRead = new DbContextOptionsBuilder<ReadContext>()
                .UseInMemoryDatabase(databaseName: "TaskUpdateHistoryRepositoryTests")
                .Options;

            var optionsWrite = new DbContextOptionsBuilder<WriteContext>()
                .UseInMemoryDatabase(databaseName: "TaskUpdateHistoryRepositoryTestsWrite")
                .Options;

            // Inicializa os contextos com as opções configuradas
            _readContext = new ReadContext(optionsRead);
            _writeContext = new WriteContext(optionsWrite);

            // Cria uma instância do repositório
            _repository = new TaskUpdateHistoryRepository(_readContext, _writeContext);
        }

        [Fact]
        public async Task AddAsync_ShouldAddTaskUpdateHistory()
        {
            // Arrange
            var taskUpdateHistory = new TaskUpdateHistory
            {
                TaskId = 1,
                UpdateDetail = "Task updated",
                UpdateDate = DateTime.UtcNow
            };

            // Act
            await _repository.AddAsync(taskUpdateHistory);

            // Assert
            var result = await _writeContext.Set<TaskUpdateHistory>().FindAsync(taskUpdateHistory.Id);
            result.Should().NotBeNull();
            result.UpdateDetail.Should().Be("Task updated");
        }
    }
}