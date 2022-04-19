using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetAllMessageComponentContentsForExcelInput
    {
		public string Filter { get; set; }

		public string ContentFilter { get; set; }


		 public string MessageComponentIsActiveFilter { get; set; }

		 		 public string CampaignTypeEventRegistrationStatusSortOrderFilter { get; set; }

		 
    }
}