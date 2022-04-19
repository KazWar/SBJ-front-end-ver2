using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllPromosInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string PromocodeFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public DateTime? MaxPromoStartFilter { get; set; }
        public DateTime? MinPromoStartFilter { get; set; }

        public DateTime? MaxPromoEndFilter { get; set; }
        public DateTime? MinPromoEndFilter { get; set; }

        public string PromoScopeFilter { get; set; }

        public string CampaignTypeFilter { get; set; }

        public string ProductCategoryFilter { get; set; }

        public string RetailerFilter { get; set; }

    }
}