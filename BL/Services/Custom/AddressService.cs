using BL.Contracts.IMapper;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using BL.GenericResponse;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using Domains.Entities;

namespace BL.Services
{
    public class AddressService : BaseService<Address, ShipAddressDTO>, IAddressService
    {
        private readonly ITableRepository<Address> _addressRepository;
        public AddressService(ITableRepository<Address> addressRepository, IBaseMapper mapper) : base(addressRepository, mapper)
        {
            _addressRepository =addressRepository;
            _mapper = mapper;
        }
        public  async Task<Response<ShipAddressDTO>> FindAddressByUserIdAsync(string userId)
        {
            var entity = await _addressRepository.FindAsync(x => x.ApplicationUserId == userId && x.CurrentState==1);
            if (entity == null) return NotFound<ShipAddressDTO>();
            var dto = _mapper.MapModel<Address, ShipAddressDTO>(entity);
            return Success(dto);
        }
        public override async Task<Response<bool>> SaveAsync(ShipAddressDTO dto, Guid userId)
        {
            var entity = _mapper.MapModel<ShipAddressDTO, Address>(dto);
            entity.ApplicationUserId = userId.ToString();
            var isSaved = await _addressRepository.SaveAsync(entity, userId);
            if (isSaved) return Success(true);
            else return BadRequest<bool>();

        }

    }
}
