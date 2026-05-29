using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static AMS.Auth_IdentityModel.IdentityModel;

namespace AMS.Data.AuthConfiguration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(new Role
        {
            Id = 1,
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR",
            Description = "Default role assigned to all employees."

        }, new Role
        {
            Id = 2,
            Name = "Seller",
            NormalizedName = "Seller",
            Description = "Default role assigned to all employees."
        }, new Role
        {
            Id = 3,
            Name = "Buyer",
            NormalizedName = "Buyer",
            Description = "Default role assigned to all customers."
        }

        );
    }
}