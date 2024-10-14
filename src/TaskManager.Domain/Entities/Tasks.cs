using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class Tasks
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskState Status { get; set; }
        public TaskPriority Priority { get; set; }

        // Navegação para o projeto
        public virtual Project Project { get; set; }

        // Navegação para os históricos de atualização
        public ICollection<TaskUpdateHistory> UpdateHistories { get; set; } = new List<TaskUpdateHistory>();

        // Navegação para os comentários da tarefa
        public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>(); 
    }
}