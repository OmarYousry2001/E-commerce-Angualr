
using BL.DTO.Entities;

namespace BL.Contracts.GeneralService.UserManagement
{
    public interface IPaymentStatusService
    {
        public Task<OrderToReturnDTO> UpdateOrderSuccess(string PaymentIntent, Guid userId);
        public Task<OrderToReturnDTO> UpdateOrderFiled(string PaymentIntent, Guid userId);
    }
}
