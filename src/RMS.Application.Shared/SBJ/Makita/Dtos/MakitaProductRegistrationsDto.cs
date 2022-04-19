using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Makita.Dtos
{
    public class MakitaProductRegistrationsDto
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
