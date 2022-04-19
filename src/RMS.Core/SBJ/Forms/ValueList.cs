using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;

namespace RMS.SBJ.Forms
{
    [Table("ValueList")]
    [Audited]
    public class ValueList : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual string ListValueApiCall { get; set; }

        public virtual ICollection<FormFieldValueList> FormFieldValueLists { get; set; }
        public virtual ICollection<ListValue> ListValues { get; set; }
    }
}