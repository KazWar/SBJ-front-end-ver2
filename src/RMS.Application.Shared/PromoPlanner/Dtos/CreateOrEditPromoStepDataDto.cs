
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class CreateOrEditPromoStepDataDto : EntityDto<int?>
    {

		public DateTime? ConfirmationDate { get; set; }
		
		
		public string Description { get; set; }
		
		
		 public int? PromoStepId { get; set; }
		 
		 		 public long? PromoId { get; set; }
		 
		 
    }
}