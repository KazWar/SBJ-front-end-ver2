using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.CodeTypeTables;

namespace RMS.SBJ.Forms
{
    [Table("ListValueTranslation")]
    [Audited]
    public class ListValueTranslation : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string KeyValue { get; set; }

        public virtual string Description { get; set; }

        public virtual long ListValueId { get; set; }

        public virtual long LocaleId { get; set; }

        [ForeignKey("ListValueId")]
        public ListValue ListValueFk { get; set; }

        [ForeignKey("LocaleId")]
        public Locale LocaleFk { get; set; }
    }
}