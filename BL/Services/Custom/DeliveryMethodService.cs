using BL.Contracts.IMapper;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using Domains.Order;

namespace BL.Services
{
    public class DeliveryMethodService : BaseService<DeliveryMethod, DeliveryMethodDTO>, IDeliveryMethodService
    {
        private readonly ITableRepository<DeliveryMethod> _deliveryMethodTableRepository;
        public DeliveryMethodService(ITableRepository<DeliveryMethod> deliveryMethodTableRepository, IBaseMapper mapper) : base(deliveryMethodTableRepository, mapper)
        {
            _deliveryMethodTableRepository = deliveryMethodTableRepository;
            _mapper = mapper;
        }

    }
}
