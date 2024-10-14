using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.MediatorR.Commands.TaskComments
{
    [ExcludeFromCodeCoverage]
    public class CreateCommentCommand : IRequest<int>
    {
        public string Comment { get; init; } = string.Empty; // Texto do comentário
        public int TaskId { get; init; } // ID da tarefa à qual o comentário pertence
    }
}