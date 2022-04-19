using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetAllCampaignCategoriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public int IsActiveFilter { get; set; }

		public int? MaxSortOrderFilter { get; set; }
		public int? MinSortOrderFilter { get; set; }



    }
}