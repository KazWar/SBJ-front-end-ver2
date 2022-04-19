using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.HandlingLines;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.HandlingLines.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_HandlingLines)]
    public class HandlingLinesController : RMSControllerBase
    {
        private readonly IHandlingLinesAppService _handlingLinesAppService;

        public HandlingLinesController(IHandlingLinesAppService handlingLinesAppService)
        {
            _handlingLinesAppService = handlingLinesAppService;
        }

        public ActionResult Index()
        {
            var model = new HandlingLinesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_HandlingLines_Create, AppPermissions.Pages_HandlingLines_Edit)]
        public async Task<ActionResult> CreateOrEdit(long? id)
        {
            GetHandlingLineForEditOutput getHandlingLineForEditOutput;

            if (id.HasValue)
            {
                getHandlingLineForEditOutput = await _handlingLinesAppService.GetHandlingLineForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getHandlingLineForEditOutput = new GetHandlingLineForEditOutput
                {
                    HandlingLine = new CreateOrEditHandlingLineDto()
                };
            }

            var viewModel = new CreateOrEditHandlingLineViewModel()
            {
                HandlingLine = getHandlingLineForEditOutput.HandlingLine,
                CampaignTypeName = getHandlingLineForEditOutput.CampaignTypeName,
                ProductHandlingDescription = getHandlingLineForEditOutput.ProductHandlingDescription,
                HandlingLineCampaignTypeList = await _handlingLinesAppService.GetAllCampaignTypeForTableDropdown(),
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewHandlingLine(long id)
        {
            var getHandlingLineForViewDto = await _handlingLinesAppService.GetHandlingLineForView(id);

            var model = new HandlingLineViewModel()
            {
                HandlingLine = getHandlingLineForViewDto.HandlingLine
                ,
                CampaignTypeName = getHandlingLineForViewDto.CampaignTypeName

                ,
                ProductHandlingDescription = getHandlingLineForViewDto.ProductHandlingDescription

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_HandlingLines_Create, AppPermissions.Pages_HandlingLines_Edit)]
        public PartialViewResult ProductHandlingLookupTableModal(long? id, string displayName)
        {
            var viewModel = new HandlingLineProductHandlingLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_HandlingLineProductHandlingLookupTableModal", viewModel);
        }

    }
}