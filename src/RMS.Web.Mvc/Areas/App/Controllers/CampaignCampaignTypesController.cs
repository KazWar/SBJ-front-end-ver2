using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.CampaignCampaignTypes;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CampaignCampaignTypes)]
    public class CampaignCampaignTypesController : RMSControllerBase
    {
        private readonly ICampaignCampaignTypesAppService _campaignCampaignTypesAppService;

        public CampaignCampaignTypesController(ICampaignCampaignTypesAppService campaignCampaignTypesAppService)
        {
            _campaignCampaignTypesAppService = campaignCampaignTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new CampaignCampaignTypesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_CampaignCampaignTypes_Create, AppPermissions.Pages_CampaignCampaignTypes_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetCampaignCampaignTypeForEditOutput getCampaignCampaignTypeForEditOutput;

				if (id.HasValue){
					getCampaignCampaignTypeForEditOutput = await _campaignCampaignTypesAppService.GetCampaignCampaignTypeForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getCampaignCampaignTypeForEditOutput = new GetCampaignCampaignTypeForEditOutput{
						CampaignCampaignType = new CreateOrEditCampaignCampaignTypeDto()
					};
				}

				var viewModel = new CreateOrEditCampaignCampaignTypeModalViewModel()
				{
					CampaignCampaignType = getCampaignCampaignTypeForEditOutput.CampaignCampaignType,
					CampaignDescription = getCampaignCampaignTypeForEditOutput.CampaignDescription,
					CampaignTypeName = getCampaignCampaignTypeForEditOutput.CampaignTypeName,
					CampaignCampaignTypeCampaignList = await _campaignCampaignTypesAppService.GetAllCampaignForTableDropdown(),
					CampaignCampaignTypeCampaignTypeList = await _campaignCampaignTypesAppService.GetAllCampaignTypeForTableDropdown(),                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewCampaignCampaignTypeModal(long id)
        {
			var getCampaignCampaignTypeForViewDto = await _campaignCampaignTypesAppService.GetCampaignCampaignTypeForView(id);

            var model = new CampaignCampaignTypeViewModel()
            {
                CampaignCampaignType = getCampaignCampaignTypeForViewDto.CampaignCampaignType
                , CampaignDescription = getCampaignCampaignTypeForViewDto.CampaignDescription 

                , CampaignTypeName = getCampaignCampaignTypeForViewDto.CampaignTypeName 

            };

            return PartialView("_ViewCampaignCampaignTypeModal", model);
        }


    }
}