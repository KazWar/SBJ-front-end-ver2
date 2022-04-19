using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.Retailers;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Retailers;
using RMS.SBJ.Retailers.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Retailers)]
    public class RetailersController : RMSControllerBase
    {
        private readonly IRetailersAppService _retailersAppService;

        public RetailersController(IRetailersAppService retailersAppService)
        {
            _retailersAppService = retailersAppService;
        }

        public ActionResult Index()
        {
            var model = new RetailersViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Retailers_Create, AppPermissions.Pages_Retailers_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetRetailerForEditOutput getRetailerForEditOutput;

				if (id.HasValue){
					getRetailerForEditOutput = await _retailersAppService.GetRetailerForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getRetailerForEditOutput = new GetRetailerForEditOutput{
						Retailer = new CreateOrEditRetailerDto()
					};
				}

				var viewModel = new CreateOrEditRetailerModalViewModel()
				{
					Retailer = getRetailerForEditOutput.Retailer,
					CountryCountryCode = getRetailerForEditOutput.CountryCountryCode,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewRetailerModal(long id)
        {
			var getRetailerForViewDto = await _retailersAppService.GetRetailerForView(id);

            var model = new RetailerViewModel()
            {
                Retailer = getRetailerForViewDto.Retailer
                , CountryCountryCode = getRetailerForViewDto.CountryCountryCode 

            };

            return PartialView("_ViewRetailerModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Retailers_Create, AppPermissions.Pages_Retailers_Edit)]
        public PartialViewResult CountryLookupTableModal(long? id, string displayName)
        {
            var viewModel = new RetailerCountryLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RetailerCountryLookupTableModal", viewModel);
        }

    }
}