using System;

namespace Order.Api.Entities
{
    public class ShippingDetail
    {
        public int ShippingDetailId { get; set; }
        public Guid OrderId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
    }
}