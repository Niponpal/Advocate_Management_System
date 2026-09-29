using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Auth_IdentityModel
{
    public class IdentityModel
    {
        // =====================================================
        // USER
        // =====================================================

        [Table("Users")]
        public class User : IdentityUser<long>
        {
            public string FullName { get; set; } = string.Empty;

            // We use Phone instead of IdentityUser.PhoneNumber
            public string Phone { get; set; } = string.Empty;

            public string Address { get; set; } = string.Empty;

            public DateTime RegisterDate { get; set; }

            public long CreatedBy { get; set; }

            public DateTimeOffset CreatedDate { get; set; }

            public long? UpdatedBy { get; set; }

            public DateTimeOffset? UpdatedDate { get; set; }
        }

        // =====================================================
        // ROLE
        // =====================================================

        [Table("Roles")]
        public class Role : IdentityRole<long>
        {
            public Role()
            {
            }

            public Role(string name)
            {
                Name = name;
                NormalizedName = name.ToUpperInvariant();
            }

            public int StatusId { get; set; }

            public string Description { get; set; } = string.Empty;

            public long CreatedBy { get; set; }

            public DateTimeOffset CreatedDateUtc { get; set; }

            public long? UpdatedBy { get; set; }

            public DateTimeOffset? UpdatedDateUtc { get; set; }
        }

        // =====================================================
        // USER ROLES
        // =====================================================

        [Table("UserRoles")]
        public class UserRole : IdentityUserRole<long>
        {
        }

        // =====================================================
        // USER CLAIMS
        // =====================================================

        [Table("UserClaims")]
        public class UserClaim : IdentityUserClaim<long>
        {
        }

        // =====================================================
        // USER LOGINS
        // =====================================================

        [Table("UserLogins")]
        public class UserLogin : IdentityUserLogin<long>
        {
        }

        // =====================================================
        // ROLE CLAIMS
        // =====================================================

        [Table("RoleClaims")]
        public class RoleClaim : IdentityRoleClaim<long>
        {
        }

        // =====================================================
        // USER TOKENS
        // =====================================================

        [Table("UserTokens")]
        public class UserToken : IdentityUserToken<long>
        {
        }
    }
}