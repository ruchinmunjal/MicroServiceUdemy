using AutoMapper;
using Basket.Api.Entities;
using ServiceBus.Events;

namespace Basket.Api.MapperConfigs
{
    public class BasketCheckoutProfile:Profile
    {
        public BasketCheckoutProfile()
        {
            CreateMap<BasketCheckoutEvent, BasketCheckout>().ReverseMap();
        }
    }
}