using Domains.Order;
using Shared.DTOs.Base;

namespace BL.DTO.Entities
{
    public class OrderToReturnDTO : BaseDTO
    {
        public string BuyerEmail { get; set; } = null!;
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public ShipAddressDTO shippingAddress { get; set; }

        public IReadOnlyList<OrderItemDTO> orderItems { get; set; }
        public string deliveryMethod { get; set; }
        public string status { get; set; } = null!;
    }
}
