using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignCountryForEditOutput
    {
        public CreateOrEditCampaignCountryDto CampaignCountry { get; set; }

        public string CampaignName { get; set; }

        public string CountryDescription { get; set; }

    }
}