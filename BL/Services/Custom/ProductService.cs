using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using BL.GenericResponse;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Product;

namespace BL.Services.Custom
{
    public class ProductService : BaseService<Product, ProductDTO>, IProductService
    {
        private readonly ITableRepository<Product> _ProductTableRepository;
        private readonly ITableRepository<Photo> _PhotoTableRepository;

        private readonly IFileUploadService _fileUploadService;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(
            ITableRepository<Product> ProductTableRepository,
            ITableRepository<Photo> PhotoTableRepository,
            IUnitOfWork unitOfWork,
            IFileUploadService fileUploadService,
            IBaseMapper mapper) : base(ProductTableRepository, mapper)
        {
            _ProductTableRepository = ProductTableRepository;
            _PhotoTableRepository = PhotoTableRepository;
            _fileUploadService = fileUploadService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Response<IEnumerable<GetProductDTO>>> GetAllWithIncludesAsync()
        {
            var entitiesList = await _ProductTableRepository.GetAsync(
              x => x.CurrentState == 1,
              x => x.Photos,
              x => x.Ratings,
              x => x.Category
            );
            if (entitiesList == null) return NotFound<IEnumerable<GetProductDTO>>();
            var dtoList = _mapper.MapList<Product, GetProductDTO>(entitiesList);
            return Success(dtoList);
        }
        public async Task<Response<GetProductDTO>> FindByIdWithIncludesAsync(Guid Id)
        {
            var entity = await _ProductTableRepository.FindAsync(
          x => x.CurrentState == 1 && x.Id == Id,
          x => x.Category,
          x => x.Photos,
          x => x.Ratings
         );
            if (entity == null) return NotFound<GetProductDTO>();
            var dto = _mapper.MapModel<Product, GetProductDTO>(entity);
            return Success(dto);
        }
        public async Task<Response<bool>> SaveAndUploadImageAsync(ProductDTO entityDTO, Guid userId)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (entityDTO.Photo == null && entityDTO.ImageName == null) return BadRequest<bool>();

                var entity = _mapper.MapModel<ProductDTO, Product>(entityDTO);
                await _ProductTableRepository.SaveAsync(entity, userId);
                if (entityDTO.Photo?.Any() == true)
                {
                        var imagePaths = await _fileUploadService.AddImagesAsync(entityDTO.Photo, "Products", entityDTO.Name, entityDTO.ImageName);
                        var photos = imagePaths.Select(path => new Photo
                    {
                        ImagePath = path,
                        ProductId = entity.Id

                    }).ToList();

                    await _PhotoTableRepository.AddRangeAsync(photos, userId);

                }
                await transaction.CommitAsync();
                return Success(true);
            }

            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }



        }






    }
}
