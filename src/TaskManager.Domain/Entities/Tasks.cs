using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskState Status { get; set; }
        public TaskPriority Priority { get; set; }
        public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    }
}
