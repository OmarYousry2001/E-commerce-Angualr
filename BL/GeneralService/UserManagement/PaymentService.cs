using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using DAL.Contracts.UnitOfWork;
using Domains.Entities;
using Domains.Order;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace BL.GeneralService.UserManagement
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomerBasketService _customerBasketService;
        //private readonly IDeliveryMethodService _deliveryMethodService;
        //private readonly IProductService _productService;
        //private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;


        public PaymentService(IConfiguration configuration,
           ICustomerBasketService customerBasketService,
          IUnitOfWork unitOfWork

          )
        {
            _configuration = configuration;
            _customerBasketService = customerBasketService;
            //_deliveryMethodService = deliveryMethodService;
            //_productService = productService;
            //_orderService = orderService;
            _unitOfWork = unitOfWork;
        }
        //public async Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId, Guid? deliveryId)
        //{
        //    var basketResponse = await _customerBasketService.GetBasketAsync(basketId);
        //    StripeConfiguration.ApiKey = _configuration["StripSetting:secretKey"];
        //    decimal shippingPrice = 0m;
        //    if (deliveryId.HasValue)
        //    {
        //        //var delivery = await _context.DeliveryMethods.AsNoTracking()
        //        //    .FirstOrDefaultAsync(m => m.Id == deliveryId.Value);
        //        var deliveryResponse = await _deliveryMethodService.FindByIdAsync(deliveryId.Value);
        //        shippingPrice = deliveryResponse.Data.Price;
        //    }

        //    foreach (var item in basketResponse.Data.BasketItems)
        //    {
        //        //var product = await work.productRepositry.GetByIdAsync(item.Id);
        //        var productResponse = await _productService.FindByIdAsync(item.Id);
        //        item.Price = productResponse.Data.NewPrice;
        //    }
        //    PaymentIntentService paymentIntentService = new();
        //    PaymentIntent _intent;
        //    if (string.IsNullOrEmpty(basketResponse.Data.PaymentIntentId))
        //    {
        //        var option = new PaymentIntentCreateOptions
        //        {
        //            Amount = (long)basketResponse.Data.BasketItems.Sum(m => m.Quantity * (m.Price * 100)) + (long)(shippingPrice * 100),

        //            Currency = "USD",
        //            PaymentMethodTypes = new List<string> { "card" }
        //        };
        //        _intent = await paymentIntentService.CreateAsync(option);
        //        basketResponse.Data.PaymentIntentId = _intent.Id;
        //        basketResponse.Data.ClientSecret = _intent.ClientSecret;
        //    }
        //    else
        //    {
        //        var option = new PaymentIntentUpdateOptions
        //        {
        //            Amount = (long)basketResponse.Data.BasketItems.Sum(m => m.Quantity * (m.Price * 100)) + (long)(shippingPrice * 100),

        //        };
        //        await paymentIntentService.UpdateAsync(basketResponse.Data.PaymentIntentId, option);
        //    }
        //    await _customerBasketService.UpdateBasketAsync(basketResponse.Data);
        //    return basketResponse.Data;
        //}

        public async Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId, Guid? deliveryId)
        {
            var basketResponse = await _customerBasketService.GetBasketAsync(basketId);
            StripeConfiguration.ApiKey = _configuration["StripSetting:secretKey"];
            decimal shippingPrice = 0m;
            if (deliveryId.HasValue)
            {
                //var delivery = await _context.DeliveryMethods.AsNoTracking()
                //    .FirstOrDefaultAsync(m => m.Id == deliveryId.Value);
                var deliveryResponse = await _unitOfWork.TableRepository<DeliveryMethod>().FindByIdAsync(deliveryId.Value);
                shippingPrice = deliveryResponse.Price;
            }

            foreach (var item in basketResponse.Data.BasketItems)
            {
                //var product = await work.productRepositry.GetByIdAsync(item.Id);
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





        //public async Task<OrderToReturnDTO> UpdateOrderField(string PaymentIntent, Guid userId)
        //{
        //    //var order = await _context.Orders.FirstOrDefaultAsync(m => m.PaymentIntentId == PaymentInten);

        //    var orderResponse = await _orderService.FindAsync(PaymentIntent);

        //    if (!orderResponse.Succeeded)
        //    {
        //        return null;
        //    }
        //    orderResponse.Data.status = Status.PaymentFailed.ToString();
        //    await _orderService.SaveStatusPaymentAsync(orderResponse.Data, userId);
        //    //_context.Orders.Update(order);
        //    //await _context.SaveChangesAsync();
        //    return orderResponse.Data;
        //}

        //public async Task<OrderToReturnDTO> UpdateOrderSuccess(string PaymentIntent, Guid userId)
        //{
        //    var orderResponse = await _orderService.FindAsync(PaymentIntent);

        //    if (!orderResponse.Succeeded)
        //    {
        //        return null;
        //    }
        //    orderResponse.Data.status = Status.PaymentReceived.ToString();
        //    await _orderService.SaveStatusPaymentAsync(orderResponse.Data, userId);
        //    return orderResponse.Data;
        //}
    }
}
