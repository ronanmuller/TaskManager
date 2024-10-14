using AutoMapper;
using Moq;
using TaskManager.Application.Dto;
using TaskManager.Application.MediatorR.Handlers.Tasks;
using TaskManager.Application.MediatorR.Queries.Tasks;
using TaskManager.Application.Services.Interfaces;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.HandlersTests.Tasks;

public class GetPerformanceReportHandlerTests
{
    private readonly Mock<IReportService> _mockReportService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetPerformanceReportHandler _handler;

    public GetPerformanceReportHandlerTests()
    {
        _mockReportService = new Mock<IReportService>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetPerformanceReportHandler(_mockReportService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedTaskReportDtos()
    {
        // Arrange
        var query = new GetPerformanceReportQuery("2024-01-01", "2024-01-31", null, 0, 10);

        var tasksFromService = new List<TaskReportDto>
        {
            new TaskReportDto { Status = "Completed", UserId = 1, CountTasksPerUser = 5, InitDate = "2024-01-01", EndDate = "2024-01-31" },
            new TaskReportDto { Status = "Pending", UserId = 2, CountTasksPerUser = 3, InitDate = "2024-01-01", EndDate = "2024-01-31" }
        };

        // Configure o mock para retornar a lista de tarefas
        _mockReportService.Setup(x => x.GetPerformanceReportAsync(query.DateFrom, query.DateTo, query.UserId, query.Skip, query.Take))
            .ReturnsAsync(tasksFromService);

        // Configure o mock do AutoMapper para mapear a lista de tarefas
        _mockMapper.Setup(m => m.Map<IEnumerable<TaskReportDto>>(It.IsAny<IEnumerable<TaskReportDto>>()))
            .Returns(tasksFromService);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tasksFromService.Count, result.Count());

        // Verifique se os itens retornados estão corretos
        for (int i = 0; i < tasksFromService.Count; i++)
        {
            Assert.Equal(tasksFromService[i].Status, result.ElementAt(i).Status);
            Assert.Equal(tasksFromService[i].UserId, result.ElementAt(i).UserId);
            Assert.Equal(tasksFromService[i].CountTasksPerUser, result.ElementAt(i).CountTasksPerUser);
            Assert.Equal(tasksFromService[i].InitDate, result.ElementAt(i).InitDate);
            Assert.Equal(tasksFromService[i].EndDate, result.ElementAt(i).EndDate);
        }

        // Verifique se o método do serviço foi chamado corretamente
        _mockReportService.Verify(x => x.GetPerformanceReportAsync(query.DateFrom, query.DateTo, query.UserId, query.Skip, query.Take), Times.Once);
    }
}
