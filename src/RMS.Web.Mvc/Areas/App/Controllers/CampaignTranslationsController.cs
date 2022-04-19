using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.CampaignTranslations;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CampaignTranslations)]
    public class CampaignTranslationsController : RMSControllerBase
    {
        private readonly ICampaignTranslationsAppService _campaignTranslationsAppService;

        public CampaignTranslationsController(ICampaignTranslationsAppService campaignTranslationsAppService)
        {
            _campaignTranslationsAppService = campaignTranslationsAppService;

        }

        public ActionResult Index()
        {
            var model = new CampaignTranslationsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CampaignTranslations_Create, AppPermissions.Pages_CampaignTranslations_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetCampaignTranslationForEditOutput getCampaignTranslationForEditOutput;

            if (id.HasValue)
            {
                getCampaignTranslationForEditOutput = await _campaignTranslationsAppService.GetCampaignTranslationForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getCampaignTranslationForEditOutput = new GetCampaignTranslationForEditOutput
                {
                    CampaignTranslation = new CreateOrEditCampaignTranslationDto()
                };
            }

            var viewModel = new CreateOrEditCampaignTranslationModalViewModel()
            {
                CampaignTranslation = getCampaignTranslationForEditOutput.CampaignTranslation,
                CampaignName = getCampaignTranslationForEditOutput.CampaignName,
                LocaleDescription = getCampaignTranslationForEditOutput.LocaleDescription,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCampaignTranslationModal(long id)
        {
            var getCampaignTranslationForViewDto = await _campaignTranslationsAppService.GetCampaignTranslationForView(id);

            var model = new CampaignTranslationViewModel()
            {
                CampaignTranslation = getCampaignTranslationForViewDto.CampaignTranslation
                ,
                CampaignName = getCampaignTranslationForViewDto.CampaignName

                ,
                LocaleDescription = getCampaignTranslationForViewDto.LocaleDescription

            };

            return PartialView("_ViewCampaignTranslationModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CampaignTranslations_Create, AppPermissions.Pages_CampaignTranslations_Edit)]
        public PartialViewResult CampaignLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignTranslationCampaignLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CampaignTranslationCampaignLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_CampaignTranslations_Create, AppPermissions.Pages_CampaignTranslations_Edit)]
        public PartialViewResult LocaleLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignTranslationLocaleLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CampaignTranslationLocaleLookupTableModal", viewModel);
        }

    }
}