using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace RMS.PromoPlanner
{
    [Table("PromoStepData")]
    [Audited]
    public class PromoStepData : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime? ConfirmationDate { get; set; }

        public virtual string Description { get; set; }

        public virtual long PromoId { get; set; }

        public virtual int PromoStepId { get; set; }

        [ForeignKey("PromoId")]
        public Promo PromoFk { get; set; }

        [ForeignKey("PromoStepId")]
        public PromoStep PromoStepFk { get; set; }
    }
}