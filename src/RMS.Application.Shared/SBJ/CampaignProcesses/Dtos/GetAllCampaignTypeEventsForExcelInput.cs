using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetAllCampaignTypeEventsForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxSortOrderFilter { get; set; }
		public int? MinSortOrderFilter { get; set; }


		 public string CampaignTypeNameFilter { get; set; }

		 		 public string ProcessEventNameFilter { get; set; }

		 
    }
}