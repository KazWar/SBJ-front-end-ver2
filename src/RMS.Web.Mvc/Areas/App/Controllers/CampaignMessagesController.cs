using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.CampaignMessages;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CampaignMessages)]
    public class CampaignMessagesController : RMSControllerBase
    {
        private readonly ICampaignMessagesAppService _campaignMessagesAppService;

        public CampaignMessagesController(ICampaignMessagesAppService campaignMessagesAppService)
        {
            _campaignMessagesAppService = campaignMessagesAppService;
        }

        public ActionResult Index()
        {
            var model = new CampaignMessagesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_CampaignMessages_Create, AppPermissions.Pages_CampaignMessages_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetCampaignMessageForEditOutput getCampaignMessageForEditOutput;

				if (id.HasValue){
					getCampaignMessageForEditOutput = await _campaignMessagesAppService.GetCampaignMessageForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getCampaignMessageForEditOutput = new GetCampaignMessageForEditOutput{
						CampaignMessage = new CreateOrEditCampaignMessageDto()
					};
				}

				var viewModel = new CreateOrEditCampaignMessageModalViewModel()
				{
					CampaignMessage = getCampaignMessageForEditOutput.CampaignMessage,
					CampaignName = getCampaignMessageForEditOutput.CampaignName,
					MessageVersion = getCampaignMessageForEditOutput.MessageVersion,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewCampaignMessageModal(long id)
        {
			var getCampaignMessageForViewDto = await _campaignMessagesAppService.GetCampaignMessageForView(id);

            var model = new CampaignMessageViewModel()
            {
                CampaignMessage = getCampaignMessageForViewDto.CampaignMessage
                , CampaignName = getCampaignMessageForViewDto.CampaignName 

                , MessageVersion = getCampaignMessageForViewDto.MessageVersion 

            };

            return PartialView("_ViewCampaignMessageModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CampaignMessages_Create, AppPermissions.Pages_CampaignMessages_Edit)]
        public PartialViewResult CampaignLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignMessageCampaignLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CampaignMessageCampaignLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_CampaignMessages_Create, AppPermissions.Pages_CampaignMessages_Edit)]
        public PartialViewResult MessageLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignMessageMessageLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CampaignMessageMessageLookupTableModal", viewModel);
        }

    }
}