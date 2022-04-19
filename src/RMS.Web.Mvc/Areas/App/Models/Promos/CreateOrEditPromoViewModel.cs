using RMS.PromoPlanner.Dtos;
using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.Promos
{
    public class CreateOrEditPromoViewModel
    {
        public CreateOrEditPromoDto Promo { get; set; }

        public string PromoScopeDescription { get; set; }

        public string CampaignTypeName { get; set; }

        public string ProductCategoryDescription { get; set; }

        public bool IsEditMode => Promo.Id.HasValue;

        public string SelectedCountryIds { get; set; }

        public IEnumerable<CustomPromoCountryForView> AvailableCountries { get; set; }

        public IEnumerable<CustomProductForView> PromoProducts { get; set; }

        public IEnumerable<CustomPromoRetailerForView> PromoRetailers { get; set; }

        public IEnumerable<CustomPromoCountryForView> PromoCountries { get; set; }

        public IEnumerable<CustomPromoStepForView> PromoSteps { get; set; }
    }
}