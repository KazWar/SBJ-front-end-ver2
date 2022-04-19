using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignForFormDto
    {
        public long LocaleId { get; set; }
        public int CountryCode { get; set; }
        public string LocaleDescription { get; set; }
        public int LanguageCode { get; set; }
        public long FormId { get; set; }
        public long CampaignId { get; set; }
        public long CampaignCode { get; set; }
        public long FormLocaleId { get; set; }
        public string Version { get; set; }
        public string BannerImagePath { get; set; }
        public DateTime CampaignEndDate { get; set; }
    }
}
