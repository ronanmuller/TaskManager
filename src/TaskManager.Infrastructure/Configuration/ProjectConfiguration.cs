using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Configuration;

[ExcludeFromCodeCoverage]
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

        builder.HasMany(p => p.Tasks) 
            .WithOne() 
            .HasForeignKey("ProjectId") 
            .OnDelete(DeleteBehavior.Restrict);  //Assumi a exclusao logica entao nao tem cascade
    }
}