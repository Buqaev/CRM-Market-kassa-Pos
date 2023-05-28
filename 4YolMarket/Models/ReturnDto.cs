using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4YolMarket.Models
{
    public class ReturnDto
    {
        public List<Stock> Stocks { get; set; }
        public List<Stock> Stocks2 { get; set; }

        public Return Return { get; set; }
        public Stock Stock { get; set; }
    }
}