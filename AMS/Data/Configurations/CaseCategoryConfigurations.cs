using AMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Data.Configurations;

public class CaseCategoryConfigurations: IEntityTypeConfiguration<CaseCategory>
{
    public void Configure(EntityTypeBuilder<CaseCategory> builder)
    {
            builder.ToTable(nameof(CaseCategory));
        builder.HasKey(cc => cc.Id);
        builder.Property(cc => cc.CategoryName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(cc => cc.IsActive)
            .IsRequired();
    }
}
