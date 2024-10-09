using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Configuration;

namespace TaskManager.Infrastructure.Context
{
    public class ReadContext(DbContextOptions<ReadContext> options) : DbContext(options)
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new TasksConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTaskConfiguration());
        }
    }

}
