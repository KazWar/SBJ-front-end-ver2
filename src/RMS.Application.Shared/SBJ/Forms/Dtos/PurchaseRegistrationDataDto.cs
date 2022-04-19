using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Forms.Dtos
{
    public class PurchaseRegistrationDataDto
    {
        public List<GetFormHandlingLineDto> ProductPremium { get; set; }
        public string HandlingLineQuantity { get; set; }
        public string PurchaseDate { get; set; }
        public string TotalAmount { get; set; }
        public string SerialNumber { get; set; }
        public List<string> InvoiceImage { get; set; }
        public List<string> SerialImage { get; set; }
        public string StorePurchased { get; set; }
    }
}
