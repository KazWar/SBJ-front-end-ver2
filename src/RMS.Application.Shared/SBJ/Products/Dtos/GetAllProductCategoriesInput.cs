using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Products.Dtos
{
    public class GetAllProductCategoriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public string POHandlingFilter { get; set; }

		public string POCashbackFilter { get; set; }

		public string ColorFilter { get; set; }



    }
}