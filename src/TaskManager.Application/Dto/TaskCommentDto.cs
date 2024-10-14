namespace TaskManager.Application.Dto;

public record TaskCommentDto
{
    public int Id { get; init; }
    public int TaskId { get; init; }
    public string Content { get; init; }
    public DateTime CreatedDate { get; init; }
    public int UserId { get; init; } // Incluindo o UserId na resposta, se necessário
}