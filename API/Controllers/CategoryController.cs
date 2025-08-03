using API.Base;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : AppControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("Product-Get-All")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _categoryService.GetAllAsync();
            return NewResult(entities);
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entities = await _categoryService.FindByIdAsync(id);
            return NewResult(entities);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create( CategoryDTO product)
        {
            var entities = await _categoryService.SaveAsync(product, GuidUserId);
            return NewResult(entities);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update( CategoryDTO product)
        {
            var entities = await _categoryService.SaveAsync(product, GuidUserId);
            return NewResult(entities);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Update(Guid id)
        {
            var entities = await _categoryService.DeleteAsync(id, GuidUserId);
            return NewResult(entities);
        }

    }
}
