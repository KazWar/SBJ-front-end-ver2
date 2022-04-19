using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.CampaignTypeEventRegistrationStatuses;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses)]
    public class CampaignTypeEventRegistrationStatusesController : RMSControllerBase
    {
        private readonly ICampaignTypeEventRegistrationStatusesAppService _campaignTypeEventRegistrationStatusesAppService;

        public CampaignTypeEventRegistrationStatusesController(ICampaignTypeEventRegistrationStatusesAppService campaignTypeEventRegistrationStatusesAppService)
        {
            _campaignTypeEventRegistrationStatusesAppService = campaignTypeEventRegistrationStatusesAppService;
        }

        public ActionResult Index()
        {
            var model = new CampaignTypeEventRegistrationStatusesViewModel
			{
				FilterText = ""
			};

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Create, AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetCampaignTypeEventRegistrationStatusForEditOutput getCampaignTypeEventRegistrationStatusForEditOutput;

			if (id.HasValue){
				getCampaignTypeEventRegistrationStatusForEditOutput = await _campaignTypeEventRegistrationStatusesAppService.GetCampaignTypeEventRegistrationStatusForEdit(new EntityDto<long> { Id = (long) id });
			}
			else{
				getCampaignTypeEventRegistrationStatusForEditOutput = new GetCampaignTypeEventRegistrationStatusForEditOutput{
					CampaignTypeEventRegistrationStatus = new CreateOrEditCampaignTypeEventRegistrationStatusDto()
				};
			}

            var viewModel = new CreateOrEditCampaignTypeEventRegistrationStatusModalViewModel()
            {
				CampaignTypeEventRegistrationStatus = getCampaignTypeEventRegistrationStatusForEditOutput.CampaignTypeEventRegistrationStatus,
					CampaignTypeEventSortOrder = getCampaignTypeEventRegistrationStatusForEditOutput.CampaignTypeEventSortOrder,
					RegistrationStatusDescription = getCampaignTypeEventRegistrationStatusForEditOutput.RegistrationStatusDescription
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCampaignTypeEventRegistrationStatusModal(long id)
        {
			var getCampaignTypeEventRegistrationStatusForViewDto = await _campaignTypeEventRegistrationStatusesAppService.GetCampaignTypeEventRegistrationStatusForView(id);

            var model = new CampaignTypeEventRegistrationStatusViewModel()
            {
				CampaignTypeEventRegistrationStatus = getCampaignTypeEventRegistrationStatusForViewDto.CampaignTypeEventRegistrationStatus
, CampaignTypeEventSortOrder = getCampaignTypeEventRegistrationStatusForViewDto.CampaignTypeEventSortOrder 
, RegistrationStatusDescription = getCampaignTypeEventRegistrationStatusForViewDto.RegistrationStatusDescription 

            };

            return PartialView("_ViewCampaignTypeEventRegistrationStatusModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Create, AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Edit)]
        public PartialViewResult CampaignTypeEventLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignTypeEventRegistrationStatusCampaignTypeEventLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CTERSCampaignTypeEventLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Create, AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Edit)]
        public PartialViewResult RegistrationStatusLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignTypeEventRegistrationStatusRegistrationStatusLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CTERSRegistrationStatusLookupTableModal", viewModel);
        }

    }
}