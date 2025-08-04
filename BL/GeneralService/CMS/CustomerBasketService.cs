using BL.Contracts.GeneralService.CMS;
using BL.GenericResponse;
using Domains.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace BL.GeneralService.CMS
{
    public class CustomerBasketService : ResponseHandler, ICustomerBasketService
    {
        private readonly IDatabase _database;
        public CustomerBasketService(IConnectionMultiplexer radius)
        {
            _database = radius.GetDatabase();
        }
        public async Task<Response<bool>> DeleteBasketAsync(string id)
        {
            var result = await _database.KeyDeleteAsync(id);
            return (result)? Success(true) : NotFound<bool>();
      
        }

        public async Task<Response<CustomerBasket>> GetBasketAsync(string id)
        {
            var result = await _database.StringGetAsync(id);
            if (!string.IsNullOrEmpty(result))
            {
                return   Success(JsonSerializer.Deserialize<CustomerBasket>(result));
            }
            return NotFound<CustomerBasket>(); 
        }

        public async Task<Response<CustomerBasket>> UpdateBasketAsync(CustomerBasket basket)
        {
            var _basket = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(3));
            if (_basket)
            {
                return await GetBasketAsync(basket.Id);
            }
            return NotFound<CustomerBasket>();

        }
    }
}
