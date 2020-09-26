using System;
using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ServiceBus.Producer;
using static ServiceBus.Common.ServiceBusConstants;
using BasketCheckoutEvent = ServiceBus.Events.BasketCheckoutEvent;

namespace Basket.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly BasketCheckoutProducer _eventProducer;
        
        public BasketController(IBasketRepository basketRepository,IMapper mapper, BasketCheckoutProducer eventProducer)
        {
            this._basketRepository = basketRepository;
            _mapper = mapper;
            _eventProducer = eventProducer;
            
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(BasketCart),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        
        public async Task<ActionResult<BasketCart>> GetCart(string username)
        {
            var cart= await _basketRepository.GetCartAsync(username);
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
            var updateCart = await _basketRepository.UpdateCartAsync(username,basket);
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
            await _basketRepository.DeleteCartAsync(username);
            return Ok("Cart Deleted");

        }
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckoutBasket([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _basketRepository.GetCartAsync(basketCheckout.UserName);
            if (basket==null)
            {
                return BadRequest("Couldn't find the cart");
                
            }
            
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.TotalPrice;

            try
            {
                _eventProducer.PublishBasketCheckout(BasketCheckoutServiceQueue,eventMessage);
            }
            catch (Exception e)
            {
                return BadRequest("Unable to process the request");
            }
            await _basketRepository.DeleteCartAsync(basketCheckout.UserName);
            return Accepted("Order Placed");
        }
        
    }
}
