using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RMS.Web.Shared.Configuration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BuildFlags
    {
        prod,
        test,
        local
    }

    public class BuildConfiguration
    {
        public BuildFlags Environment { get; set; }
    }
}
