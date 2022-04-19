using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetFormLocalesDto
    {
        //public long CampaignId { get; set; }
        //public long FormId { get; set; }
        //public string CampaignDescription { get; set; }
        //public string Version { get; set; }
        //public long CampaignCode { get; set; }
        //public List<GetActiveFormLocaleDto> LocaleSelect { get; set; }

        public long LocaleId { get; set; }
        public long CountryId { get; set; }
        public string LocaleDescription { get; set; }
        public string LanguageCode { get; set; }
    }
}
