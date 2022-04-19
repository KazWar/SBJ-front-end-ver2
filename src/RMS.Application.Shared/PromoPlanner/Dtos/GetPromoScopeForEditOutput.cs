using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class GetPromoScopeForEditOutput
    {
		public CreateOrEditPromoScopeDto PromoScope { get; set; }


    }
}