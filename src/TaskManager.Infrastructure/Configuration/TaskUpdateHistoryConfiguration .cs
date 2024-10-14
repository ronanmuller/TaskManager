using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Configuration;

[ExcludeFromCodeCoverage]
public class TaskUpdateHistoryConfiguration : IEntityTypeConfiguration<TaskUpdateHistory>
{
    public void Configure(EntityTypeBuilder<TaskUpdateHistory> builder)
    {
        builder.ToTable("TaskUpdateHistory");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.TaskId)
            .IsRequired();

        builder.Property(u => u.UserId)
            .IsRequired();

        builder.Property(u => u.UpdateDetail)
            .IsRequired()
            .HasMaxLength(1000); 

        builder.Property(u => u.UpdateDate)
            .IsRequired();

        builder.HasOne(u => u.Task)
            .WithMany(t => t.UpdateHistories)
            .HasForeignKey(u => u.TaskId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}