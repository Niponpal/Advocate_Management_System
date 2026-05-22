using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Configurations
{
    public class CaseConfiguration : IEntityTypeConfiguration<Case>
    {
        public void Configure(EntityTypeBuilder<Case> builder)
        {
            // Table Name
            builder.ToTable(nameof(Case));

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.CaseNumber)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.CaseTitle)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.CaseType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.FilingDate)
                   .IsRequired();

            // Relationships

            // Advocate Relationship
            builder.HasOne(x => x.Advocate)
                   .WithMany(x => x.Cases)
                   .HasForeignKey(x => x.AdvocateId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Client Relationship
            builder.HasOne(x => x.Client)
                   .WithMany(x => x.Cases)
                   .HasForeignKey(x => x.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Hearing Relationship
            builder.HasMany(x => x.Hearings)
                   .WithOne(x => x.Case)
                   .HasForeignKey(x => x.CaseId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Document Relationship
            builder.HasMany(x => x.Documents)
                   .WithOne(x => x.Case)
                   .HasForeignKey(x => x.CaseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}