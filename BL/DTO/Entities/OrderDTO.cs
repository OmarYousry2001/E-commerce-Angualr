using Shared.DTOs.Base;

namespace BL.DTO.Entities
{
    public class OrderDTO 
    {
        public Guid DeliveryMethodId { get; set; }
        public string BasketId { get; set; } = null!;
        public ShipAddressDTO ShipAddress { get; set; }
    }

}
