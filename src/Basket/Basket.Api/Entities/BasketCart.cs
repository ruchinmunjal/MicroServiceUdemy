using System.Collections.Generic;
using System.Linq;

namespace Basket.Api.Entities
{
    public class BasketCart
    {
        public string UserName { get; set; }

        public List<BasketCartItem> BasketItems { get; set; } = new List<BasketCartItem>();

        public BasketCart()
        {

        }
        public BasketCart(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                totalPrice = BasketItems.Sum(x => x.Qunatity * x.Price);
                return totalPrice;
            }
        }
    }
}
