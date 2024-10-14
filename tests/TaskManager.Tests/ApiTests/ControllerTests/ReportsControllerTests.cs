using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.Dto;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using Xunit;

namespace TaskManager.Tests.Services
{
    public class ReportServiceTests
    {
        private readonly Mock<IReportRepository> _reportRepositoryMock;
        private readonly ReportService _reportService;

        public ReportServiceTests()
        {
            _reportRepositoryMock = new Mock<IReportRepository>();
            var mapperMock = new Mock<IMapper>(); 
            _reportService = new ReportService(_reportRepositoryMock.Object, mapperMock.Object);
        }

      
        [Fact]
        public async Task GetPerformanceReportAsync_StartDateGreaterThanEndDate_ThrowsArgumentException()
        {
            // Arrange
            var dateFrom = "2025-01-02";
            var dateTo = "2025-01-01";  

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _reportService.GetPerformanceReportAsync(dateFrom, dateTo, null, 0, 10));
            Assert.Equal("A data de início não pode ser maior que a data de término.", exception.Message);
        }

        [Fact]
        public async Task GetPerformanceReportAsync_ValidDates_ReturnsTaskReportDto()
        {
            // Arrange
            var dateFrom = "2024-01-01";
            var dateTo = "2024-01-31";
            var userId = 1;
            var skip = 0;
            var take = 10;

            var tasks = new List<Tasks>
            {
                new Tasks { Project = new Project { UserId = userId },  },
                new Tasks { Project = new Project { UserId = userId }, },
            }.AsQueryable();

            _reportRepositoryMock
                .Setup(repo => repo.GetTasksReportAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int?>()))
                .ReturnsAsync(tasks);

            // Act
            var result = await _reportService.GetPerformanceReportAsync(dateFrom, dateTo, userId, skip, take);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<TaskReportDto>>(result);
            Assert.Equal(1, result.Count()); 
        }
    }
}
