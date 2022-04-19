using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace RMS.PromoPlanner
{
    [Table("PromoStepFieldData")]
    public class PromoStepFieldData : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Value { get; set; }

        public virtual int PromoStepDataId { get; set; }

        public virtual int PromoStepFieldId { get; set; }

        [ForeignKey("PromoStepDataId")]
        public PromoStepData PromoStepDataFk { get; set; }

        [ForeignKey("PromoStepFieldId")]
        public PromoStepField PromoStepFieldFk { get; set; }
    }
}