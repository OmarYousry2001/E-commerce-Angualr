using AutoMapper;
using BL.DTO.Entities;
using Domains.Entities;
using Domains.Entities.Product;

namespace BL.Mapper
{
    // Main mapping profile file (MappingProfile.cs)
    public partial class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //CreateMap<TbAuthor, AuthorDto>().ReverseMap();
            //CreateMap<TbRefreshToken, RefreshTokenDto>().ReverseMap();
            //CreateMap<ApplicationUser, UserRegistrationDto>().ReverseMap();
            //CreateMap<VwBook, BookViewDto>().ReverseMap();
            //CreateMap<VwUserProfile, UserProfileViewDto>().ReverseMap();

            #region Product
            CreateMap<Product, GetProductDTO>()
            .ForMember(des => des.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id)).ReverseMap();

            CreateMap<Photo, PhotoDTO>()
            .ForMember(des => des.ImageName, opt => opt.MapFrom(src => src.ImagePath)).ReverseMap();

            CreateMap<ProductDTO, Product>().ReverseMap();
            #endregion
            #region Category 
            CreateMap<Category, CategoryDTO>().ReverseMap();
            #endregion

            #region Address
            CreateMap<Address, ShipAddressDTO>().ReverseMap(); 
            #endregion


        }
    }
}
