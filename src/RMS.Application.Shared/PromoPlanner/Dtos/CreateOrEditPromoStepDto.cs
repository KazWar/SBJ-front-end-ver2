
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class CreateOrEditPromoStepDto : EntityDto<int?>
    {

		public short Sequence { get; set; }
		
		
		public string Description { get; set; }
		
		

    }
}