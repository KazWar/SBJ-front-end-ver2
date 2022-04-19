using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignCampaignTypeForEditOutput
    {
		public CreateOrEditCampaignCampaignTypeDto CampaignCampaignType { get; set; }

		public string CampaignDescription { get; set;}

		public string CampaignTypeName { get; set;}


    }
}