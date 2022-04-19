using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignMessageForEditOutput
    {
		public CreateOrEditCampaignMessageDto CampaignMessage { get; set; }

		public string CampaignName { get; set;}

		public string MessageVersion { get; set;}


    }
}