using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignTranslationForEditOutput
    {
        public CreateOrEditCampaignTranslationDto CampaignTranslation { get; set; }

        public string CampaignName { get; set; }

        public string LocaleDescription { get; set; }

    }
}