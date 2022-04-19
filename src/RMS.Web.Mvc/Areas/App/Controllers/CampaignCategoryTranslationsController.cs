using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.CampaignCategoryTranslations;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CampaignCategoryTranslations)]
    public class CampaignCategoryTranslationsController : RMSControllerBase
    {
        private readonly ICampaignCategoryTranslationsAppService _campaignCategoryTranslationsAppService;

        public CampaignCategoryTranslationsController(ICampaignCategoryTranslationsAppService campaignCategoryTranslationsAppService)
        {
            _campaignCategoryTranslationsAppService = campaignCategoryTranslationsAppService;
        }

        public ActionResult Index()
        {
            var model = new CampaignCategoryTranslationsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_CampaignCategoryTranslations_Create, AppPermissions.Pages_CampaignCategoryTranslations_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetCampaignCategoryTranslationForEditOutput getCampaignCategoryTranslationForEditOutput;

				if (id.HasValue){
					getCampaignCategoryTranslationForEditOutput = await _campaignCategoryTranslationsAppService.GetCampaignCategoryTranslationForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getCampaignCategoryTranslationForEditOutput = new GetCampaignCategoryTranslationForEditOutput{
						CampaignCategoryTranslation = new CreateOrEditCampaignCategoryTranslationDto()
					};
				}

				var viewModel = new CreateOrEditCampaignCategoryTranslationModalViewModel()
				{
					CampaignCategoryTranslation = getCampaignCategoryTranslationForEditOutput.CampaignCategoryTranslation,
					LocaleDescription = getCampaignCategoryTranslationForEditOutput.LocaleDescription,
					CampaignCategoryName = getCampaignCategoryTranslationForEditOutput.CampaignCategoryName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewCampaignCategoryTranslationModal(long id)
        {
			var getCampaignCategoryTranslationForViewDto = await _campaignCategoryTranslationsAppService.GetCampaignCategoryTranslationForView(id);

            var model = new CampaignCategoryTranslationViewModel()
            {
                CampaignCategoryTranslation = getCampaignCategoryTranslationForViewDto.CampaignCategoryTranslation
                , LocaleDescription = getCampaignCategoryTranslationForViewDto.LocaleDescription 

                , CampaignCategoryName = getCampaignCategoryTranslationForViewDto.CampaignCategoryName 

            };

            return PartialView("_ViewCampaignCategoryTranslationModal", model);
        }


    }
}