using BL.Contracts.IMapper;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using Domains.Entities.Product;

namespace BL.Services
{
    public class CategoryService : BaseService<Category, CategoryDTO>, ICategoryService
    {
        private readonly ITableRepository<Category> _categoryTableRepository;
        public CategoryService(ITableRepository<Category> categoryTableRepository, IBaseMapper mapper) : base(categoryTableRepository, mapper)
        {
            _categoryTableRepository = categoryTableRepository;
            _mapper = mapper;
        }

    }
}
