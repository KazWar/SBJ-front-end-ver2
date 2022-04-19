using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CreateOrEditCampaignCountryDto : EntityDto<long?>
    {

        public long CampaignId { get; set; }

        public long CountryId { get; set; }

    }
}