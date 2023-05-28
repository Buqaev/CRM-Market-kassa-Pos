using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4YolMarket.Models
{
    public class OrderDto
    {
        public List<Order> Orders { get; set; }
        public Order Order { get; set; }
    }
}