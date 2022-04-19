using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CampaignCountryDto : EntityDto<long>
    {

        public long CampaignId { get; set; }

        public long CountryId { get; set; }

    }
}