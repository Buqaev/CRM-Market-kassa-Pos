using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4YolMarket.Models
{
    public class DashboardDto
    {
        public List<Product> Products { get; set; }
        public Cash cash { get; set; }
        public List<Stock> Stocks { get; set; }

    }
}