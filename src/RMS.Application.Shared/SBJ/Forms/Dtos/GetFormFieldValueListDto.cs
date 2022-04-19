using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormFieldValueListDto
    {

        [JsonProperty("ListValueTranslationKeyValue")]
        public string ListValueTranslationKeyValue { get; set; }

        [JsonProperty("ListValueTranslationDescription")]
        public string ListValueTranslationDescription { get; set; }

        [JsonProperty("HandlingLine")]
        public List<GetFormHandlingLineDto> HandlingLine { get; set; }

        
    }
}
