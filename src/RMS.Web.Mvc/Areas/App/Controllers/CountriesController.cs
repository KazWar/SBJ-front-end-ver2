using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.Countries;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Countries)]
    public class CountriesController : RMSControllerBase
    {
        private readonly ICountriesAppService _countriesAppService;

        public CountriesController(ICountriesAppService countriesAppService)
        {
            _countriesAppService = countriesAppService;
        }

        public ActionResult Index()
        {
            var model = new CountriesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Countries_Create, AppPermissions.Pages_Countries_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetCountryForEditOutput getCountryForEditOutput;

				if (id.HasValue){
					getCountryForEditOutput = await _countriesAppService.GetCountryForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getCountryForEditOutput = new GetCountryForEditOutput{
						Country = new CreateOrEditCountryDto()
					};
				}

				var viewModel = new CreateOrEditCountryModalViewModel()
				{
					Country = getCountryForEditOutput.Country,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewCountryModal(long id)
        {
			var getCountryForViewDto = await _countriesAppService.GetCountryForView(id);

            var model = new CountryViewModel()
            {
                Country = getCountryForViewDto.Country
            };

            return PartialView("_ViewCountryModal", model);
        }


    }
}