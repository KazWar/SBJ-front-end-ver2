using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Forms.Dtos
{
    public class DefaultValuesPurchaseRegistrationDto
    {
        public List<GetFormFieldValueListDto> ProductPremium { get; set; }
        public string Quantity { get; set; }
        public string PurchaseDate { get; set; }
        public string TotalAmount { get; set; }
        public string SerialNumber { get; set; }
        public List<DropdownListDto> StorePurchased { get; set; }
    }
}
