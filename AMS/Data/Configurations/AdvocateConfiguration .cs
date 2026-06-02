using AMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Data.Configurations;

public class AdvocateConfiguration : IEntityTypeConfiguration<Advocate>
{
    public void Configure(EntityTypeBuilder<Advocate> builder)
    {
        builder.ToTable(nameof(Advocate));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AdvocateName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LicenseNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Specialization)
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Phone)
            .HasMaxLength(20);

        builder.Property(x => x.Address)
            .HasMaxLength(300);

        builder.Property(x => x.ExperienceYears)
            .HasDefaultValue(0);

        // Unique License Number
        builder.HasIndex(x => x.LicenseNumber)
            .IsUnique();

        // Unique Email
        builder.HasIndex(x => x.Email)
            .IsUnique();

        // Relationship
        builder.HasMany(x => x.Cases)
            .WithOne(x => x.Advocate)
            .HasForeignKey(x => x.AdvocateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
