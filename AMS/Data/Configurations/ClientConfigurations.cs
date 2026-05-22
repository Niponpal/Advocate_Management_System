using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AMS.Data.Configurations;

public class ClientConfigurations: IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable(nameof(Client));
        builder.HasKey(c => c.Id);
        builder.Property(c => c.ClientName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(c => c.Phone)
            .HasMaxLength(20);
        builder.Property(c => c.Address)
            .HasMaxLength(300);
        builder.Property(c => c.NIDNumber)
            .HasMaxLength(50);
        // Relationships
        builder.HasMany(c => c.Cases)
            .WithOne(ca => ca.Client)
            .HasForeignKey(ca => ca.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(c => c.Payments)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(c => c.Appointments)
            .WithOne(a => a.Client)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
