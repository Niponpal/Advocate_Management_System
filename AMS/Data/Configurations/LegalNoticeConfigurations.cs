using AMS.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Data.Configurations;

public class LegalNoticeConfigurations: IEntityTypeConfiguration<LegalNotice>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<LegalNotice> builder)
    {
        builder.ToTable(nameof(LegalNotice));
        builder.HasKey(ln => ln.Id);
        builder.Property(ln => ln.NoticeTitle).IsRequired().HasMaxLength(200);
        builder.Property(ln => ln.Description).IsRequired();
        builder.Property(ln => ln.NoticeDate).IsRequired();
        builder.HasOne(ln => ln.Client)
            .WithMany(c => c.LegalNotices)
            .HasForeignKey(ln => ln.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
