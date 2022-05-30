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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Locale[]>> GetAll(
            [FromQuery] string? languageCode,
            [FromQuery] string? countryCode)
        {
            return Locales!;
        }

        [HttpGet("locale/{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Locale>> GetOne(
            [FromRoute] int id)
        {
            if (NullOrEmptyOrWhiteSpace.Check(id.ToString())) 
                return BadRequest($"Id: {id} is a null, contains only whitespace or is empty.");

            // Find the Locale
            var locale = Locales!.FirstOrDefault(x => x.Id == id);

            // Check if a locale is found
            if (locale != null)
            {
                return locale;
            }
            else
            {
                return NotFound($"No result has been found for the locale with the id: {id}.");
            }
        }
    }
}
