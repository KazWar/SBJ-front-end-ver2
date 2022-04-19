using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetAllCampaignCampaignTypesForExcelInput
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string CampaignDescriptionFilter { get; set; }

		 		 public string CampaignTypeNameFilter { get; set; }

		 
    }
}