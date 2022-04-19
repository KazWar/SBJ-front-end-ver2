using RMS.SBJ.CampaignProcesses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace RMS.SBJ.ProductGifts
{
    [Table("ProductGift")]
    public class ProductGift : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string ProductCode { get; set; }

        public virtual long GiftId { get; set; }

        public virtual string GiftName { get; set; }

        public virtual int TotalPoints { get; set; }

        public virtual long CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign CampaignFk { get; set; }

    }
}