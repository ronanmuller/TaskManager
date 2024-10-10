namespace TaskManager.Domain.Entities
{
    public class Updates
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateDetail { get; set; } = string.Empty;
    }
}
