using System;
using System.Collections;
using System.Collections.Generic;
using Order.Api.Controllers;

namespace Order.Api.Entities
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
            ShippingDetail = new ShippingDetail();
        }
        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime OrderDateTime { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        
        public ShippingDetail ShippingDetail { get; set; }
        
    }
}