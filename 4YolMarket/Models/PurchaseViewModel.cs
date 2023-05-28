using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4YolMarket.Models
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal AlisQiymeti { get; set; }
        public DateTime BitmeVaxti { get; set; }
        public DateTime IsdehsalVaxti { get; set; }
        public decimal Say { get; set; }

    }
}