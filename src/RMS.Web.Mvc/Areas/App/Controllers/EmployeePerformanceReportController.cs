using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Authorization;
using RMS.SBJ.Report.EmployeePerformanceReports;
using RMS.Web.Controllers;
using RMS.Web.Areas.App.Models.EmployeePerformanceReports;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpAuthorize(AppPermissions.Pages_EmployeePerformanceReport)]
    public class EmployeePerformanceReportController : RMSControllerBase
    {
        private readonly IEmployeePerformanceReportAppService _employeePerformanceReportAppService;

        public EmployeePerformanceReportController(IEmployeePerformanceReportAppService employeePerformanceReportAppService)
        {
            _employeePerformanceReportAppService = employeePerformanceReportAppService;
        }

        public ActionResult StoredProcedureNotFound()
        {
            return View();
        }

        public ActionResult Index()
        {
            var model = new EmployeePerformanceReportViewModel
            {

            };

            return View(model);
        }
    }
}
