using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.Locales;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Locales)]
    public class LocalesController : RMSControllerBase
    {
        private readonly ILocalesAppService _localesAppService;

        public LocalesController(ILocalesAppService localesAppService)
        {
            _localesAppService = localesAppService;
        }

        public ActionResult Index()
        {
            var model = new LocalesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Locales_Create, AppPermissions.Pages_Locales_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetLocaleForEditOutput getLocaleForEditOutput;

				if (id.HasValue){
					getLocaleForEditOutput = await _localesAppService.GetLocaleForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getLocaleForEditOutput = new GetLocaleForEditOutput{
						Locale = new CreateOrEditLocaleDto()
					};
				}

				var viewModel = new CreateOrEditLocaleModalViewModel()
				{
					Locale = getLocaleForEditOutput.Locale,
					CountryCountryCode = getLocaleForEditOutput.CountryCountryCode,
					LocaleCountryList = await _localesAppService.GetAllCountryForTableDropdown(),                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}


        public async Task<PartialViewResult> ViewLocaleModal(long id)
        {
			var getLocaleForViewDto = await _localesAppService.GetLocaleForView(id);

            var model = new LocaleViewModel()
            {
                Locale = getLocaleForViewDto.Locale
                , CountryCountryCode = getLocaleForViewDto.CountryCountryCode 

            };

            return PartialView("_ViewLocaleModal", model);
        }


    }
}