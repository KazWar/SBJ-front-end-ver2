using Abp.Application.Services.Dto;
using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllProductCategoryYearPosInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxYearFilter { get; set; }
		public int? MinYearFilter { get; set; }

		public string PONumberHandlingFilter { get; set; }

		public string PONumberCashbackFilter { get; set; }


		 public string ProductCategoryCodeFilter { get; set; }

		 
    }
}