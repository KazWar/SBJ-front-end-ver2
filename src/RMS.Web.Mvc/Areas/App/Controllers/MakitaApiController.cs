using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using RMS.Authorization;
using RMS.SBJ.Makita;
using RMS.SBJ.Makita.Dtos;
using RMS.Web.Areas.App.Models.Makita;
using RMS.Web.Controllers;
using Abp.Web.Models;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;
using RMS.Web.Models.TenantSetup;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Registrations)]
    [DontWrapResult]
    public class MakitaApiController : RMSControllerBase
    {
        private readonly IMakitaRegistrationsAppService _makitaRegistrationsAppService;
        private readonly TenantSetupModel _tenantSetupModel;
        private readonly IMakitaApiAppService _makitaApiAppService;

        public MakitaApiController(IMakitaRegistrationsAppService makitaRegistrationsAppService, TenantSetupModel tenantSetupModel, IMakitaApiAppService makitaApiAppService)
        {
            _makitaRegistrationsAppService = makitaRegistrationsAppService;
            _tenantSetupModel = tenantSetupModel;
            _makitaApiAppService = makitaApiAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<object> MakitaRegistrationApproved(long registrationId)
        {
           var makitaRegistrationsPost = await _makitaApiAppService.MakitaRegistrationApproved(registrationId);
                       
           return makitaRegistrationsPost;
        }
    }
}
