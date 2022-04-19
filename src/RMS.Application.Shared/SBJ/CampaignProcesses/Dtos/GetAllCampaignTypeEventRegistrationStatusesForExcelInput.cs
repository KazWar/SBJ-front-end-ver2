using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetAllCampaignTypeEventRegistrationStatusesForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxSortOrderFilter { get; set; }
		public int? MinSortOrderFilter { get; set; }


		 public string CampaignTypeEventSortOrderFilter { get; set; }

		 		 public string RegistrationStatusDescriptionFilter { get; set; }

		 
    }
}