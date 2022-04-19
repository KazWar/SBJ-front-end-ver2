using System.Collections.Generic;

namespace RMS.SBJ.Registrations.Dtos.ProcessRegistration
{
    public sealed class FormField
    {
        public long FieldId { get; set; }
        public long FieldTypeId { get; set; }
        public string FieldType { get; set; }
        public string FieldLabel { get; set; }
        public string FieldValue { get; set; }
        public string FieldSource { get; set; }
        public string FieldName { get; set; }
        public long? FieldLineId { get; set; }
        public long? FallbackFieldId { get; set; }
        public string RegistrationField { get; set; }
        public string PurchaseRegistrationField { get; set; }
        public bool IsRejected { get; set; }
        public bool IsReadOnly { get; set; }
        public IList<FormFieldListValue> FieldListValues { get; set; }
    }
}
