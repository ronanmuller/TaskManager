using TaskManager.Domain.Enums;

namespace TaskManager.Application.Dto
{
    public record UpdateTaskDto
    {
        public string? Title { get; init; } 
        public string? Description { get; init; }
        public DateTime? DueDate { get; init; }
        public TaskState? Status { get; init; } 
    }

}
