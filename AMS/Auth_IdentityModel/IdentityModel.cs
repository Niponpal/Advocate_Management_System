using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Auth_IdentityModel;


public class IdentityModel
{
    // ------------------ User Table ------------------
    [Table("Users")]
    public class User : IdentityUser<long>
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime RegisterDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }

    }

    // ------------------ Roles ------------------
    [Table("Roles")]
    public class Role : IdentityRole<long>
    {
        public Role() { }
        public Role(string name) { Name = name; }

        public int StatusId { get; set; }
        public string Description { get; set; }

        public long CreatedBy { get; set; }
        public DateTimeOffset CreatedDateUtc { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedDateUtc { get; set; }
    }

    // ------------------ User Roles ------------------
    [Table("UserRoles")]
    public class UserRole : IdentityUserRole<long>
    {
    }

    // ------------------ User Claims ------------------
    [Table("UserClaims")]
    public class UserClaim : IdentityUserClaim<long>
    {
    }

    // ------------------ User Logins ------------------
    // ❌ এখানে আর কোন Key / ForeignKey override করা যাবে না
    // IdentityUserLogin-এর PK = (LoginProvider, ProviderKey)
    [Table("UserLogins")]
    public class UserLogin : IdentityUserLogin<long>
    {
    }

    // ------------------ Role Claims ------------------
    [Table("RoleClaims")]
    public class RoleClaim : IdentityRoleClaim<long>
    {
    }

    // ------------------ User Tokens ------------------
    [Table("UserTokens")]
    public class UserToken : IdentityUserToken<long>
    {
    }
}