using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Order
{
    public class DeliveryMethod : BaseEntity
    {
        public DeliveryMethod()
        {

        }
        public DeliveryMethod(string name, decimal price, string deliveryTime, string description)
        {
            Name = name;
            Price = price;
            DeliveryTime = deliveryTime;
            Description = description;
        }

        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
    }
}