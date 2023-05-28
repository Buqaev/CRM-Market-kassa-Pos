using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _4YolMarket.Models
{
    public class AlisDto
    {
        public List<Purchase> Purchases { get; set; }
        public Purchase Purchase { get; set; }
        public List<PurchaseItem> PurchaseItems { get; set; }
        public List<Firma> Firmas { get; set; }


        //[Display(Name = "Alinma Tarixi")]
        [DataType(DataType.Date)]
        public DateTime? Tim { get; set; }


        [Display(Name = "Qaytarma Tarixi")]
        [DataType(DataType.Date)]
        public DateTime? Tim2 { get; set; }


    }
}