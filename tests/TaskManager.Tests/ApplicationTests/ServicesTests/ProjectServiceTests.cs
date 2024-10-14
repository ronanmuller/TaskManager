using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.Application.Dto;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.ServicesTests
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<ILogger<ProjectService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockLogger = new Mock<ILogger<ProjectService>>();
            _mockMapper = new Mock<IMapper>();

            _projectService = new ProjectService(
                _mockProjectRepository.Object,
                _mockTaskRepository.Object,
                _mockLogger.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task GetProjectsByUserIdAsync_ShouldReturnMappedProjects()
        {
            // Arrange
            var userId = 1;
            var skip = 0;
            var take = 10;
            var projects = new List<Project>
            {
                new Project { Id = 1, Name = "Project 1" },
                new Project { Id = 2, Name = "Project 2" }
            };
            var projectDtos = new List<ProjectDto>
            {
                new ProjectDto { Id = 1, Name = "Project 1" },
                new ProjectDto { Id = 2, Name = "Project 2" }
            };

            _mockProjectRepository.Setup(repo => repo.GetProjectsByUserIdAsync(userId, skip, take))
                .ReturnsAsync(projects);
            _mockMapper.Setup(m => m.Map<IEnumerable<ProjectDto>>(projects))
                .Returns(projectDtos);

            // Act
            var result = await _projectService.GetProjectsByUserIdAsync(userId, skip, take);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectDtos.Count, result.Count());
            _mockProjectRepository.Verify(repo => repo.GetProjectsByUserIdAsync(userId, skip, take), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ProjectDto>>(projects), Times.Once);
        }

        [Fact]
        public async Task CreateProjectAsync_ShouldReturnMappedProjectDto()
        {
            // Arrange
            var createProjectDto = new CreateProjectDto { Name = "New Project" };
            var project = new Project { Id = 1, Name = "New Project" };
            var projectDto = new ProjectDto { Id = 1, Name = "New Project" };

            _mockMapper.Setup(m => m.Map<Project>(createProjectDto)).Returns(project);
            _mockMapper.Setup(m => m.Map<ProjectDto>(project)).Returns(projectDto);

            // Act
            var result = await _projectService.CreateProjectAsync(createProjectDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectDto.Name, result.Name);
            _mockProjectRepository.Verify(repo => repo.AddAsync(project), Times.Once);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldThrowException_WhenProjectIdIsInvalid()
        {
            // Arrange
            var projectId = 0;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _projectService.DeleteProjectAsync(projectId));
            Assert.Equal("Project id não pode ser menor ou igual a 0.", exception.Message);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldThrowUnauthorizedAccessException_WhenProjectHasPendingTasks()
        {
            // Arrange
            var projectId = 1;
            _mockTaskRepository.Setup(repo => repo.HasPendingTasksAsync(projectId)).ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _projectService.DeleteProjectAsync(projectId));
            Assert.Equal("Não é possível excluir um projeto com tarefas pendentes. Conclua as tarefas pendentes e repita a operação.", exception.Message);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldDeleteProject_WhenValidProjectIdIsProvided()
        {
            // Arrange
            var projectId = 1;
            _mockTaskRepository.Setup(repo => repo.HasPendingTasksAsync(projectId)).ReturnsAsync(false);

            // Act
            await _projectService.DeleteProjectAsync(projectId);

            // Assert
            _mockProjectRepository.Verify(repo => repo.DeleteLogicalProjectsByIdAsync(projectId), Times.Once);
        }
    }
}
