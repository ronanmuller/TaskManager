using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Repositories;
using Xunit;

namespace TaskManager.Tests.InfrastructureTests.Repositories
{
    public class TaskRepositoryTests : IDisposable
    {
        private readonly WriteContext _writeContext;
        private readonly ReadContext _readContext;
        private readonly TaskRepository _taskRepository;

        public TaskRepositoryTests()
        {
            // Configuração do banco de dados em memória
            var writeOptions = new DbContextOptionsBuilder<WriteContext>()
                .UseInMemoryDatabase(databaseName: "TaskRepositoryTests")
                .Options;

            var readOptions = new DbContextOptionsBuilder<ReadContext>()
                .UseInMemoryDatabase(databaseName: "TaskRepositoryTests")
                .Options;

            _writeContext = new WriteContext(writeOptions);
            _readContext = new ReadContext(readOptions);
            _taskRepository = new TaskRepository(_readContext, _writeContext);

            SeedDatabase(); // Método para preencher o banco com dados de teste
        }

        private void SeedDatabase()
        {
            var tasks = new List<Tasks>
            {
                new Tasks { Id = 1, ProjectId = 1, Status = TaskState.Pending },
                new Tasks { Id = 2, ProjectId = 1, Status = TaskState.Completed },
                new Tasks { Id = 3, ProjectId = 1, Status = TaskState.InProgress },
                new Tasks { Id = 4, ProjectId = 2, Status = TaskState.Completed }
            };

            _writeContext.Tasks.AddRange(tasks);
            _writeContext.SaveChanges();
        }

        [Fact]
        public async Task HasPendingTasksAsync_ShouldReturnTrue_WhenPendingTasksExist()
        {
            // Arrange
            int projectId = 1;

            // Act
            var result = await _taskRepository.HasPendingTasksAsync(projectId);

            // Assert
            Assert.True(result); // Deve retornar verdadeiro
        }

        [Fact]
        public async Task HasPendingTasksAsync_ShouldReturnFalse_WhenNoPendingTasksExist()
        {
            // Arrange
            int projectId = 2; // Nenhuma tarefa pendente

            // Act
            var result = await _taskRepository.HasPendingTasksAsync(projectId);

            // Assert
            Assert.False(result); // Deve retornar falso
        }

        [Fact]
        public async Task GetTasksByProjectIdAsync_ShouldReturnTasks_ForValidProjectId()
        {
            // Arrange
            int projectId = 1;

            // Act
            var tasks = await _taskRepository.GetTasksByProjectIdAsync(projectId, 0, 10);

            // Assert
            Assert.Equal(3, tasks.Count()); // Deve retornar 3 tarefas
        }

        [Fact]
        public async Task GetTasksByProjectIdAsync_ShouldReturnEmpty_WhenNoTasksExistForProject()
        {
            // Arrange
            int projectId = 999; // ID de projeto inexistente

            // Act
            var tasks = await _taskRepository.GetTasksByProjectIdAsync(projectId, 0, 10);

            // Assert
            Assert.Empty(tasks); // Deve retornar 0 tarefas
        }

        [Fact]
        public async Task CountTasksByProjectIdAsync_ShouldReturnCount_ForValidProjectId()
        {
            // Arrange
            int projectId = 1;

            // Act
            var count = await _taskRepository.CountTasksByProjectIdAsync(projectId);

            // Assert
            Assert.Equal(3, count); // Deve retornar 3
        }

        [Fact]
        public async Task CountTasksByProjectIdAsync_ShouldReturnZero_WhenNoTasksExistForProject()
        {
            // Arrange
            int projectId = 999; // ID de projeto inexistente

            // Act
            var count = await _taskRepository.CountTasksByProjectIdAsync(projectId);

            // Assert
            Assert.Equal(0, count); // Deve retornar 0
        }

        public void Dispose()
        {
            // Limpa os bancos de dados após cada teste
            _writeContext.Database.EnsureDeleted();
            _readContext.Database.EnsureDeleted();
        }
    }
}
