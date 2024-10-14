using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Configuration;

namespace TaskManager.Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public class ReadContext(DbContextOptions<ReadContext> options) : DbContext(options)
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TaskUpdateHistory> TaskUpdateHistories { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new TasksConfiguration());
            modelBuilder.ApplyConfiguration(new TaskUpdateHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new TaskCommentConfiguration());
        }
    }

}
