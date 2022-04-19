using Abp.Application.Services.Dto;
using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllPromoCountriesForExcelInput
    {
		public string Filter { get; set; }


		 public string PromoPromocodeFilter { get; set; }

		 		 public string CountryCountryCodeFilter { get; set; }

		 
    }
}