using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class GetPromoStepDataForEditOutput
    {
		public CreateOrEditPromoStepDataDto PromoStepData { get; set; }

		public string PromoStepDescription { get; set;}

		public string PromoPromocode { get; set;}


    }
}