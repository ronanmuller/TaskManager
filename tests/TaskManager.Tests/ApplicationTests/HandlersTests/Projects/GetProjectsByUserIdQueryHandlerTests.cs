using AutoMapper;
using Moq;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Handlers.Projects;
using TaskManager.Application.MediatorR.Queries.Projects;
using TaskManager.Application.Services.Interfaces;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.HandlersTests.Projects
{
    public class GetProjectsByUserIdQueryHandlerTests
    {
        private readonly Mock<IProjectService> _projectServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetProjectsByUserIdQueryHandler _handler;

        public GetProjectsByUserIdQueryHandlerTests()
        {
            _projectServiceMock = new Mock<IProjectService>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetProjectsByUserIdQueryHandler(_projectServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallGetProjectsByUserIdAsync_WhenQueryIsHandled()
        {
            // Arrange
            var userId = 1;
            var skip = 0;
            var take = 10;
            var query = new GetProjectsByUserIdQuery(userId, skip, take);
            var projectsFromService = new List<ProjectDto>
            {
                new ProjectDto { },
                new ProjectDto {  }
            };

            _projectServiceMock
                .Setup(service => service.GetProjectsByUserIdAsync(userId, skip, take))
                .ReturnsAsync(projectsFromService);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _projectServiceMock.Verify(service => service.GetProjectsByUserIdAsync(userId, skip, take), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldMapProjects_WhenQueryIsHandled()
        {
            // Arrange
            var userId = 1;
            var skip = 0;
            var take = 10;
            var query = new GetProjectsByUserIdQuery(userId, skip, take);
            var projectsFromService = new List<ProjectDto>
            {
                new ProjectDto { },
                new ProjectDto { }
            };

            var mappedProjects = new List<ProjectDto>
            {
                new ProjectDto { },
                new ProjectDto { }
            };

            _projectServiceMock
                .Setup(service => service.GetProjectsByUserIdAsync(userId, skip, take))
                .ReturnsAsync(projectsFromService);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<ProjectDto>>(projectsFromService))
                .Returns(mappedProjects);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mappedProjects, result);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<ProjectDto>>(projectsFromService), Times.Once);
        }
    }
}
