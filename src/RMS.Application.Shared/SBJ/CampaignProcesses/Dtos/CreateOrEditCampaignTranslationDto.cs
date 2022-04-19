using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CreateOrEditCampaignTranslationDto : EntityDto<long?>
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public long CampaignId { get; set; }

        public long LocaleId { get; set; }

    }
}