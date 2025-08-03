using Domains.Entities.Base;

namespace Domains.Entities.Product
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<Product> products { get; set; } = new HashSet<Product>();
    }
}
