
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class CreateOrEditPromoStepFieldDataDto : EntityDto<int?>
    {

		public string Value { get; set; }
		
		
		 public int? PromoStepFieldId { get; set; }
		 
		 		 public int? PromoStepDataId { get; set; }
		 
		 
    }
}