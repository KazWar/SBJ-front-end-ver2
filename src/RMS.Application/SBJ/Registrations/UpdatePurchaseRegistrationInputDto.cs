using RMS.SBJ.Forms.Dtos;
using System;
using System.Collections.Generic;

namespace RMS.SBJ.Registrations
{
    public sealed class UpdatePurchaseRegistrationInputDto
    {
        public long PurchaseRegistrationId { get; set; }

        public uint Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime PurchaseDate { get; set; }

        public IEnumerable<string> InvoiceImagePath { get; set; }

        public DropdownListDto StorePicker { get; set; }

        public string StorePurchased { get; set; }

        public GetFormHandlingLineDto ProductPremium { get; set; }

        public string Product { get; set; }

        public Dictionary<string, string> CustomFields { get; set; }
    }
}
