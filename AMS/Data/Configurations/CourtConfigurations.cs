using AMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Configurations
{
    public class CourtConfiguration : IEntityTypeConfiguration<Court>
    {
        public void Configure(EntityTypeBuilder<Court> builder)
        {
            // Table Name
            builder.ToTable(nameof(Court));

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties

            builder.Property(x => x.CourtName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.CourtType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Location)
                   .IsRequired()
                   .HasMaxLength(300);

            // Relationships

            builder.HasMany(x => x.Hearings)
                   .WithOne(x => x.Court)
                   .HasForeignKey(x => x.CourtId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}