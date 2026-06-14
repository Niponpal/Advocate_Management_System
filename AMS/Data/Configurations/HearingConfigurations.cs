using AMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Data.Configurations;

public class HearingConfigurations:IEntityTypeConfiguration<Hearing>
{
    public void Configure(EntityTypeBuilder<Hearing> builder)
    {
        builder.ToTable(nameof(Hearing));
        builder.HasKey(h => h.Id);
        builder.Property(h => h.HearingDate)
            .IsRequired();
        builder.Property(h => h.Remarks)
            .HasMaxLength(500);
        builder.Property(h => h.HearingStatus)
            .HasMaxLength(50);
        // Relationships
        builder.HasOne(h => h.Case)
            .WithMany(c => c.Hearings)
            .HasForeignKey(h => h.CaseId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(h => h.Court)
            .WithMany(c => c.Hearings)
            .HasForeignKey(h => h.CourtId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
