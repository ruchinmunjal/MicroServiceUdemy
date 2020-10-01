using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Api.DbProvider;

namespace Order.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class Order : ControllerBase
    {
        private readonly IMediator _mediatr;

        public Order(IMediator mediatr)
        {
            _mediatr = mediatr;
        }


        [HttpGet("{customerId}")]
        public async Task<ActionResult> GetOrders(string customerId)
        {
            var response= await _mediatr.Send(new GetOrderForCustomerQuery {CustomerId = customerId});
            return Ok(response);
        }
    }

    public class GetOrderForCustomerQuery:IRequest<IEnumerable<GetOrderForCustomerQueryResult>>
    {
        public string CustomerId { get; set; }
    }

    public class GetOrderForCustomerQueryResult
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public ShippingDetails ShippingDetails { get; set; }
        
    }
    
    public class GetOrderForCustomersHandler:IRequestHandler<GetOrderForCustomerQuery,IEnumerable<GetOrderForCustomerQueryResult>>
    {

        private readonly OrderDbContext _dbContext;

        public GetOrderForCustomersHandler(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<GetOrderForCustomerQueryResult>> Handle(GetOrderForCustomerQuery request, CancellationToken cancellationToken)
        {
            Guid.TryParse(request.CustomerId, out var customerGuidId);
            var orders = await _dbContext.Orders.AsNoTracking()
                .Where(x=>x.CustomerId== customerGuidId)
                .Select(o => new GetOrderForCustomerQueryResult()
            {
                OrderDate = o.OrderDateTime,
                OrderId = o.OrderId.ToString(),
                ShippingDetails = new ShippingDetails()
                {
                    Country = o.ShippingDetail.Country,
                    PostCode = o.ShippingDetail.PostCode,
                    ShippingAddress = o.ShippingDetail.AddressLine1 + " " + o.ShippingDetail.AddressLine2
                }
            }).ToListAsync(cancellationToken);
            return orders;
        }
    }

    public class ShippingDetails
    {
        public string ShippingAddress { get; set; }

        public string PostCode { get; set; }

        public string Country { get; set; }
    }
    
}