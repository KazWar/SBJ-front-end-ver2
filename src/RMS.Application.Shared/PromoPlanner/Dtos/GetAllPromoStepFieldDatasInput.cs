using Abp.Application.Services.Dto;
using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllPromoStepFieldDatasInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string ValueFilter { get; set; }


		 public string PromoStepFieldDescriptionFilter { get; set; }

		 		 public string PromoStepDataDescriptionFilter { get; set; }

		 
    }
}