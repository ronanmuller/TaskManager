using TaskManager.Domain.Enums;

namespace TaskManager.Application.Dto
{
    public record TaskReportDto
    {
        public string Status { get; init; } = "Completed";
        public int UserId { get; init; }
        public double CountTasksPerUser { get; set; }
        public string InitDate { get; set; }
        public string EndDate { get; set; }

    }
}