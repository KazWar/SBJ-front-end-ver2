using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignStepForEditOutput
    {
		public CreateOrEditCampaignStepDto CampaignStep { get; set; }

		public string CampaignTypeName { get; set;}


    }
}