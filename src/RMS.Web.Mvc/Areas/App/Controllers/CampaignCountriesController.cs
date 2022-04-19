using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.CampaignCountries;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CampaignCountries)]
    public class CampaignCountriesController : RMSControllerBase
    {
        private readonly ICampaignCountriesAppService _campaignCountriesAppService;

        public CampaignCountriesController(ICampaignCountriesAppService campaignCountriesAppService)
        {
            _campaignCountriesAppService = campaignCountriesAppService;

        }

        public ActionResult Index()
        {
            var model = new CampaignCountriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CampaignCountries_Create, AppPermissions.Pages_CampaignCountries_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetCampaignCountryForEditOutput getCampaignCountryForEditOutput;

            if (id.HasValue)
            {
                getCampaignCountryForEditOutput = await _campaignCountriesAppService.GetCampaignCountryForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getCampaignCountryForEditOutput = new GetCampaignCountryForEditOutput
                {
                    CampaignCountry = new CreateOrEditCampaignCountryDto()
                };
            }

            var viewModel = new CreateOrEditCampaignCountryModalViewModel()
            {
                CampaignCountry = getCampaignCountryForEditOutput.CampaignCountry,
                CampaignName = getCampaignCountryForEditOutput.CampaignName,
                CountryDescription = getCampaignCountryForEditOutput.CountryDescription,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCampaignCountryModal(long id)
        {
            var getCampaignCountryForViewDto = await _campaignCountriesAppService.GetCampaignCountryForView(id);

            var model = new CampaignCountryViewModel()
            {
                CampaignCountry = getCampaignCountryForViewDto.CampaignCountry
                ,
                CampaignName = getCampaignCountryForViewDto.CampaignName

                ,
                CountryDescription = getCampaignCountryForViewDto.CountryDescription

            };

            return PartialView("_ViewCampaignCountryModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CampaignCountries_Create, AppPermissions.Pages_CampaignCountries_Edit)]
        public PartialViewResult CampaignLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignCountryCampaignLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CampaignCountryCampaignLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_CampaignCountries_Create, AppPermissions.Pages_CampaignCountries_Edit)]
        public PartialViewResult CountryLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignCountryCountryLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CampaignCountryCountryLookupTableModal", viewModel);
        }

    }
}