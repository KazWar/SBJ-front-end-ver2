using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.PromoCountries;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.PromoPlanner;
using RMS.PromoPlanner.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PromoCountries)]
    public class PromoCountriesController : RMSControllerBase
    {
        private readonly IPromoCountriesAppService _promoCountriesAppService;

        public PromoCountriesController(IPromoCountriesAppService promoCountriesAppService)
        {
            _promoCountriesAppService = promoCountriesAppService;
        }

        public ActionResult Index()
        {
            var model = new PromoCountriesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_PromoCountries_Create, AppPermissions.Pages_PromoCountries_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetPromoCountryForEditOutput getPromoCountryForEditOutput;

				if (id.HasValue){
					getPromoCountryForEditOutput = await _promoCountriesAppService.GetPromoCountryForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getPromoCountryForEditOutput = new GetPromoCountryForEditOutput{
						PromoCountry = new CreateOrEditPromoCountryDto()
					};
				}

				var viewModel = new CreateOrEditPromoCountryModalViewModel()
				{
					PromoCountry = getPromoCountryForEditOutput.PromoCountry,
					PromoPromocode = getPromoCountryForEditOutput.PromoPromocode,
					CountryCountryCode = getPromoCountryForEditOutput.CountryCountryCode,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewPromoCountryModal(long id)
        {
			var getPromoCountryForViewDto = await _promoCountriesAppService.GetPromoCountryForView(id);

            var model = new PromoCountryViewModel()
            {
                PromoCountry = getPromoCountryForViewDto.PromoCountry
                , PromoPromocode = getPromoCountryForViewDto.PromoPromocode 

                , CountryCountryCode = getPromoCountryForViewDto.CountryCountryCode 

            };

            return PartialView("_ViewPromoCountryModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_PromoCountries_Create, AppPermissions.Pages_PromoCountries_Edit)]
        public PartialViewResult PromoLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoCountryPromoLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoCountryPromoLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_PromoCountries_Create, AppPermissions.Pages_PromoCountries_Edit)]
        public PartialViewResult CountryLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PromoCountryCountryLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PromoCountryCountryLookupTableModal", viewModel);
        }

    }
}