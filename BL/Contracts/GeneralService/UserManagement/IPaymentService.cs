using BL.DTO.Entities;
using Domains.Entities;

namespace BL.Contracts.GeneralService.UserManagement
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId, Guid? deliveryId);
    }
}
