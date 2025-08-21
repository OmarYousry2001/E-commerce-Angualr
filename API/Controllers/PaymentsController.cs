using API.Base;
using BL.Contracts.GeneralService.UserManagement;
using BL.DTO.Entities;
using Domains.AppMetaData;
using Domains.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers
{
    [ApiController]

    public class PaymentsController : AppControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentStatusService _paymentStatusService;
        const string endpointSecret = "whsec_28cc3dec50be3eaba23c0d5217e31f075148d84948bb1e7aa84452952a3a9461";

        public PaymentsController(IPaymentService paymentService , IPaymentStatusService paymentStatusService   )
        {
            this._paymentService = paymentService;
            _paymentStatusService = paymentStatusService;

        }
        [Authorize]
        [HttpPost(Router.PaymentRouting.Create)]
        public async Task<ActionResult<CustomerBasket>> create(string basketId, Guid? deliveryId)
        {
            return await _paymentService.CreateOrUpdatePaymentAsync(basketId, deliveryId);
        }

        [HttpPost(Router.PaymentRouting.webhook)]
        public async Task<IActionResult> UpdateStatusWithStripe()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret, throwOnApiVersionMismatch: false);
                PaymentIntent intent;
                OrderToReturnDTO orders;
                // Handle the event
                if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    intent = stripeEvent.Data.Object as PaymentIntent;
                    orders = await _paymentStatusService.UpdateOrderFiled(intent.Id , GuidUserId);
                }
                else if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    intent = stripeEvent.Data.Object as PaymentIntent;
                    orders = await _paymentStatusService.UpdateOrderSuccess(intent.Id , GuidUserId);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
