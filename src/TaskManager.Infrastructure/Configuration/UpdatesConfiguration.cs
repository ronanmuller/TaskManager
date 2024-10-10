using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

public class UpdatesConfiguration : IEntityTypeConfiguration<Updates>
{
    public void Configure(EntityTypeBuilder<Updates> builder)
    {
        builder.ToTable("Updates");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.TaskId)
            .IsRequired(); 
        builder.Property(u => u.UserId)
            .IsRequired(); 
        builder.Property(u => u.UpdateDate)
            .IsRequired() 
            .HasColumnType("datetime"); 
        builder.Property(u => u.UpdateDetail)
            .IsRequired() 
            .HasMaxLength(500); 
    }
}