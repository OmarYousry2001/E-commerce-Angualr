using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using Domains.Order;

namespace BL.GeneralService.UserManagement
{
    public class PaymentStatusService: IPaymentStatusService
    {
        private readonly IOrderService _orderService;

        public PaymentStatusService(IOrderService orderService)
        {
            this._orderService = orderService;
        }   
        public async Task<OrderToReturnDTO> UpdateOrderSuccess(string PaymentIntent, Guid userId)
        {
            var orderResponse = await _orderService.FindAsync(PaymentIntent);

            if (!orderResponse.Succeeded)
            {
                return null;
            }
            orderResponse.Data.status = Status.PaymentReceived.ToString();
            await _orderService.SaveStatusPaymentAsync(orderResponse.Data, userId);
            return orderResponse.Data;
        }

        public async Task<OrderToReturnDTO> UpdateOrderFiled(string PaymentIntent, Guid userId)
        {
            var orderResponse = await _orderService.FindAsync(PaymentIntent);
            if (orderResponse is null)
            {
                return null;
            }
            orderResponse.Data.status = Status.PaymentFailed.ToString();
            await _orderService.SaveStatusPaymentAsync(orderResponse.Data, userId);
            return orderResponse.Data;
        }

        
    }
}
