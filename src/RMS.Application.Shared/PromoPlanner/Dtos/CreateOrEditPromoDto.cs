using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace RMS.PromoPlanner.Dtos
{
    public class CreateOrEditPromoDto : EntityDto<long?>
    {
        [Required]
        public string Promocode { get; set; }
        public string Description { get; set; }
        public DateTime PromoStart { get; set; }
        public DateTime PromoEnd { get; set; }
        public DateTime? CloseDate { get; set; }
        public string CustomerCode { get; set; }
        public string Comments { get; set; }
        public long PromoScopeId { get; set; }
        public long CampaignTypeId { get; set; }
        public long ProductCategoryId { get; set; }
        public IReadOnlyList<CustomPromoStepForView> PromoSteps { get; set; }
    }
}