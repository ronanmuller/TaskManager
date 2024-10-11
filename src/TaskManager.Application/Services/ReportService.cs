using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using System.Globalization;
using TaskManager.Application.Dto;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public ReportService(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<TaskReportDto>> GetPerformanceReportAsync(int averageDays)
        {
            if (averageDays <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(averageDays), "O número de dias deve ser maior que zero.");
            }

            // Obtém as tarefas concluídas no intervalo de dias especificado
            var tasks = await _reportRepository.GetTasksReportAsync(averageDays);
            return GenerateTaskReport(tasks);
        }

        public IEnumerable<TaskReportDto> GenerateTaskReport(IEnumerable<Tasks> tasks)
        {
            var taskGroups = tasks
                .GroupBy(t => t.Project.UserId)
                .Select(g => new TaskReportDto
                {
                    UserId = g.Key,
                    CompletionDate = DateTime.UtcNow, // Data atual, se necessário; ajuste conforme a lógica do relatório
                    AverageTasksPerUser = g.Count() // Número total de tarefas por usuário
                })
                .ToList();

            return taskGroups;
        }

        [ExcludeFromCodeCoverage]
        private static bool TryParseDate(string dateString, out DateTime date)
        {
            string[] formats = { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "yyyy/MM/dd" };
            return DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }
    }
}
