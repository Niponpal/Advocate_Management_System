using Microsoft.EntityFrameworkCore;

namespace AMS.Data.Configurations;

public class PaymentConfigurations: IEntityTypeConfiguration<Payment>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(nameof(Payment));
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(p => p.PaymentDate).IsRequired();
        builder.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(50);
        builder.HasOne(p => p.Client)
            .WithMany(c => c.Payments)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
