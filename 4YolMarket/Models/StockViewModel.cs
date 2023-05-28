using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4YolMarket.Models
{
    public class StockViewModel
    {
        public int Id { get; set; }
        public decimal Say_ceki_ { get; set; }
        public decimal SalePrice { get; set; }
        public int ProductId { get; set; }

        public String Sekil { get; set; }
    }
}