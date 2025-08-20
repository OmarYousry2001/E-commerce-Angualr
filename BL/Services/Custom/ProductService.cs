using AutoMapper.QueryableExtensions;
using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using BL.GenericResponse;
using BL.Mapper.Base;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using DAL.UnitOfWork;
using Domains.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;


namespace BL.Services.Custom
{
    public class ProductService : BaseService<Product, ProductDTO>, IProductService
    {
        //private readonly ITableRepository<Product> _ProductTableRepository;
        //private readonly ITableRepository<Photo> _PhotoTableRepository;

        private readonly IFileUploadService _fileUploadService;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(
            IUnitOfWork unitOfWork,
            IFileUploadService fileUploadService,
            IBaseMapper mapper) : base(unitOfWork.TableRepository<Product>(), mapper)
        {
            _fileUploadService = fileUploadService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Response<IEnumerable<GetProductDTO>>> GetAllWithIncludesAsync()
        {
            var entitiesList = await _unitOfWork.TableRepository<Product>().GetAsync(
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
            var entity = await _unitOfWork.TableRepository<Product>().FindAsync(
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
                await _unitOfWork.TableRepository<Product>().SaveAsync(entity, userId);
                if (entityDTO.Photo?.Any() == true)
                {
                    var imagePaths = await _fileUploadService.AddImagesAsync(entityDTO.Photo, "Products", entityDTO.Name, entityDTO.ImageName);
                    var photos = imagePaths.Select(path => new Photo
                    {
                        ImagePath = path,
                        ProductId = entity.Id

                    }).ToList();

                    await _unitOfWork.TableRepository<Photo>().AddRangeAsync(photos, userId);

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
        public async Task<PaginatedResult<GetProductDTO>> GetProductsPaginatedListAsync(BaseSearchCriteriaModel SearchCriterial)
        {
            Expression<Func<Product, bool>> filter = null;

            if (SearchCriterial.CategoryId != null && SearchCriterial.CategoryId != default)
                filter = x => x.CategoryId == SearchCriterial.CategoryId;

            var query = _unitOfWork.TableRepository<Product>()
                 .BuildQuery(SearchCriterial.PageNumber,
                  SearchCriterial.PageSize,
                    filter,
                  SearchCriterial.OrderingEnum)
                  .Include(x => x.Category)
                  .Include(m => m.Photos)
                  .Where(x => x.CurrentState == 1);

            //filtering by word
            if (!string.IsNullOrEmpty(SearchCriterial.SearchTerm))
            {
                var searchWords = SearchCriterial.SearchTerm.Split(' ');
                query = query.Where(m => searchWords.All(word =>

                m.Name.ToLower().Contains(word.ToLower()) ||
                m.Description.ToLower().Contains(word.ToLower())

                ));
            }
            var paginatedList = await _mapper
                .ProjectToPaginatedListAsync<GetProductDTO>(query, SearchCriterial.PageNumber, SearchCriterial.PageSize);
            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;

            //var FilterQuery = _movieService.FilterMoviePaginatedQueryable(request.MovieOrdering, request.Search);
            //  var PaginatedList = await _mapper.ProjectTo<GetMoviesPaginatedListResponse>(FilterQuery).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            //  PaginatedList.Meta = new { Count = PaginatedList.Data.Count() };
            //  return PaginatedList;

            ////filtering by category Id
            //if (SearchCriterial.CategoryId != default)
            //    queryable = queryable.Where(m => m.CategoryId == productParams.CategoryId);

            //var queryable = _unitOfWork.TableRepository<Product>().GetTableNoTracking()
            //    .Include(x => x.Category)
            //    .Include(m => m.Photos)
            //    .Where(x => x.CurrentState == 1);

            ////filtering by word
            //if (!string.IsNullOrEmpty(SearchCriterial.SearchTerm))
            //{
            //    var searchWords = SearchCriterial.SearchTerm.Split(' ');
            //    queryable = queryable.Where(m => searchWords.All(word =>

            //    m.Name.ToLower().Contains(word.ToLower()) ||
            //    m.Description.ToLower().Contains(word.ToLower())

            //    ));
            //}

            ////filtering by category Id
            //if (SearchCriterial.CategoryId!= default)
            //    queryable = queryable.Where(m => m.CategoryId == productParams.CategoryId);

        }




    }
}
