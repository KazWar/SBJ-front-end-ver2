using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.Addresses;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Company;
using RMS.SBJ.Company.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Addresses)]
    public class AddressesController : RMSControllerBase
    {
        private readonly IAddressesAppService _addressesAppService;

        public AddressesController(IAddressesAppService addressesAppService)
        {
            _addressesAppService = addressesAppService;
        }

        public ActionResult Index()
        {
            var model = new AddressesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Addresses_Create, AppPermissions.Pages_Addresses_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetAddressForEditOutput getAddressForEditOutput;

				if (id.HasValue){
					getAddressForEditOutput = await _addressesAppService.GetAddressForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getAddressForEditOutput = new GetAddressForEditOutput{
						Address = new CreateOrEditAddressDto()
					};
				}

				var viewModel = new CreateOrEditAddressModalViewModel()
				{
					Address = getAddressForEditOutput.Address,
					CountryCountryCode = getAddressForEditOutput.CountryCountryCode,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewAddressModal(long id)
        {
			var getAddressForViewDto = await _addressesAppService.GetAddressForView(id);

            var model = new AddressViewModel()
            {
                Address = getAddressForViewDto.Address
                , CountryCountryCode = getAddressForViewDto.CountryCountryCode 

            };

            return PartialView("_ViewAddressModal", model);
        }


    }
}