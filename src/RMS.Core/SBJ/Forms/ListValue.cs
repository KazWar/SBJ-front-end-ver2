using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;

namespace RMS.SBJ.Forms
{
    [Table("ListValue")]
    [Audited]
    public class ListValue : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string KeyValue { get; set; }

        public virtual string Description { get; set; }

        public virtual int SortOrder { get; set; }

        public virtual long ValueListId { get; set; }

        [ForeignKey("ValueListId")]
        public ValueList ValueListFk { get; set; }

        public virtual ICollection<ListValueTranslation> ListValueTranslations { get; set; }
    }
}