using Abp.Application.Services.Dto;
using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllPromoStepDatasInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxConfirmationDateFilter { get; set; }
		public DateTime? MinConfirmationDateFilter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string PromoStepDescriptionFilter { get; set; }

		 		 public string PromoPromocodeFilter { get; set; }

		 
    }
}