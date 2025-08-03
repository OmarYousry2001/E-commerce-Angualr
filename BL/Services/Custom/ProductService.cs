using BL.Contracts.IMapper;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using BL.GenericResponse;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using Domains.Entities.Product;

namespace BL.Services.Custom
{
    public class ProductService : BaseService<Product, ProductDTO>, IProductService
    {
        private readonly ITableRepository<Product> _baseTableRepository;

        public ProductService(ITableRepository<Product> baseTableRepository, IBaseMapper mapper) : base(baseTableRepository, mapper)
        {
            _baseTableRepository = baseTableRepository;
            _mapper = mapper;

        }

        public override async Task<Response<IEnumerable<ProductDTO>>> GetAllAsync()
        {
            var entitiesList = await _baseTableRepository.GetAsync(
              x => x.CurrentState == 1,
              x => x.Photos,
              x => x.Ratings,
              x => x.Category
            );
            if (entitiesList == null) return NotFound<IEnumerable<ProductDTO>>();
            var dtoList = _mapper.MapList<Product, ProductDTO>(entitiesList);
            return Success(dtoList);
        }
        public override async Task<Response<ProductDTO>> FindByIdAsync(Guid Id)
        {
            var entity = await _baseTableRepository.FindAsync(
          x => x.CurrentState == 1 && x.Id == Id,
          x => x.Category,
          x => x.Photos,
          x => x.Ratings
         );
            if (entity == null) return NotFound<ProductDTO>();
            var dto = _mapper.MapModel<Product, ProductDTO>(entity);
            return Success(dto);
        }




    }
}
