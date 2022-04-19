using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace RMS.PromoPlanner.Dtos
{
    public class GetPromoForEditOutput
    {
		public CreateOrEditPromoDto Promo { get; set; }

		public string PromoScopeDescription { get; set;}

		public string CampaignTypeName { get; set;}

		public string ProductCategoryDescription { get; set; }

        public string SelectedCountryIds { get; set; }

        public IEnumerable<CustomPromoCountryForView> AvailableCountries { get; set; }

        public IEnumerable<CustomProductForView> PromoProducts { get; set; }

        public IEnumerable<CustomPromoRetailerForView> PromoRetailers { get; set; }

        public IEnumerable<CustomPromoCountryForView> PromoCountries { get; set; }

        public IEnumerable<CustomPromoStepForView> PromoSteps { get; set; }
    }
}