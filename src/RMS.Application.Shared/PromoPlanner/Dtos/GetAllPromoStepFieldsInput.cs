using Abp.Application.Services.Dto;
using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllPromoStepFieldsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxFormFieldIdFilter { get; set; }
		public int? MinFormFieldIdFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public short? MaxSequenceFilter { get; set; }
		public short? MinSequenceFilter { get; set; }


		 public string PromoStepDescriptionFilter { get; set; }

		 
    }
}