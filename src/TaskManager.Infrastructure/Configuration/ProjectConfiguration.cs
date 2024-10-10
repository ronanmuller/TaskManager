using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired() 
            .HasMaxLength(100); 
        builder.Property(p => p.UserId)
            .IsRequired();

        // Configurar o relacionamento com a entidade Tasks
        builder.HasMany(p => p.Tasks) // Relacionamento "um para muitos"
            .WithOne() 
            .HasForeignKey("ProjectId") // Adiciona a chave estrangeira em Tasks
            .OnDelete(DeleteBehavior.Cascade); // Cascata de deleção
    }
}