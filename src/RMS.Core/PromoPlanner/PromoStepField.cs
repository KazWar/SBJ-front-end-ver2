using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace RMS.PromoPlanner
{
    [Table("PromoStepField")]
    public class PromoStepField : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int FormFieldId { get; set; }

        public virtual string Description { get; set; }

        public virtual short Sequence { get; set; }

        public virtual int PromoStepId { get; set; }

        [ForeignKey("PromoStepId")]
        public PromoStep PromoStepFk { get; set; }
    }
}