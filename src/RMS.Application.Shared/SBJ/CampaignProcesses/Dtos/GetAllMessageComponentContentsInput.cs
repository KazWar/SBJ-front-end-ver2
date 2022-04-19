using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetAllMessageComponentContentsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string ContentFilter { get; set; }


		 public string MessageComponentIsActiveFilter { get; set; }

		 		 public string CampaignTypeEventRegistrationStatusSortOrderFilter { get; set; }

		 
    }
}