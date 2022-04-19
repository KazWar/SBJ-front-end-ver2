using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CampaignTranslationDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long CampaignId { get; set; }

        public long LocaleId { get; set; }

    }
}