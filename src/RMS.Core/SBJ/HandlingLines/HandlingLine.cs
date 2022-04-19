using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.HandlingLineLocales;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.HandlingLineRetailers;
using RMS.SBJ.ProductHandlings;
using RMS.SBJ.PurchaseRegistrations;

namespace RMS.SBJ.HandlingLines
{
    [Table("HandlingLine")]
    public class HandlingLine : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual decimal? MinimumPurchaseAmount { get; set; }

        public virtual decimal? MaximumPurchaseAmount { get; set; }

        public virtual string CustomerCode { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual bool Fixed { get; set; }

        public virtual bool Percentage { get; set; }

        public virtual bool ActivationCode { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual string PremiumDescription { get; set; }

        public virtual long CampaignTypeId { get; set; }

        public virtual long ProductHandlingId { get; set; }

        [ForeignKey("CampaignTypeId")]
        public CampaignType CampaignTypeFk { get; set; }

        [ForeignKey("ProductHandlingId")]
        public ProductHandling ProductHandlingFk { get; set; }

        public virtual ICollection<HandlingLineLocale> HandlingLineLocales { get; set; }
        public virtual ICollection<HandlingLineProduct> HandlingLineProducts { get; set; }
        public virtual ICollection<HandlingLineRetailer> HandlingLineRetailers { get; set; }
        public virtual ICollection<PurchaseRegistration> PurchaseRegistrations { get; set; }
    }
}