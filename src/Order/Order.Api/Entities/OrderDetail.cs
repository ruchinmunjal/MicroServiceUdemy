using System;

namespace Order.Api.Entities
{
    public class OrderDetail
    {
        public Guid OrderId { get; set; }

        public int OrderDetailId { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        
    }
}