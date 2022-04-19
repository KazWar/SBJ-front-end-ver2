using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Models.Makita
{
    public class MakitaProductRegistrationModel
    {
        public string Gift_id { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string PurchaseDate { get; set; }
        public string StorePurchased { get; set; }
        public string InvoiceImagePath { get; set; }
        public string SerialNumberImagePath { get; set; }
    }
}
