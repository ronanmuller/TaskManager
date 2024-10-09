namespace TaskManager.Domain.Entities
{
    public class ProjectTask
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int TaskId { get; set; }
        public Tasks Task { get; set; }
    }
}
