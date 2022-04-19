using Newtonsoft.Json;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormHandlingLineDto
    {
        [JsonProperty("HandlingLineId")]
        public string HandlingLineId { get; set; }

        [JsonProperty("HandlingLineDescription")]
        public string HandlingLineDescription { get; set; }

        [JsonProperty("ChosenItemId")]
        public string ChosenItemId { get; set; }
    }
}
