using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Controllers;
using RMS.SBJ.UniqueCodes;
using RMS.Web.Areas.App.Models.UniqueCodes;
using System.Net.Http;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    public class UniqueCodesController : RMSControllerBase
    {
        private readonly IUniqueCodesAppService _uniqueCodesAppService;

        public UniqueCodesController(IUniqueCodesAppService uniqueCodesAppService)
        {
            _uniqueCodesAppService = uniqueCodesAppService;
        }
        
        [HttpGet]
        public async Task<HttpResponseMessage> IsCodeValid(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            var isValid = await _uniqueCodesAppService.IsCodeValid(code.Trim());

            if (isValid == true)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);

        }

        [HttpGet]
        public async Task<HttpResponseMessage> IsCodeValidByCampaign(string code, string? campaignCode)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            var isValid = await _uniqueCodesAppService.IsCodeValidByCampaign(code.Trim(), campaignCode);

            if (isValid == true)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);

        }

        [HttpPost]
        public async Task<HttpResponseMessage> SetCodeUsed([FromBody]UniqueCodeViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            var successfullySetCodeAsUsed = await _uniqueCodesAppService.SetCodeUsed(model.Code.Trim());
            
            if (successfullySetCodeAsUsed == true)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
           
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SetCodeUsedByCampaign([FromBody] UniqueCodeViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            var successfullySetCodeAsUsed = await _uniqueCodesAppService.SetCodeUsedByCampaign(model.Code.Trim(), model.CampaignCode);

            if (successfullySetCodeAsUsed == true)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);

        }
    }
}