using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class GetPromoStepForEditOutput
    {
		public CreateOrEditPromoStepDto PromoStep { get; set; }


    }
}