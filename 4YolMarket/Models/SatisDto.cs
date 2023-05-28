using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4YolMarket.Models
{
    public class SatisDto
    {
        public List<OrderItem> OrderItems { get; set; }
        public List<Order> Orders { get; set; }

        public Order Order { get; set; }

    }
}