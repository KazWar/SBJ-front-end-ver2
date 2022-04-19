using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetAllCampaignCategoryTranslationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }


		 public string LocaleDescriptionFilter { get; set; }

		 		 public string CampaignCategoryNameFilter { get; set; }

		 
    }
}