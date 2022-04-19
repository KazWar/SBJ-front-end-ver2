using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.RejectionReasons;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RejectionReasons)]
    public class RejectionReasonsController : RMSControllerBase
    {
        private readonly IRejectionReasonsAppService _rejectionReasonsAppService;

        public RejectionReasonsController(IRejectionReasonsAppService rejectionReasonsAppService)
        {
            _rejectionReasonsAppService = rejectionReasonsAppService;
        }

        public ActionResult Index()
        {
            var model = new RejectionReasonsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RejectionReasons_Create, AppPermissions.Pages_RejectionReasons_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetRejectionReasonForEditOutput getRejectionReasonForEditOutput;

            if (id.HasValue)
            {
                getRejectionReasonForEditOutput = await _rejectionReasonsAppService.GetRejectionReasonForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getRejectionReasonForEditOutput = new GetRejectionReasonForEditOutput
                {
                    RejectionReason = new CreateOrEditRejectionReasonDto()
                };
            }

            var viewModel = new CreateOrEditRejectionReasonModalViewModel()
            {
                RejectionReason = getRejectionReasonForEditOutput.RejectionReason,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRejectionReasonModal(long id)
        {
            var getRejectionReasonForViewDto = await _rejectionReasonsAppService.GetRejectionReasonForView(id);

            var model = new RejectionReasonViewModel()
            {
                RejectionReason = getRejectionReasonForViewDto.RejectionReason
            };

            return PartialView("_ViewRejectionReasonModal", model);
        }

    }
}