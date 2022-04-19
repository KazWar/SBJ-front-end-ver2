using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace RMS.SBJ.Forms
{
    [Table("FormBlockField")]
    [Audited]
    public class FormBlockField : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int SortOrder { get; set; }

        public virtual long? FormFieldId { get; set; }

        public virtual long? FormBlockId { get; set; }

        [ForeignKey("FormFieldId")]
        public FormField FormFieldFk { get; set; }

        [ForeignKey("FormBlockId")]
        public FormBlock FormBlockFk { get; set; }
    }
}