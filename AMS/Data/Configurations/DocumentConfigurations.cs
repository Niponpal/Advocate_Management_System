using AMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Data.Configurations;

public class DocumentConfigurations: IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable(nameof(Document));
        builder.HasKey(d => d.Id);
        builder.Property(d => d.DocumentTitle)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(d => d.FilePath)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(d => d.DocumentType)
            .HasMaxLength(100);
        builder.Property(d => d.UploadDate)
            .IsRequired();
        // Relationships
        builder.HasOne(d => d.Case)
            .WithMany(c => c.Documents)
            .HasForeignKey(d => d.CaseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
