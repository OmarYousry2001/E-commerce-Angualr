using BL.Contracts.Services.Generic;
using BL.DTO.Entities;
using BL.GenericResponse;
using Domains.Order;


namespace BL.Contracts.Services.Custom
{
    public interface IOrderService : IBaseService<Orders, OrderDTO>
    {
        public Task<Response<OrderDTO>> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail, Guid userId);
        public Task<Response<IEnumerable<OrderToReturnDTO>>> GetAllOrdersForUserAsync(string BuyerEmail);
        public  Task<Response<OrderToReturnDTO>> GetOrderByIdAsync(Guid Id, string BuyerEmail);

        public Task<Response<OrderToReturnDTO>> FindAsync(string PaymentIntentId);
        public Task<Response<bool>> SaveStatusPaymentAsync(OrderToReturnDTO dto, Guid userId);
        


    }
}
