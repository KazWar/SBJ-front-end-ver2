using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.ProductHandlings.Dtos
{
    public class GetAllProductHandlingsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string CampaignNameFilter { get; set; }

		 
    }
}