
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class CreateOrEditPromoStepFieldDto : EntityDto<int?>
    {

		public int FormFieldId { get; set; }
		
		
		public string Description { get; set; }
		
		
		public short Sequence { get; set; }
		
		
		 public int? PromoStepId { get; set; }
		 
		 
    }
}