using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;

namespace RMS.SBJ.Forms
{
    [Table("FormBlock")]
    [Audited]
    public class FormBlock : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsPurchaseRegistration { get; set; }

        public virtual int SortOrder { get; set; }

        public virtual long FormLocaleId { get; set; }

        [ForeignKey("FormLocaleId")]
        public FormLocale FormLocaleFk { get; set; }

        public virtual ICollection<FormBlockField> FormBlockFields { get; set; }
    }
}