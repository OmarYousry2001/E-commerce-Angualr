using API.Base;
using BL.Contracts.Services.Custom;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : AppControllerBase
    {
        private readonly IProductService _ProductService;

        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }

        [HttpGet("Product-Get-All")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _ProductService.GetAllAsync();
            return NewResult(entities);
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entities = await _ProductService.FindByIdAsync(id);
            return NewResult(entities);
        }



    }
}
