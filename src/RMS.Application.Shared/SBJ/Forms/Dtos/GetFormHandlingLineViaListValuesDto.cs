using Newtonsoft.Json;

namespace RMS.SBJ.Forms.Dtos
{
    public sealed class GetFormHandlingLineViaListValuesDto
    {
        [JsonProperty("ListValueTranslationKeyValue")]
        public string ListValueTranslationKeyValue { get; set; }

        [JsonProperty("ListValueTranslationDescription")]
        public string ListValueTranslationDescription { get; set; }
    }
}
