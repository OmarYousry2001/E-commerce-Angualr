using API.Base;
using BL.Contracts.GeneralService.CMS;
using Domains.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : AppControllerBase
    {
        private readonly ICustomerBasketService _customerBasketService;

        public BasketsController(ICustomerBasketService customerBasketService)
        {
            _customerBasketService = customerBasketService;
        }

        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var entities = await _customerBasketService.GetBasketAsync(id);
            return NewResult(entities);
        }

        [HttpPost("update-basket")]
        public async Task<IActionResult> Add(CustomerBasket basket)
        {
            var entity = await _customerBasketService.UpdateBasketAsync(basket);
            return NewResult(entity);
        }

        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _customerBasketService.DeleteBasketAsync(id);
            return NewResult(result);
        }




    }
}
