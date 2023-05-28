using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4YolMarket.Models
{
    public class PosDto
    {
        public List<Product> Products { get; set; }

        public List<Stock> Stocks { get; set; }
        public Stock Stock { get; set; }

        public List<OrderItem> OrderItemss { get; set; }
        public OrderItem OrderItem { get; set; }
        public Order Order { get; set; }

        public List<Order> Orders { get; set; }
        public Customer Customer { get; set; }


    }
}