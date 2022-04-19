using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.UniqueCodes.Dtos
{
    public class UniqueCodeByCampaignDto : EntityDto<string>
    {
        public bool Used { get; set; }

        public long CampaignId { get; set; }

    }
}