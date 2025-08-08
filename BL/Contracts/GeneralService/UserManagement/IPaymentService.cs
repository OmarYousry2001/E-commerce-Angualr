using BL.DTO.Entities;
using Domains.Entities;
using Domains.Order;

namespace BL.Contracts.GeneralService.UserManagement
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId, Guid? deliveryId);
        //public  Task<OrderToReturnDTO> UpdateOrderField(string PaymentIntent, Guid userId);
        //public Task<OrderToReturnDTO> UpdateOrderSuccess(string PaymentIntent, Guid userId);
    }
}
