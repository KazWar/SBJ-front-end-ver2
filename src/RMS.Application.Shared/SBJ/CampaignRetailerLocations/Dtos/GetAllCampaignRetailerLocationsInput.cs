using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CampaignRetailerLocations.Dtos
{
    public class GetAllCampaignRetailerLocationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string CampaignNameFilter { get; set; }

		 		 public string RetailerLocationNameFilter { get; set; }

		 
    }
}