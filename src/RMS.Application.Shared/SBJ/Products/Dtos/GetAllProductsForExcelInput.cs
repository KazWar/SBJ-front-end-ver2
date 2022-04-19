using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Products.Dtos
{
    public class GetAllProductsForExcelInput
    {
		public string Filter { get; set; }

		public string ProductCodeFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public string EanFilter { get; set; }


		 public string ProductCategoryDescriptionFilter { get; set; }

		 
    }
}