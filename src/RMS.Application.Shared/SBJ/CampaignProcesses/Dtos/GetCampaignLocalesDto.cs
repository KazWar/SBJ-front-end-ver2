using System;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignLocalesDto
    {
        public long CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string CampaignDescription { get; set; }
        public string Version { get; set; }
        public long CampaignCode { get; set; }     
        public string LocaleDescription { get; set; }
        public string LocaleName { get; set; }
        public string CampaignThumbnail { get; set; }
        public long LocaleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
