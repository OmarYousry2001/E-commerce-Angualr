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
            return NewResult(await _ProductService.GetAllAsync());
        }

        [HttpGet(Router.ProductRouting.GetById)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return NewResult(await _ProductService.FindByIdAsync(id));
        }

        [HttpPost(Router.ProductRouting.Create)]
        public async Task<IActionResult> Create([FromForm] ProductDTO product )
        {
            return NewResult(await _ProductService.SaveAndUploadImageAsync(product, GuidUserId));
        }

        [HttpPut(Router.ProductRouting.Update)]
        public async Task<IActionResult> Update([FromForm]  ProductDTO product)
        {
            return NewResult(await _ProductService.SaveAndUploadImageAsync(product, GuidUserId));
        }

        [HttpDelete(Router.ProductRouting.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            return NewResult(await _ProductService.DeleteAsync(id, GuidUserId));
        }

    }
}
