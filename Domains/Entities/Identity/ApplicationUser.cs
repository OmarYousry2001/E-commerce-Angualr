using Domains.Entities;
using Domains.Entities.Identity;
using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;

namespace Domains.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = null!;
        public Address Address { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public int CurrentState { get; set; } = 1;
        public DateTime LastLoginDate { get; set; }
        public ICollection<UserRefreshToken> RefreshTokens { get; set; } = new List<UserRefreshToken>();

        [EncryptColumn]
        public string? Code { get; set; }

    }
}
