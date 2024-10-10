using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

public class TasksConfiguration : IEntityTypeConfiguration<Tasks>
{
    public void Configure(EntityTypeBuilder<Tasks> builder)
    {

        builder.ToTable("Tasks");
        builder.HasKey(t => t.Id);

        // Configurar a propriedade ProjectId como chave estrangeira
        builder.Property(t => t.ProjectId)
            .IsRequired(); // ProjectId é obrigatório

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200); // 

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.DueDate)
            .IsRequired();

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.Priority)
            .IsRequired(); // Definir como obrigatório

        builder.HasOne<Project>() 
            .WithMany(p => p.Tasks) // Cada Project tem muitas Tasks
            .HasForeignKey(t => t.ProjectId) 
            .OnDelete(DeleteBehavior.Cascade);

    }
}