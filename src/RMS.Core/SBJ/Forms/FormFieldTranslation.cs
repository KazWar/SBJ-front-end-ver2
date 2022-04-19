using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.CodeTypeTables;

namespace RMS.SBJ.Forms
{
    [Table("FormFieldTranslation")]
    [Audited]
    public class FormFieldTranslation : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Label { get; set; }

        public virtual string DefaultValue { get; set; }

        public virtual string RegularExpression { get; set; }

        public virtual long FormFieldId { get; set; }

        public virtual long LocaleId { get; set; }

        [ForeignKey("FormFieldId")]
        public FormField FormFieldFk { get; set; }

        [ForeignKey("LocaleId")]
        public Locale LocaleFk { get; set; }
    }
}