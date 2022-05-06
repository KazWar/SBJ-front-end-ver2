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
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<ActionResult<Locale[]>> GetAll(
            [FromQuery] string? languageCode,
            [FromQuery] string? countryCode)
        {
            return Locales!.Where(locale =>
                locale.CountryCode == countryCode ||
                locale.LanguageCode == languageCode).ToArray();
        }

        [HttpGet("locale/{id:int}")]
        public Locale GetSingle(
            [FromRoute] int id)
        {
            return Locales!.Where(x => x.Id == id).First();
        }
    }
}
