using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(BasketCart),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        
        public async Task<ActionResult<BasketCart>> GetCart(string username)
        {
            var cart= await basketRepository.GetCartAsync(username);
            if (cart ==null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BasketCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasket(string username ,[FromBody]BasketCart basket)
        {
            var updateCart = await basketRepository.UpdateCartAsync(username,basket);
            if (updateCart==null)
            {
                return BadRequest("Could not update the cart");

            }
            return Ok(updateCart);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void),(int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteCart(string username)
        {
            await basketRepository.DeleteCartAsync(username);
            return Ok("Cart Deleted");

        }
    }
}
