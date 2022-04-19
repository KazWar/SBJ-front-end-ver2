using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class GetPromoStepFieldForEditOutput
    {
		public CreateOrEditPromoStepFieldDto PromoStepField { get; set; }

		public string PromoStepDescription { get; set;}


    }
}