
using System;
using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public class PromoStepDataDto : EntityDto
    {
		public DateTime? ConfirmationDate { get; set; }

		public string Description { get; set; }


		 public int? PromoStepId { get; set; }

		 		 public long? PromoId { get; set; }

		 
    }
}