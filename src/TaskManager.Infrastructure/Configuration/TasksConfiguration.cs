using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Configuration
{
    public class TasksConfiguration : IEntityTypeConfiguration<Tasks>
    {
        public void Configure(EntityTypeBuilder<Tasks> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.Status)
                .HasConversion<string>();
            builder.Property(t => t.Priority)
                .HasConversion<string>();
        }
    }

}
