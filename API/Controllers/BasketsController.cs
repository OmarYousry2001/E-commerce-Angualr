using API.Base;
using BL.Contracts.GeneralService.CMS;
using Domains.AppMetaData;
using Domains.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class BasketsController : AppControllerBase
    {
        private readonly ICustomerBasketService _customerBasketService;

        public BasketsController(ICustomerBasketService customerBasketService)
        {
            _customerBasketService = customerBasketService;
        }

        [HttpGet(Router.BasketRouting.Get)]
        public async Task<IActionResult> Get(string id)
        {
            var entities = await _customerBasketService.GetBasketAsync(id);
            return NewResult(entities);
        }

        [HttpPost(Router.BasketRouting.Update)]
        public async Task<IActionResult> Update(CustomerBasket basket)
        {
            var entity = await _customerBasketService.UpdateBasketAsync(basket);
            return NewResult(entity);
        }

        [HttpDelete(Router.BasketRouting.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _customerBasketService.DeleteBasketAsync(id);
            return NewResult(result);
        }




    }
}
