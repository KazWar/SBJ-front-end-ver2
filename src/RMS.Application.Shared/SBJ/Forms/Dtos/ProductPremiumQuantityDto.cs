using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Forms.Dtos
{
    public class ProductPremiumQuantityDto
    {
        public List<GetFormHandlingLineDto> ProductPremium { get; set; }
        public string HandlingLineQuantity { get; set; }
    }
}
