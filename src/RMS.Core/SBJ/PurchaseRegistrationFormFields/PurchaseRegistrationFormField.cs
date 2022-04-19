using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.Forms;

namespace RMS.SBJ.PurchaseRegistrationFormFields
{
    [Table("PurchaseRegistrationFormField")]
    [Audited]
    public class PurchaseRegistrationFormField : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual long FormFieldId { get; set; }

        [ForeignKey("FormFieldId")]
        public FormField FormFieldFk { get; set; }
    }
}