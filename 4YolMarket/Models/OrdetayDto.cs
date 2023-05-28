using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4YolMarket.Models
{
    public class OrdetayDto
    {
        public List<OrderItem> orderItems { get; set; }
        public Order order { get; set; }
    }
}