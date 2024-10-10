namespace TaskManager.Application.Dto
{
    public record CreateProjectDto
    {
        public string Name { get; init; } = string.Empty;
        public int UserId { get; init; }
    }


}
