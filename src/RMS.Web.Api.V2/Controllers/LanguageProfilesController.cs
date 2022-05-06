using Microsoft.AspNetCore.Http;

namespace RMS.Web.Api.V2.Controllers
{
    public class LanguageProfilesController : BaseController
    {
        static readonly string data = System.IO.File.ReadAllText(@"./Controllers/Data/locales.json");
        readonly Locale[]? LanguageProfiles = JsonConvert.DeserializeObject<Locale[]>(data);

        [HttpGet("language-profile")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Locale[]>> GetAll(
            [FromQuery] string? LocaleCode,
            [FromQuery] string? CampaignId)
        {
            return Locales!.Where(Profile =>
                Profile.LocaleCode == LocaleCode).ToArray();
        }

        [HttpGet("language-profile/{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Locale>> GetSingle(
            [FromRoute] int id = -1)
        {
            if (id is (-1))
            {
                return BadRequest();
            }

            return Locales!.Where(x => x.Id == id).First();
        }
    }
}
