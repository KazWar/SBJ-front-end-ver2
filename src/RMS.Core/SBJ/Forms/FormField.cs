using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;
using RMS.SBJ.PurchaseRegistrationFields;
using RMS.SBJ.RegistrationFormFields;
using RMS.SBJ.RegistrationFields;

namespace RMS.SBJ.Forms
{
    [Table("FormField")]
    [Audited]
    public class FormField : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual string Label { get; set; }

        public virtual string DefaultValue { get; set; }

        public virtual int MaxLength { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool ReadOnly { get; set; }

        public virtual string InputMask { get; set; }

        public virtual string RegularExpression { get; set; }

        public virtual string ValidationApiCall { get; set; }

        public virtual string RegistrationField { get; set; }

        public virtual string PurchaseRegistrationField { get; set; }

        public virtual string FieldName { get; set; }

        public virtual long FieldTypeId { get; set; }

        [ForeignKey("FieldTypeId")]
        public FieldType FieldTypeFk { get; set; }

        public virtual ICollection<FormBlockField> FormBlockFields { get; set; }
        public virtual ICollection<FormFieldTranslation> FormFieldTranslations { get; set; }
        public virtual ICollection<FormFieldValueList> FormFieldValueLists { get; set; }
        public virtual ICollection<PurchaseRegistrationField> PurchaseRegistrationFields { get; set; }
        public virtual ICollection<RegistrationField> RegistrationFields { get; set; }
    }
}