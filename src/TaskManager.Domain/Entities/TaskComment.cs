namespace TaskManager.Domain.Entities
{
    public class TaskComment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public virtual Tasks Task { get; set; }
        public int UserId { get; set; } // Adicionando UserId para rastrear quem fez o comentário
    }
}