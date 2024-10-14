using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Configuration
{
    [ExcludeFromCodeCoverage]
    public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
    {
        public void Configure(EntityTypeBuilder<TaskComment> builder)
        {
            builder.HasKey(c => c.Id); 

            builder.Property(c => c.Comment)
                .IsRequired() // Campo de comentário obrigatório
                .HasMaxLength(500); // Limite de 500 caracteres, ajuste conforme necessário

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()"); // Define a data de criação padrão

            builder.HasOne(c => c.Task) // Configurando o relacionamento com a entidade Task
                .WithMany(t => t.Comments) // Uma tarefa pode ter muitos comentários
                .HasForeignKey(c => c.TaskId); 
        }
    }
}