namespace RMS.Web.Api.V2.Controllers
{
    [ApiController]
    public class LanguageProfilesController : BaseController
    {
        static readonly string data = System.IO.File.ReadAllText(@"./Controllers/Data/campaigns.json");
        readonly Locale[]? Locales = JsonConvert.DeserializeObject<Locale[]>(data);

        [HttpGet("campaigns")]
        [Produces("application/json")]
        public async Task<ActionResult<Locale[]>> GetAll(
            [FromQuery] string languageCode,
            [FromQuery] string countryCode)
        {
            return Locales!.Where(locale =>
                locale.CountryCode == countryCode ||
                locale.LanguageCode == languageCode).ToArray();
        }

        [HttpGet("campaigns/{id:int}")]
        [Produces("application/json")]
        public Locale GetSingle(
            [FromRoute] int id)
        {
            return Locales!.Where(x => x.Id == id).First();
        }
    }
}
