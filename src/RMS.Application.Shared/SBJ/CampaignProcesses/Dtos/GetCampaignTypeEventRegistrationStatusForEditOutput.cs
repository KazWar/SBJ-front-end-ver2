using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignTypeEventRegistrationStatusForEditOutput
    {
		public CreateOrEditCampaignTypeEventRegistrationStatusDto CampaignTypeEventRegistrationStatus { get; set; }

		public string CampaignTypeEventSortOrder { get; set;}

		public string RegistrationStatusDescription { get; set;}


    }
}