using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.HandlingLines.Dtos
{
    public class GetAllHandlingLinesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public decimal? MaxMinimumPurchaseAmountFilter { get; set; }
        public decimal? MinMinimumPurchaseAmountFilter { get; set; }

        public decimal? MaxMaximumPurchaseAmountFilter { get; set; }
        public decimal? MinMaximumPurchaseAmountFilter { get; set; }

        public string CustomerCodeFilter { get; set; }

        public decimal? MaxAmountFilter { get; set; }
        public decimal? MinAmountFilter { get; set; }

        public int? FixedFilter { get; set; }

        public int? ActivationCodeFilter { get; set; }

        public int? MaxQuantityFilter { get; set; }
        public int? MinQuantityFilter { get; set; }

        public string CampaignTypeNameFilter { get; set; }

        public string ProductHandlingDescriptionFilter { get; set; }

    }
}