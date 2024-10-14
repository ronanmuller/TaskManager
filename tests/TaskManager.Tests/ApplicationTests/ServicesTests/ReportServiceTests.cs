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
using Xunit;

namespace TaskManager.Tests.ApplicationTests.ServicesTests
{
    public class ReportServiceTests
    {
        private readonly Mock<IReportRepository> _mockReportRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ReportService _reportService;

        public ReportServiceTests()
        {
            _mockReportRepository = new Mock<IReportRepository>();
            _mockMapper = new Mock<IMapper>();
            _reportService = new ReportService(_mockReportRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetPerformanceReportAsync_ShouldThrowArgumentException_WhenDatesAreInvalid()
        {
            // Arrange
            string dateFrom = "invalid-date";
            string dateTo = "another-invalid-date";
            int? userId = null;
            int skip = 0;
            int take = 10;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _reportService.GetPerformanceReportAsync(dateFrom, dateTo, userId, skip, take));
            Assert.Equal("Datas inválidas fornecidas. Use formatos como dd/MM/yyyy ou yyyy-MM-dd.", exception.Message);
        }

        [Fact]
        public async Task GetPerformanceReportAsync_ShouldThrowArgumentException_WhenFromDateIsGreaterThanToDate()
        {
            // Arrange
            string dateFrom = "2023-12-31";
            string dateTo = "2023-01-01";
            int? userId = null;
            int skip = 0;
            int take = 10;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _reportService.GetPerformanceReportAsync(dateFrom, dateTo, userId, skip, take));
            Assert.Equal("A data de início não pode ser maior que a data de término.", exception.Message);
        }

        [Fact]
        public async Task GetPerformanceReportAsync_ShouldReturnTaskReportDto_WhenValidInputsAreProvided()
        {
            // Arrange
            var dateFrom = "2023-01-01";
            var dateTo = "2023-12-31";
            int? userId = null;
            int skip = 0;
            int take = 10;

            var tasks = new List<Tasks>
            {
                new Tasks { Project = new Project { UserId = 1 } },
                new Tasks { Project = new Project { UserId = 1 } },
                new Tasks { Project = new Project { UserId = 2 } }
            }.AsQueryable();

            _mockReportRepository.Setup(repo => repo.GetTasksReportAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), userId))
                .ReturnsAsync(tasks);

            // Act
            var result = await _reportService.GetPerformanceReportAsync(dateFrom, dateTo, userId, skip, take);

            // Assert
            var report = result.ToList();
            Assert.NotNull(report);
            Assert.Equal(2, report.Count); // 2 users
            Assert.Equal(2, report.First().CountTasksPerUser); // User 1 has 2 tasks
            Assert.Equal(1, report.Last().CountTasksPerUser); // User 2 has 1 task
        }

        [Fact]
        public async Task GenerateTaskReportAsync_ShouldReturnTaskReportDtos_WhenValidInputsAreProvided()
        {
            // Arrange
            var tasks = new List<Tasks>
            {
                new Tasks { Project = new Project { UserId = 1 } },
                new Tasks { Project = new Project { UserId = 1 } },
                new Tasks { Project = new Project { UserId = 2 } }
            }.AsQueryable();

            var dateFrom = "2023-01-01";
            var dateTo = "2023-12-31";
            int skip = 0;
            int take = 10;

            // Act
            var result = await _reportService.GenerateTaskReportAsync(tasks, dateFrom, dateTo, skip, take);

            // Assert
            var report = result.ToList();
            Assert.NotNull(report);
            Assert.Equal(2, report.Count); // 2 users
            Assert.Equal(2, report.First().CountTasksPerUser); // User 1 has 2 tasks
            Assert.Equal(1, report.Last().CountTasksPerUser); // User 2 has 1 task
        }
    }
}
