using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Product
{
    public class Photo : BaseEntity
    {
        public string ImagePath { get; set; } = null!;

        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
