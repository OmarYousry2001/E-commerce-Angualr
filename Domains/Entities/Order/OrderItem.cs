using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Order
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {

        }
        public OrderItem(Guid productItemId, string mainImage, string productName, decimal price, int quantity ,Guid createdBy)
        {
            ProductItemId = productItemId;
            MainImage = mainImage;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
            CreatedBy = createdBy;


        }

        public Guid ProductItemId { get; set; }
        public string MainImage { get; set; }
        public string ProductName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}