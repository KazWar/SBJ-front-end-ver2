namespace RMS.Web.Api.V2.Controllers
{
    public class LocalesController : BaseController
    {
        static readonly string data = System.IO.File.ReadAllText(@"./Controllers/Data/locales.json");
        readonly Locale[]? Locales = JsonConvert.DeserializeObject<Locale[]>(data);

        [HttpGet("locale")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Locale[]>> GetAll(
            [FromQuery] string? languageCode,
            [FromQuery] string? countryCode)
        {
            return Locales!;
        }

        [HttpGet("locale")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Locale[]>> GetAll(
            [FromQuery] string? tenantId)
        {
            return Locales!;
        }
    }
}
