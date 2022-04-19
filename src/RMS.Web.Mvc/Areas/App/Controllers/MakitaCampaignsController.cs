using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using RMS.Authorization;
using RMS.SBJ.Makita;
using RMS.Web.Controllers;
using RMS.Web.Models.TenantSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Controllers
{

    [Area("App")]
    public class MakitaCampaignsController : RMSControllerBase
    {
        private readonly IMakitaCampaignsAppService _makitaCampaignsAppService;
        private readonly TenantSetupModel _tenantSetupModel;

        public MakitaCampaignsController(IMakitaCampaignsAppService makitaCampaignsAppService, TenantSetupModel tenantSetupModel)
        {
            _makitaCampaignsAppService = makitaCampaignsAppService;
            _tenantSetupModel = tenantSetupModel;
        }

        public IActionResult OnlyWorksForMakitaTenant()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}
