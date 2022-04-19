
using System;
using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public class PromoStepFieldDataDto : EntityDto
    {
		public string Value { get; set; }


		 public int? PromoStepFieldId { get; set; }

		 		 public int? PromoStepDataId { get; set; }

		 
    }
}