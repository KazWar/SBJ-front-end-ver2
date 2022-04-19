
using System;
using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public class PromoStepFieldDto : EntityDto
    {
		public int FormFieldId { get; set; }

		public string Description { get; set; }

		public short Sequence { get; set; }


		 public int? PromoStepId { get; set; }

		 
    }
}