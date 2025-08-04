
using BL.GenericResponse;
using Domains.Entities;

namespace BL.Contracts.GeneralService.CMS
{
    public  interface ICustomerBasketService
    {
        Task<Response<CustomerBasket>> GetBasketAsync(string id);
        Task<Response<CustomerBasket>> UpdateBasketAsync(CustomerBasket basket);
        Task<Response<bool>> DeleteBasketAsync(string id);
    }
}
