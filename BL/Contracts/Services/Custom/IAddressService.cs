using BL.Contracts.Services.Generic;
using BL.DTO.Entities;
using BL.GenericResponse;
using Domains.Entities;

namespace BL.Contracts.Services.Custom
{
    public interface IAddressService : IBaseService<Address, ShipAddressDTO>
    {
        public Task<Response<ShipAddressDTO>> FindAddressByUserIdAsync(string userId);

    }
}
