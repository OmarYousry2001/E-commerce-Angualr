using API.Base;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using Domains.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class ProductController : AppControllerBase
    {
        private readonly IProductService _ProductService;

        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }

        [HttpGet(Router.ProductRouting.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _ProductService.GetAllAsync();
            return NewResult(entities);
        }

        [HttpGet(Router.ProductRouting.GetById)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entities = await _ProductService.FindByIdAsync(id);
            return NewResult(entities);
        }

        [HttpPost(Router.ProductRouting.Create)]
        public async Task<IActionResult> Create([FromForm] ProductDTO product )
        {
            var entities = await _ProductService.SaveAndUploadImageAsync(product , GuidUserId);
            return NewResult(entities);
        }

        [HttpPut(Router.ProductRouting.Update)]
        public async Task<IActionResult> Update([FromForm]  ProductDTO product)
        {
            var entities = await _ProductService.SaveAndUploadImageAsync(product, GuidUserId);
            return NewResult(entities);
        }

        [HttpDelete(Router.ProductRouting.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entities = await _ProductService.DeleteAsync(id , GuidUserId);
            return NewResult(entities);
        }

    }
}
