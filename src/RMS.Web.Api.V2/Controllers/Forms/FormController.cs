namespace RMS.Web.Api.V2.Controllers.Forms
{
    public class FormController : BaseController
    {
        static readonly string data = System.IO.File.ReadAllText(@"./Controllers/Data/forms.json");
        readonly Form[]? forms = JsonConvert.DeserializeObject<Form[]>(data);

        [HttpGet("form")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Form[]>> GetAll(
            [FromQuery] string? locale)
        {
            return forms!;
        }

        [HttpGet("form/{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Form>> GetOne(
            [FromRoute] int id)
        {
            if (NullOrEmptyOrWhiteSpace.Check(id.ToString()))
                return BadRequest($"Id: {id} is a null, contains only whitespace or is empty.");

            var form = forms!.First(x=> x.Id == id);

            // Check if a locale is found
            if (forms != null)
            {
                return form;
            }
            else
            {
                return NotFound($"No result has been found for the locale with the id: {id}.");
            }
        }
    }
}
