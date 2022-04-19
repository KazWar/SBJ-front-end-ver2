using System.Collections.Generic;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormBlockFieldForEditDto
    {
        public long FieldId { get; set; }

        public long BlockId { get; set; }

        public int FieldTarget { get; set; }

        public string FieldName { get; set; }

        public string FieldDescription { get; set; }

        public string FieldLabelGlobal { get; set; }

        public string FieldLabelLocale { get; set; }

        public string DefaultValueGlobal { get; set; }

        public string DefaultValueLocale { get; set; }

        public string RegularExpressionGlobal { get; set; }

        public string RegularExpressionLocale { get; set; }

        public string RegistrationField { get; set; }

        public string PurchaseRegistrationField { get; set; }

        public long CustomRegistrationFieldId { get; set; }

        public long CustomPurchaseRegistrationFieldId { get; set; }

        public long FieldTypeId { get; set; }

        public int MaxLength { get; set; }

        public bool RequiredField { get; set; }

        public bool ReadOnly { get; set; }

        public long LocaleId { get; set; }

        public string Locale { get; set; }

        public string DropDownDesc { get; set; }

        public int SortOrderBlock { get; set; }

        public int SortOrderField { get; set; }

        public Dictionary<long, string> AvailableFieldTypes { get; set; }

        public Dictionary<long, string> AvailableCustomRegistrationFields { get; set; }

        public Dictionary<long, string> AvailableCustomPurchaseRegistrationFields { get; set; }
    }
}
