using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetPurchaseRegistrationForFormsDto
    {

        public string InvoiceImage { get; set; }
        public DateTime PurchaseDate { get; set; }
        public long RetailerLocationId { get; set; }
        public long ProductId { get; set; }
        public decimal TotalAmount { get; set; }
        public long Quantity { get; set; }
        public long RegistrationId { get; set; }


    }
}
