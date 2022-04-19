using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.ProductHandlings.Dtos
{
    
    public class GetProductHandlingForApiDto 
    {
        public long CampaignId { get; set; } 
        public string Description {get; set;}
        public List<GetCampaignRetailersDto> Retailers { get; set; }
        public List<GetHandlingLinesDto> HandlingLines { get; set; }
        public List<GetProductHandelingDto> Products { get; set; }
    }
}
