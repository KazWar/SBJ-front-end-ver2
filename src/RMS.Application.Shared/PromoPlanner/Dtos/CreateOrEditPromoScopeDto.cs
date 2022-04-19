
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class CreateOrEditPromoScopeDto : EntityDto<long?>
    {

		public string Description { get; set; }
		
		

    }
}