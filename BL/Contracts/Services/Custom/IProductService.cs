using BL.Contracts.Services.Generic;
using BL.DTO.Entities;
using BL.GenericResponse;
using DAL.Models;
using Domains.Entities.Product;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Services.Custom
{
    public interface IProductService : IBaseService<Product, ProductDTO>
    {
        public  Task<Response<bool>> SaveAndUploadImageAsync(ProductDTO entityDTO, Guid userId);
        public  Task<Response<IEnumerable<GetProductDTO>>> GetAllWithIncludesAsync();
        public Task<Response<GetProductDTO>> FindByIdWithIncludesAsync(Guid Id);
        public  Task<PaginatedResult<GetProductDTO>> GetProductsPaginatedListAsync(BaseSearchCriteriaModel SearchCriterial);
    }
}
