using Abp.Application.Services.Dto;
using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllPromosForExcelInput
    {
		public string Filter { get; set; }

		public string PromocodeFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public DateTime? MaxPromoStartFilter { get; set; }
		public DateTime? MinPromoStartFilter { get; set; }

		public DateTime? MaxPromoEndFilter { get; set; }
		public DateTime? MinPromoEndFilter { get; set; }


		 public string PromoScopeDescriptionFilter { get; set; }

		 		 public string CampaignTypeNameFilter { get; set; }

		 		 public string ProductCategoryDescriptionFilter { get; set; }

		 
    }
}