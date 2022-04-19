using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.ProductHandlings.Dtos
{
    public class GetCampaignRetailersDto
    {
        public long RetailId { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        
        

    }
}
 