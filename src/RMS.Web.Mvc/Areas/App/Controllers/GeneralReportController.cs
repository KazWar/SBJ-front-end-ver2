using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Authorization;
using RMS.SBJ.Report.GeneralReports;
using RMS.Web.Controllers;
using RMS.Web.Areas.App.Models.GeneralReports;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpAuthorize(AppPermissions.Pages_GeneralReport)]
    public class GeneralReportController : RMSControllerBase
    {
        private readonly IGeneralReportAppService _generalReportAppService;

        public GeneralReportController(IGeneralReportAppService generalReportAppService)
        {
            _generalReportAppService = generalReportAppService;
        }

        public ActionResult Index()
        {
            var model = new GeneralReportViewModel
            {

            };

            return View(model);
        }
    }
}
