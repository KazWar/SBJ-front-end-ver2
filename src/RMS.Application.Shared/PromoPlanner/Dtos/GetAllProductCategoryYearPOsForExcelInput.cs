using Abp.Application.Services.Dto;
using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllProductCategoryYearPosForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxYearFilter { get; set; }
		public int? MinYearFilter { get; set; }

		public string PoNumberHandlingFilter { get; set; }

		public string PoNumberCashbackFilter { get; set; }


		 public string ProductCategoryCodeFilter { get; set; }

		 
    }
}