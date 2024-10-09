using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Configuration
{
    public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.HasKey(pt => new { pt.ProjectId, pt.TaskId });

            builder.HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(pt => pt.ProjectId);

            builder.HasOne(pt => pt.Task)
                .WithMany(t => t.ProjectTasks)
                .HasForeignKey(pt => pt.TaskId);
        }
    }

}
