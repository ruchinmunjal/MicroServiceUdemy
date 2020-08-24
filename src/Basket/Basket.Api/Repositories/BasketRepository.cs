using Basket.Api.Data.Interfaces;
using Basket.Api.Entities;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public class BasketRepository:IBasketRepository
    {

        public readonly  IBasketContext  _dbContext;

        public BasketRepository(IBasketContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BasketCart> GetCartAsync(string userName)
        {
            var basket =await _dbContext.Redis.StringGetAsync(userName);
            if (basket.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateCartAsync(string username,BasketCart basket)
        {
            var updated = await _dbContext.Redis.StringSetAsync(username,JsonConvert.SerializeObject(basket));
            if (!updated)
            {
                return null;
            }
            return await GetCartAsync(username);

        }

        public async Task DeleteCartAsync(string username)
        {
            _ = await _dbContext.Redis.KeyDeleteAsync(username);

        }

    }
}
