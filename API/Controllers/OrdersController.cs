using API.Base;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using Domains.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class OrdersController : AppControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost(Router.OrdersRouting.Create)]
        public async Task<IActionResult> Create(OrderDTO product)
        {
            return NewResult(await _orderService.CreateOrdersAsync(product, UserEmail,  GuidUserId));
        }

        [HttpGet(Router.OrdersRouting.GetOrdersForUser)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForUser()
        {
            return NewResult(await _orderService.GetAllOrdersForUserAsync(UserEmail));
        }

        [HttpGet(Router.OrdersRouting.GetById)]
        public async Task<ActionResult<OrderToReturnDTO>> GetById(Guid id)
        {
            return NewResult(await _orderService.GetOrderByIdAsync(id, UserEmail));
        }

        //[HttpGet("get-delivery")]
        //public async Task<ActionResult> GetDeliver()
        //=> Ok(await _orderService.GetDeliveryMethodAsync());

    }
}
