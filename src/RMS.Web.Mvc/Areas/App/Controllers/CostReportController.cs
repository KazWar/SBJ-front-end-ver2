using Abp.Authorization;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using RMS.Authorization;
using RMS.Dto;
using RMS.SBJ.Report.CostReports;
using RMS.SBJ.Report.CostReports.Dtos;
using RMS.Web.Areas.App.Models.CostReport;
using RMS.Web.Controllers;
using System;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpAuthorize(AppPermissions.Pages_CostReport)]
    public class CostReportController : RMSControllerBase
    {
        private readonly ICostReportAppService _CostReportAppService;

        public CostReportController(ICostReportAppService costReportAppService)
        {
            _CostReportAppService = costReportAppService;
        }
        public async Task<ActionResult> Index()
        {
            //var result = await _CostReportAppService.GetCostReport(new SBJ.Report.CostReports.Dtos.GetAllCostReportInput
            //{
            //    CampaignId = 2,
            //    StartDateFilter = new DateTime(2022, 1, 1),
            //    EndDateFilter = new DateTime(2022, 1, 31)
            //});

            var model = new CostReportViewModel
            {
            };
            return View(model);
        }

        [HttpPost]
        [DontWrapResult]
        public async Task<JsonResult> GetCostReport([FromBody] GetAllCostReportInput input)
        {
            var costReport = await _CostReportAppService.GetCostReport(input);
            var costReportOut = Json(costReport);

            return costReportOut;
        }

        [HttpPost]
        [DontWrapResult]
        public async Task<FileDto> GetCostReportToExcel([FromBody] GetAllCostReportInput input)
        {
            return await _CostReportAppService.GetCostReportToExcel(input);
        }
    }
}
