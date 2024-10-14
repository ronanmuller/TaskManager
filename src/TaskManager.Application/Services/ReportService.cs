using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using System.Globalization;
using TaskManager.Application.Dto;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Services
{
    public class ReportService(IReportRepository reportRepository, IMapper mapper) : IReportService
    {
        private readonly IReportRepository _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<IEnumerable<TaskReportDto>> GetPerformanceReportAsync(string dateFrom, string dateTo, int? userId, int skip, int take)
        {
            // Valida e tenta converter as datas fornecidas
            if (!TryParseDate(dateFrom, out var fromDate) || !TryParseDate(dateTo, out var toDate))
            {
                throw new ArgumentException("Datas inválidas fornecidas. Use formatos como dd/MM/yyyy ou yyyy-MM-dd.");
            }

            if (fromDate > toDate)
            {
                throw new ArgumentException("A data de início não pode ser maior que a data de término.");
            }

            // Obtém as tarefas com base nos filtros fornecidos
            var tasks = await _reportRepository.GetTasksReportAsync(fromDate, toDate, userId);

            return await GenerateTaskReportAsync(tasks, dateFrom, dateTo, skip, take);
        }

        public async Task<IEnumerable<TaskReportDto>> GenerateTaskReportAsync(IQueryable<Tasks> tasks, string dateFrom, string dateTo, int skip, int take)
        {
            var taskGroups = tasks
                .GroupBy(t => t.Project.UserId) // Agrupa as tarefas por UserId
                .Select(g => new TaskReportDto
                {
                    UserId = g.Key, // O UserId do grupo
                    CountTasksPerUser = g.Count(), // Número total de tarefas concluídas por usuário
                    InitDate = dateFrom,
                    EndDate = dateTo
                });

            return await Task.FromResult(taskGroups.Skip(skip).Take(take).ToList());
        }

        [ExcludeFromCodeCoverage]
        private static bool TryParseDate(string dateString, out DateTime date)
        {
            string[] formats = ["dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "yyyy/MM/dd"];
            return DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }
    }
}
