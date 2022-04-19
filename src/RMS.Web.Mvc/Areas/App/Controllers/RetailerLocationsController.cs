using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.RetailerLocations;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.RetailerLocations;
using RMS.SBJ.RetailerLocations.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RetailerLocations)]
    public class RetailerLocationsController : RMSControllerBase
    {
        private readonly IRetailerLocationsAppService _retailerLocationsAppService;

        public RetailerLocationsController(IRetailerLocationsAppService retailerLocationsAppService)
        {
            _retailerLocationsAppService = retailerLocationsAppService;
        }

        public ActionResult Index()
        {
            var model = new RetailerLocationsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       
[AbpMvcAuthorize(AppPermissions.Pages_RetailerLocations_Create, AppPermissions.Pages_RetailerLocations_Edit)]
			public async Task<ActionResult> CreateOrEdit(long? id)
			{
				GetRetailerLocationForEditOutput getRetailerLocationForEditOutput;

				if (id.HasValue){
					getRetailerLocationForEditOutput = await _retailerLocationsAppService.GetRetailerLocationForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getRetailerLocationForEditOutput = new GetRetailerLocationForEditOutput{
						RetailerLocation = new CreateOrEditRetailerLocationDto()
					};
				}

				var viewModel = new CreateOrEditRetailerLocationViewModel()
				{
					RetailerLocation = getRetailerLocationForEditOutput.RetailerLocation,
					RetailerName = getRetailerLocationForEditOutput.RetailerName,
					AddressAddressLine1 = getRetailerLocationForEditOutput.AddressAddressLine1,                
				};

				return View(viewModel);
			}
			

        public async Task<ActionResult> ViewRetailerLocation(long id)
        {
			var getRetailerLocationForViewDto = await _retailerLocationsAppService.GetRetailerLocationForView(id);

            var model = new RetailerLocationViewModel()
            {
                RetailerLocation = getRetailerLocationForViewDto.RetailerLocation
                , RetailerName = getRetailerLocationForViewDto.RetailerName 

                , AddressAddressLine1 = getRetailerLocationForViewDto.AddressAddressLine1 

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RetailerLocations_Create, AppPermissions.Pages_RetailerLocations_Edit)]
        public PartialViewResult RetailerLookupTableModal(long? id, string displayName)
        {
            var viewModel = new RetailerLocationRetailerLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RetailerLocationRetailerLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_RetailerLocations_Create, AppPermissions.Pages_RetailerLocations_Edit)]
        public PartialViewResult AddressLookupTableModal(long? id, string displayName)
        {
            var viewModel = new RetailerLocationAddressLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RetailerLocationAddressLookupTableModal", viewModel);
        }

    }
}