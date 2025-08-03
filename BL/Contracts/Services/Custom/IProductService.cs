
using BL.Contracts.Services.Generic;
using BL.DTO.Entities;
using Domains.Entities.Product;

namespace BL.Contracts.Services.Custom
{
    public interface IProductService : IBaseService<Product, ProductDTO>
    {

    }
}
