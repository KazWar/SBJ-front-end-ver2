using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace RMS.PromoPlanner
{
    [Table("PromoStep")]
    [Audited]
    public class PromoStep : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual short Sequence { get; set; }

        public virtual string Description { get; set; }
    }
}