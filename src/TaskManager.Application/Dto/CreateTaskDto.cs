using TaskManager.Domain.Enums;

namespace TaskManager.Application.Dto
{
    public record CreateTaskDto
    {
        public int ProjectId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime DueDate { get; init; }
        public TaskState Status { get; init; } // Enum para o status da tarefa
        public TaskPriority Priority { get; init; } // Assume que você já tem o enum TaskPriority
    }

}
