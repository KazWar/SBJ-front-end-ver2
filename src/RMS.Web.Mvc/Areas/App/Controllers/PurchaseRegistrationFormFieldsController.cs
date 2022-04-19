using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.PurchaseRegistrationFormFields;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.PurchaseRegistrationFormFields;
using RMS.SBJ.PurchaseRegistrationFormFields.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PurchaseRegistrationFormFields)]
    public class PurchaseRegistrationFormFieldsController : RMSControllerBase
    {
        private readonly IPurchaseRegistrationFormFieldsAppService _purchaseRegistrationFormFieldsAppService;

        public PurchaseRegistrationFormFieldsController(IPurchaseRegistrationFormFieldsAppService purchaseRegistrationFormFieldsAppService)
        {
            _purchaseRegistrationFormFieldsAppService = purchaseRegistrationFormFieldsAppService;
        }

        public ActionResult Index()
        {
            var model = new PurchaseRegistrationFormFieldsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_PurchaseRegistrationFormFields_Create, AppPermissions.Pages_PurchaseRegistrationFormFields_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetPurchaseRegistrationFormFieldForEditOutput getPurchaseRegistrationFormFieldForEditOutput;

				if (id.HasValue){
					getPurchaseRegistrationFormFieldForEditOutput = await _purchaseRegistrationFormFieldsAppService.GetPurchaseRegistrationFormFieldForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getPurchaseRegistrationFormFieldForEditOutput = new GetPurchaseRegistrationFormFieldForEditOutput{
						PurchaseRegistrationFormField = new CreateOrEditPurchaseRegistrationFormFieldDto()
					};
				}

				var viewModel = new CreateOrEditPurchaseRegistrationFormFieldModalViewModel()
				{
					PurchaseRegistrationFormField = getPurchaseRegistrationFormFieldForEditOutput.PurchaseRegistrationFormField,
					FormFieldDescription = getPurchaseRegistrationFormFieldForEditOutput.FormFieldDescription,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewPurchaseRegistrationFormFieldModal(long id)
        {
			var getPurchaseRegistrationFormFieldForViewDto = await _purchaseRegistrationFormFieldsAppService.GetPurchaseRegistrationFormFieldForView(id);

            var model = new PurchaseRegistrationFormFieldViewModel()
            {
                PurchaseRegistrationFormField = getPurchaseRegistrationFormFieldForViewDto.PurchaseRegistrationFormField
                , FormFieldDescription = getPurchaseRegistrationFormFieldForViewDto.FormFieldDescription 

            };

            return PartialView("_ViewPurchaseRegistrationFormFieldModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_PurchaseRegistrationFormFields_Create, AppPermissions.Pages_PurchaseRegistrationFormFields_Edit)]
        public PartialViewResult FormFieldLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PurchaseRegistrationFormFieldFormFieldLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PurchaseRegistrationFormFieldFormFieldLookupTableModal", viewModel);
        }

    }
}