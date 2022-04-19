using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace RMS.SBJ.Forms
{
    [Table("FormFieldValueList")]
    [Audited]
    public class FormFieldValueList : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string PossibleListValues { get; set; }

        public virtual long FormFieldId { get; set; }

        public virtual long ValueListId { get; set; }

        [ForeignKey("FormFieldId")]
        public FormField FormFieldFk { get; set; }

        [ForeignKey("ValueListId")]
        public ValueList ValueListFk { get; set; }
    }
}