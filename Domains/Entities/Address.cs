
using Domains.Entities.Base;
using Domains.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities
{
    public class Address :BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string State { get; set; } = null!;

        public string ApplicationUserId { get; set; } = null!;
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
