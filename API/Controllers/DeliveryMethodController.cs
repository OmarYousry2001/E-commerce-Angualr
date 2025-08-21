using API.Base;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using Domains.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class DeliveryMethodController : AppControllerBase
    {
        #region Fields
        private readonly IDeliveryMethodService _deliveryMethodService;
        #endregion

        #region Constructor
        public DeliveryMethodController(IDeliveryMethodService deliveryMethodService)
        {
            _deliveryMethodService = deliveryMethodService;
        }
        #endregion

        #region Apis
        [HttpGet(Router.DeliveryMethodRouting.GetAll)]
        public async Task<IActionResult> GetAll()
     => NewResult(await _deliveryMethodService.GetAllAsync());

        [HttpGet(Router.DeliveryMethodRouting.GetById)]
        public async Task<IActionResult> GetById(Guid id)
        => NewResult(await _deliveryMethodService.FindByIdAsync(id));

        [HttpPost(Router.DeliveryMethodRouting.Create)]
        public async Task<IActionResult> Create(DeliveryMethodDTO product)
        => NewResult(await _deliveryMethodService.SaveAsync(product, GuidUserId));

        [HttpPut(Router.DeliveryMethodRouting.Update)]
        public async Task<IActionResult> Update(DeliveryMethodDTO product)
        => NewResult(await _deliveryMethodService.SaveAsync(product, GuidUserId));

        [HttpDelete(Router.DeliveryMethodRouting.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        => NewResult(await _deliveryMethodService.DeleteAsync(id, GuidUserId)); 
        #endregion

    }
}
