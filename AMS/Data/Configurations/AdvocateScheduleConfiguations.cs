using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Configurations;

public class AdvocateScheduleConfiguration
    : IEntityTypeConfiguration<AdvocateSchedule>
{
    public void Configure(EntityTypeBuilder<AdvocateSchedule> builder)
    {
        // Table Name
        builder.ToTable(nameof(AdvocateSchedule));

        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.ScheduleDate)
            .IsRequired();

        builder.Property(x => x.AvailableTime)
            .IsRequired()
            .HasMaxLength(100);

        // Relationship
        builder.HasOne(x => x.Advocate)
            .WithMany(x => x.AdvocateSchedules)
            .HasForeignKey(x => x.AdvocateId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index
        builder.HasIndex(x => new
        {
            x.AdvocateId,
            x.ScheduleDate
        });
    }
}