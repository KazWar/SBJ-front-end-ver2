using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingLines.Dtos
{
    public class CreateOrEditHandlingLineDto : EntityDto<long?>
    {

        public decimal? MinimumPurchaseAmount { get; set; }

        public decimal? MaximumPurchaseAmount { get; set; }

        public string CustomerCode { get; set; }

        public decimal? Amount { get; set; }

        public bool Fixed { get; set; }

        public bool ActivationCode { get; set; }

        public int? Quantity { get; set; }

        public long CampaignTypeId { get; set; }

        public long ProductHandlingId { get; set; }

    }
}