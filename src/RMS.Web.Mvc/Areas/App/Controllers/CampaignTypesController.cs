using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.CampaignTypes;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CampaignTypes)]
    public class CampaignTypesController : RMSControllerBase
    {
        private readonly ICampaignTypesAppService _campaignTypesAppService;

        public CampaignTypesController(ICampaignTypesAppService campaignTypesAppService)
        {
            _campaignTypesAppService = campaignTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new CampaignTypesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_CampaignTypes_Create, AppPermissions.Pages_CampaignTypes_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetCampaignTypeForEditOutput getCampaignTypeForEditOutput;

				if (id.HasValue){
					getCampaignTypeForEditOutput = await _campaignTypesAppService.GetCampaignTypeForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getCampaignTypeForEditOutput = new GetCampaignTypeForEditOutput{
						CampaignType = new CreateOrEditCampaignTypeDto()
					};
				}

				var viewModel = new CreateOrEditCampaignTypeModalViewModel()
				{
					CampaignType = getCampaignTypeForEditOutput.CampaignType,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewCampaignTypeModal(long id)
        {
			var getCampaignTypeForViewDto = await _campaignTypesAppService.GetCampaignTypeForView(id);

            var model = new CampaignTypeViewModel()
            {
                CampaignType = getCampaignTypeForViewDto.CampaignType
            };

            return PartialView("_ViewCampaignTypeModal", model);
        }


    }
}