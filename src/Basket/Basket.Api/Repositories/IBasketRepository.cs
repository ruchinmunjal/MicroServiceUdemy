using Basket.Api.Entities;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
        Task DeleteCartAsync(string username);
        Task<BasketCart> GetCartAsync(string userName);
        Task<BasketCart> UpdateCartAsync(string username, BasketCart basket);
    }
}
