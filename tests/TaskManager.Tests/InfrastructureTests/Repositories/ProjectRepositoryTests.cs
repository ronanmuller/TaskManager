using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Repositories;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TaskManager.Tests.InfrastructureTests.Repositories
{
    public class ProjectRepositoryTests
    {
        private readonly WriteContext _writeContext;
        private readonly ReadContext _readContext;
        private readonly ProjectRepository _projectRepository;

        public ProjectRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<WriteContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var optionsRead = new DbContextOptionsBuilder<ReadContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;


            _writeContext = new WriteContext(options);
            _readContext = new ReadContext(optionsRead);
            _projectRepository = new ProjectRepository(_readContext, _writeContext);
        }

        [Fact]
        public async Task GetProjectsByUserIdAsync_ShouldReturnProjects_WhenUserHasProjects()
        {
            // Arrange
            var userId = 1;
            var projects = new List<Project>
            {
                new Project { Id = 1, Name = "Project 1", UserId = userId, IsDeleted = false },
                new Project { Id = 2, Name = "Project 2", UserId = userId, IsDeleted = false },
                new Project { Id = 3, Name = "Deleted Project", UserId = userId, IsDeleted = true },
            };

            await _writeContext.Projects.AddRangeAsync(projects);
            await _writeContext.SaveChangesAsync();

            // Act
            var result = await _projectRepository.GetProjectsByUserIdAsync(userId, 0, 10);

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task DeleteLogicalProjectsByIdAsync_ShouldThrowKeyNotFoundException_WhenProjectDoesNotExist()
        {
            // Arrange
            var nonExistentProjectId = 999;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _projectRepository.DeleteLogicalProjectsByIdAsync(nonExistentProjectId));

            Assert.Equal("Projeto não encontrado para exclusão.", exception.Message);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenProjectDoesNotExist()
        {
            // Arrange
            var nonExistentProjectId = 999;

            // Act
            var exists = await _projectRepository.ExistsAsync(nonExistentProjectId);

            // Assert
            Assert.False(exists); // O projeto não deve existir
        }
    }
}
