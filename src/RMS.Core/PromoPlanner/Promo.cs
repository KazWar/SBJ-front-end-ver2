using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace RMS.PromoPlanner
{
    [Table("Promo")]
    [Audited]
    public class Promo : Entity<long> , IMayHaveTenant
    {
		public int? TenantId { get; set; }

		[Required]
		public virtual string Promocode { get; set; }

		public virtual string Description { get; set; }

		public virtual DateTime PromoStart { get; set; }

		public virtual DateTime PromoEnd { get; set; }

		public virtual DateTime CloseDate { get; set; }

		public string CustomerCode { get; set; }

		public string Comments { get; set; }

		public virtual long PromoScopeId { get; set; }

		public virtual long CampaignTypeId { get; set; }

		public virtual long ProductCategoryId { get; set; }

        [ForeignKey("PromoScopeId")]
		public PromoScope PromoScopeFk { get; set; }

        [ForeignKey("CampaignTypeId")]
		public CampaignType CampaignTypeFk { get; set; }

        [ForeignKey("ProductCategoryId")]
		public ProductCategory ProductCategoryFk { get; set; }
    }
}