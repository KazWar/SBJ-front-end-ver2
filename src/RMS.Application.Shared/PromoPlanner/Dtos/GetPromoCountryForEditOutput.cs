using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class GetPromoCountryForEditOutput
    {
		public CreateOrEditPromoCountryDto PromoCountry { get; set; }

		public string PromoPromocode { get; set;}

		public string CountryCountryCode { get; set;}


    }
}