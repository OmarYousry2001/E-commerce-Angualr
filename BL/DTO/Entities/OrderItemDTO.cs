
namespace BL.DTO.Entities
{
    public class OrderItemDTO
    {
        public Guid ProductItemId { get; set; }
        public string MainImage { get; set; } = null!;
        public string ProductName { get; set; } = null!;    
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
