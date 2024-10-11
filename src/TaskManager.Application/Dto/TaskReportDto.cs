using TaskManager.Domain.Enums;

namespace TaskManager.Application.Dto
{
    public record TaskReportDto
    {
        public string Status { get; init; } = "Completed";
        public int UserId { get; init; }
        public double AverageTasksPerUser { get; set; }
        public DateTime CompletionDate { get; set; }

    }
}