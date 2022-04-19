using RMS.PromoPlanner.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.PromoCountries
{
    public class CreateOrEditPromoCountryModalViewModel
    {
       public CreateOrEditPromoCountryDto PromoCountry { get; set; }

	   		public string PromoPromocode { get; set;}

		public string CountryCountryCode { get; set;}


       
	   public bool IsEditMode => PromoCountry.Id.HasValue;
    }
}