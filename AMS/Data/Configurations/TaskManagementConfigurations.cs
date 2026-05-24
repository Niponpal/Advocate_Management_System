using Microsoft.EntityFrameworkCore;

namespace AMS.Data.Configurations;

public class TaskManagementConfigurations : IEntityTypeConfiguration<TaskManagement>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TaskManagement> builder)
    {
        builder.ToTable(nameof(TaskManagement));
        builder.HasKey(tm => tm.Id);
        builder.Property(tm => tm.TaskTitle).IsRequired().HasMaxLength(200);
        builder.Property(tm => tm.Description).IsRequired();
        builder.Property(tm => tm.DueDate).IsRequired();
        builder.Property(tm => tm.Status).IsRequired().HasMaxLength(50);
        builder.HasOne(tm => tm.Advocate)
            .WithMany(a => a.TaskManagements)
            .HasForeignKey(tm => tm.AdvocateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
