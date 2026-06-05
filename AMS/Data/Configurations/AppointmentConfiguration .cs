using AMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AppointmentDate)
                   .IsRequired();

            builder.Property(x => x.Purpose)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasMaxLength(100);

            // Client Relationship
            builder.HasOne(x => x.Client)
                   .WithMany(x => x.Appointments)
                   .HasForeignKey(x => x.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Advocate Relationship
            builder.HasOne(x => x.Advocate)
                   .WithMany(x => x.Appointments)
                   .HasForeignKey(x => x.AdvocateId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}