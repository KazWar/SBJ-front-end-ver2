using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.PurchaseRegistrations;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.PurchaseRegistrations.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PurchaseRegistrations)]
    public class PurchaseRegistrationsController : RMSControllerBase
    {
        private readonly IPurchaseRegistrationsAppService _purchaseRegistrationsAppService;

        public PurchaseRegistrationsController(IPurchaseRegistrationsAppService purchaseRegistrationsAppService)
        {
            _purchaseRegistrationsAppService = purchaseRegistrationsAppService;
        }

        public ActionResult Index()
        {
            var model = new PurchaseRegistrationsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       
[AbpMvcAuthorize(AppPermissions.Pages_PurchaseRegistrations_Create, AppPermissions.Pages_PurchaseRegistrations_Edit)]
			public async Task<ActionResult> CreateOrEdit(long? id)
			{
				GetPurchaseRegistrationForEditOutput getPurchaseRegistrationForEditOutput;

				if (id.HasValue){
					getPurchaseRegistrationForEditOutput = await _purchaseRegistrationsAppService.GetPurchaseRegistrationForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getPurchaseRegistrationForEditOutput = new GetPurchaseRegistrationForEditOutput{
						PurchaseRegistration = new CreateOrEditPurchaseRegistrationDto()
					};
				getPurchaseRegistrationForEditOutput.PurchaseRegistration.PurchaseDate = DateTime.Now;
				}

				var viewModel = new CreateOrEditPurchaseRegistrationViewModel()
				{
					PurchaseRegistration = getPurchaseRegistrationForEditOutput.PurchaseRegistration,
					RegistrationFirstName = getPurchaseRegistrationForEditOutput.RegistrationFirstName,
					ProductCtn = getPurchaseRegistrationForEditOutput.ProductCtn,
					HandlingLineCustomerCode = getPurchaseRegistrationForEditOutput.HandlingLineCustomerCode,
					RetailerLocationName = getPurchaseRegistrationForEditOutput.RetailerLocationName,                
				};

				return View(viewModel);
			}
			

        public async Task<ActionResult> ViewPurchaseRegistration(long id)
        {
			var getPurchaseRegistrationForViewDto = await _purchaseRegistrationsAppService.GetPurchaseRegistrationForView(id);

            var model = new PurchaseRegistrationViewModel()
            {
                PurchaseRegistration = getPurchaseRegistrationForViewDto.PurchaseRegistration
                , RegistrationFirstName = getPurchaseRegistrationForViewDto.RegistrationFirstName 

                , ProductCtn = getPurchaseRegistrationForViewDto.ProductCtn 

                , HandlingLineCustomerCode = getPurchaseRegistrationForViewDto.HandlingLineCustomerCode 

                , RetailerLocationName = getPurchaseRegistrationForViewDto.RetailerLocationName 

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_PurchaseRegistrations_Create, AppPermissions.Pages_PurchaseRegistrations_Edit)]
        public PartialViewResult RegistrationLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PurchaseRegistrationRegistrationLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PurchaseRegistrationRegistrationLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_PurchaseRegistrations_Create, AppPermissions.Pages_PurchaseRegistrations_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PurchaseRegistrationProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PurchaseRegistrationProductLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_PurchaseRegistrations_Create, AppPermissions.Pages_PurchaseRegistrations_Edit)]
        public PartialViewResult HandlingLineLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PurchaseRegistrationHandlingLineLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PurchaseRegistrationHandlingLineLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_PurchaseRegistrations_Create, AppPermissions.Pages_PurchaseRegistrations_Edit)]
        public PartialViewResult RetailerLocationLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PurchaseRegistrationRetailerLocationLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PurchaseRegistrationRetailerLocationLookupTableModal", viewModel);
        }

    }
}