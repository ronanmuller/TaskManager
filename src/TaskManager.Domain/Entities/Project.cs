namespace TaskManager.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
}
