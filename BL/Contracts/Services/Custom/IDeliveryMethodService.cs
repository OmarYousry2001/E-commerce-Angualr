using BL.Contracts.Services.Generic;
using BL.DTO.Entities;
using BL.GenericResponse;
using Domains.Order;

namespace BL.Contracts.Services.Custom
{
    public interface IDeliveryMethodService : IBaseService<DeliveryMethod, DeliveryMethodDTO>
    {
        //public Task<Response<DeliveryMethodDTO>> FindAsync(OrderDTO orderDTO, string BuyerEmail, Guid userId);
    }
}
