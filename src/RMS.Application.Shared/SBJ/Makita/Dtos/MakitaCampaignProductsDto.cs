using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Makita.Dtos
{
    public class MakitaCampaignProductsDto
    {
        public string Product_id { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public List<MakitaCampaignProductGiftsDto> Gift_choices { get; set; }
    }
}
