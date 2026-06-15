using AMS.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Data.Configurations;

public class InvoiceConfigurations: IEntityTypeConfiguration<Invoice>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable(nameof(Invoice));
        builder.HasKey(i => i.Id);
        builder.Property(i => i.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(i => i.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        builder.Property(i => i.InvoiceDate)
            .IsRequired();
        // Relationships
        builder.HasOne(i => i.Client)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
