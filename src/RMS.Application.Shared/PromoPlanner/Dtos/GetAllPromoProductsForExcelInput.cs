using Abp.Application.Services.Dto;
using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllPromoProductsForExcelInput
    {
		public string Filter { get; set; }


		 public string PromoPromocodeFilter { get; set; }

		 		 public string ProductCtnFilter { get; set; }

		 
    }
}