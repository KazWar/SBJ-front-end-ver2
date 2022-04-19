using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.CodeTypeTables;
using System.Collections.Generic;

namespace RMS.SBJ.Forms
{
    [Table("FormLocale")]
    [Audited]
    public class FormLocale : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual long FormId { get; set; }

        public virtual long LocaleId { get; set; }

        [ForeignKey("FormId")]
        public Form FormFk { get; set; }

        [ForeignKey("LocaleId")]
        public Locale LocaleFk { get; set; }

        public virtual ICollection<FormBlock> FormBlocks { get; set; }
    }
}