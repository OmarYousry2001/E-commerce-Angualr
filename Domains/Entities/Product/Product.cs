using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Product
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public double rating { get; set; }

        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
        public virtual ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();
    }
}
