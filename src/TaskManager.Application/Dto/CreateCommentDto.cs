namespace TaskManager.Application.Dto;

public record CreateCommentDto
{
    public string Comment { get; init; } = string.Empty; // Texto do comentário
    public int TaskId { get; init; }
}