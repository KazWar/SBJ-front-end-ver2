using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignFormForEditOutput
    {
		public CreateOrEditCampaignFormDto CampaignForm { get; set; }

		public string CampaignName { get; set;}

		public string FormVersion { get; set;}


    }
}