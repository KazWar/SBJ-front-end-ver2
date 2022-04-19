using System.Collections.Generic;
using Newtonsoft.Json;

namespace RMS.SBJ.Forms.Dtos
{
    public sealed class FormFieldsExportJsonDto
    {
        [JsonProperty("fieldType")]
        public string FieldType { get; set; }

        [JsonProperty("name")]
        public string Description { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("labelList")]
        public List<LabelListDto> LabelList { get; set; }

        [JsonProperty("values")]
        public string DefaultValue { get; set; }

        [JsonProperty("maxlength")]
        public int MaxLength { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("readonly")]
        public bool ReadOnly { get; set; }

        [JsonProperty("inputmask")]
        public string InputMask { get; set; }

        [JsonProperty("IsPurchaseRegistration")]
        public bool IsPurchaseRegistration { get; set; }

        [JsonProperty("StartDate")]
        public string StartDate { get; set; }

        [JsonProperty("EndDate")]
        public string EndDate { get; set; }

        [JsonProperty("RegularExpression")]
        public string RegularExpression { get; set; }

        [JsonProperty("RegistrationField")]
        public string RegistrationField { get; set; }

        [JsonProperty("PurchaseRegistrationField")]
        public string PurchaseRegistrationField { get; set; }

        public long FormFieldId { get; set; }

        public List<GetFormFieldValueListDto> FormFieldValueList { get; set; }

        public List<DropdownListDto> DropdownList { get; set; }

        public List<PurchaseRegistrationsComponentDto> PurchaseOptions { get; set; }
        public List<DefaultValuesPurchaseRegistrationDto> DefaultValuesPurchaseRegistrations { get; set; }
    }
} 
 