namespace TaskManager.Domain.Entities
{
    public class TaskUpdateHistory
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; } 
        public DateTime UpdateDate { get; set; } 
        public string UpdateDetail { get; set; } = string.Empty; 
        public Tasks Task { get; set; }
    }
}