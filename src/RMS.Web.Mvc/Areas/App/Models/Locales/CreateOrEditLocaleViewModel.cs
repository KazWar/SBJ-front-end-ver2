using RMS.SBJ.CodeTypeTables.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.Locales
{
    public class CreateOrEditLocaleModalViewModel
    {
       public CreateOrEditLocaleDto Locale { get; set; }

	   		public string CountryCountryCode { get; set;}


       public List<LocaleCountryLookupTableDto> LocaleCountryList { get; set;}


	   public bool IsEditMode => Locale.Id.HasValue;
    }
}