using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class GetPromoStepFieldDataForEditOutput
    {
		public CreateOrEditPromoStepFieldDataDto PromoStepFieldData { get; set; }

		public string PromoStepFieldDescription { get; set;}

		public string PromoStepDataDescription { get; set;}


    }
}