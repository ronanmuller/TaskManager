using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Repositories;
using Xunit;

namespace TaskManager.Tests.InfrastructureTests.Repositories
{
    public class ReportRepositoryTests : IDisposable
    {
        private readonly WriteContext _writeContext;
        private readonly ReadContext _readContext;
        private readonly ReportRepository _reportRepository;

        public ReportRepositoryTests()
        {
            // Configuração do banco de dados em memória
            var writeOptions = new DbContextOptionsBuilder<WriteContext>()
                .UseInMemoryDatabase(databaseName: "ReportRepositoryTests")
                .Options;

            var readOptions = new DbContextOptionsBuilder<ReadContext>()
                .UseInMemoryDatabase(databaseName: "ReportRepositoryTests")
                .Options;

            _writeContext = new WriteContext(writeOptions);
            _readContext = new ReadContext(readOptions);
            _reportRepository = new ReportRepository(_readContext, _writeContext);

            SeedDatabase(); // Método para preencher o banco com dados de teste
        }

        private void SeedDatabase()
        {
            var projects = new List<Project>
            {
                new Project { Id = 1, Name = "Project 1", UserId = 1 },
                new Project { Id = 2, Name = "Project 2", UserId = 2 }
            };

            var tasks = new List<Tasks>
            {
                new Tasks { Id = 1, Title = "Task 1", Status = TaskState.Completed, DueDate = DateTime.Now.AddDays(-1), Project = projects[0] },
                new Tasks { Id = 2, Title = "Task 2", Status = TaskState.Completed, DueDate = DateTime.Now.AddDays(-5), Project = projects[0] },
                new Tasks { Id = 3, Title = "Task 3", Status = TaskState.Pending, DueDate = DateTime.Now.AddDays(-3), Project = projects[1] },
                new Tasks { Id = 4, Title = "Task 4", Status = TaskState.Completed, DueDate = DateTime.Now.AddDays(-10), Project = projects[1] }
            };

            _writeContext.Projects.AddRange(projects);
            _writeContext.Tasks.AddRange(tasks);
            _writeContext.SaveChanges();
        }

        [Fact]
        public async Task GetTasksReportAsync_ShouldReturnCompletedTasks_WithinDateRange()
        {
            // Arrange
            var dateFrom = DateTime.Now.AddDays(-7);
            var dateTo = DateTime.Now;

            // Act
            var result = await _reportRepository.GetTasksReportAsync(dateFrom, dateTo, null);

            // Assert
            Assert.Equal(2, result.Count()); // Deve retornar 3 tarefas concluída
        }

        [Fact]
        public async Task GetTasksReportAsync_ShouldReturnCompletedTasks_ForSpecificUser()
        {
            // Arrange
            var dateFrom = DateTime.Now.AddDays(-7);
            var dateTo = DateTime.Now;
            int userId = 1; // ID do usuário do Project 1

            // Act
            var result = await _reportRepository.GetTasksReportAsync(dateFrom, dateTo, userId);

            // Assert
            Assert.Equal(2, result.Count()); // Deve retornar 2 tarefas concluídas para o usuário com ID 1
        }

        [Fact]
        public async Task GetTasksReportAsync_ShouldReturnEmpty_WhenNoTasksFound()
        {
            // Arrange
            var dateFrom = DateTime.Now.AddDays(-1);
            var dateTo = DateTime.Now.AddDays(-1); // Data que não tem tarefas

            // Act
            var result = await _reportRepository.GetTasksReportAsync(dateFrom, dateTo, null);

            // Assert
            Assert.Empty(result); // Deve retornar 0 tarefas
        }

        public void Dispose()
        {
            // Limpa os bancos de dados após cada teste
            _writeContext.Database.EnsureDeleted();
            _readContext.Database.EnsureDeleted();
        }
    }
}
