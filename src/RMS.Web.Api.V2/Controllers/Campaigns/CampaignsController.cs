using RMS.Web.Api.V2.Controllers.Campaigns.Models;

namespace RMS.Web.Api.V2.Controllers
{
    public class CampaignsController : BaseController
    {
        static readonly string data = System.IO.File.ReadAllText(@"./Controllers/Data/campaigns.json");
        readonly Campaign[]? Campaigns = JsonConvert.DeserializeObject<Campaign[]>(data);

        [HttpGet("campaign")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Campaign[]>> GetAll()
        {
            return Campaigns!;
        }
    }
}
