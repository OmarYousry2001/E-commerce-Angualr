using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using BL.Services;
using DAL.Contracts.UnitOfWork;
using Domains.Entities;
using Domains.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace BL.GeneralService.UserManagement
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomerBasketService _customerBasketService;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IOrderPaymentHelper _orderHelper;

        public PaymentService(IConfiguration configuration,
           ICustomerBasketService customerBasketService,
          IUnitOfWork unitOfWork
         //IOrderPaymentHelper orderHelper

          )
        {
            _configuration = configuration;
            _customerBasketService = customerBasketService;
            _unitOfWork = unitOfWork;
            //_orderHelper = orderHelper;   
        }
    
        public async Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId, Guid? deliveryId)
        {
            var basketResponse = await _customerBasketService.GetBasketAsync(basketId);
            StripeConfiguration.ApiKey = _configuration["StripSetting:secretKey"];
            decimal shippingPrice = 0m;
            if (deliveryId.HasValue)
            {
                var deliveryResponse = await _unitOfWork.TableRepository<DeliveryMethod>().FindByIdAsync(deliveryId.Value);
                shippingPrice = deliveryResponse.Price;
            }

            foreach (var item in basketResponse.Data.BasketItems)
            {
                var product = await  _unitOfWork.TableRepository<Domains.Entities.Product.Product>().FindByIdAsync(item.Id);
                item.Price = product.NewPrice;
            }
            PaymentIntentService paymentIntentService = new();
            PaymentIntent _intent;
            if (string.IsNullOrEmpty(basketResponse.Data.PaymentIntentId))
            {
                var option = new PaymentIntentCreateOptions
                {
                    Amount = (long)basketResponse.Data.BasketItems.Sum(m => m.Quantity * (m.Price * 100)) + (long)(shippingPrice * 100),

                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                _intent = await paymentIntentService.CreateAsync(option);
                basketResponse.Data.PaymentIntentId = _intent.Id;
                basketResponse.Data.ClientSecret = _intent.ClientSecret;
            }
            else
            {
                var option = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basketResponse.Data.BasketItems.Sum(m => m.Quantity * (m.Price * 100)) + (long)(shippingPrice * 100),

                };
                await paymentIntentService.UpdateAsync(basketResponse.Data.PaymentIntentId, option);
            }
            await _customerBasketService.UpdateBasketAsync(basketResponse.Data);
            return basketResponse.Data;
        }
        //public async Task<OrderToReturnDTO> UpdateOrderSuccess(string paymentIntent, Guid userId)
        //{

        //    //var orderResponse = await _orderService.FindAsync(PaymentIntent);

        //    //if (!orderResponse.Succeeded)
        //    //{
        //    //    return null;
        //    //}
        //    //orderResponse.Data.status = Status.PaymentReceived.ToString();
        //    //await _orderService.SaveStatusPaymentAsync(orderResponse.Data, userId);
        //    //return orderResponse.Data;
        //    //return await _orderHelper.UpdateOrderSuccess(paymentIntent, userId);
        //}
        //public async Task<OrderToReturnDTO> UpdateOrderFiled(string paymentIntent, Guid userId)
        //{
        //    var orderResponse = await _orderService.FindAsync(PaymentIntent);
        //    if (orderResponse is null)
        //    {
        //        return null;
        //    }
        //    orderResponse.Data.status = Status.PaymentFailed.ToString();
        //    await _orderService.SaveStatusPaymentAsync(orderResponse.Data, userId);
        //    return orderResponse.Data;
        //    //return await _orderHelper.UpdateOrderFailed(paymentIntent, userId);
        //}

       
   
    }
}
