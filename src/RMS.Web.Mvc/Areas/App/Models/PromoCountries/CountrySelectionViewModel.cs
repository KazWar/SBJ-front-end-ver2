using System.Collections.Generic;
using RMS.PromoPlanner.Dtos;

namespace RMS.Web.Areas.App.Models.PromoCountries
{
    public sealed class CountrySelectionViewModel
    {
        public bool IsEditMode { get; set; }
        public IEnumerable<CustomPromoCountryForView> AvailableCountries { get; set; }
        public IEnumerable<CustomPromoCountryForView> SelectedCountries { get; set; }
    }
}
