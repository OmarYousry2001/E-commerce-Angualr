
using BL.Contracts.Services.Generic;
using BL.DTO.Entities;
using BL.GenericResponse;
using Domains.Entities.Product;

namespace BL.Contracts.Services.Custom
{
    public interface IProductService : IBaseService<Product, ProductDTO>
    {
        public  Task<Response<bool>> SaveAndUploadImageAsync(ProductDTO entityDTO, Guid userId);
        public  Task<Response<IEnumerable<GetProductDTO>>> GetAllWithIncludesAsync();
        public Task<Response<GetProductDTO>> FindByIdWithIncludesAsync(Guid Id);
    }
}
