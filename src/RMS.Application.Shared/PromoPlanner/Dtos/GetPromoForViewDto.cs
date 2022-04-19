using System.Collections.Generic;

namespace RMS.PromoPlanner.Dtos
{
    public class GetPromoForViewDto
    {
		public PromoDto Promo { get; set; }

		public string PromoScopeDescription { get; set;}
		public string Status { get; set; }
		public string CampaignTypeName { get; set;}

		public string ProductCategoryDescription { get; set;}

		public List<StepStatus> PromoProgress { get; set; }

		public IEnumerable<CustomProductForView> PromoProducts { get; set; }

		public IEnumerable<CustomPromoRetailerForView> PromoRetailers { get; set; }

		public IEnumerable<CustomPromoCountryForView> PromoCountries { get; set; }

		public IEnumerable<CustomPromoStepForView> PromoSteps { get; set; }
	}

    public class StepStatus
    {
        public bool Confirmed { get; set; }
        public string Description { get; set; }
    }

}