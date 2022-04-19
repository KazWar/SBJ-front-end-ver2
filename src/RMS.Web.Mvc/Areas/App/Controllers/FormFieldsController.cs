using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.FormFields;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_FormFields)]
    public class FormFieldsController : RMSControllerBase
    {
        private readonly IFormFieldsAppService _formFieldsAppService;

        public FormFieldsController(IFormFieldsAppService formFieldsAppService)
        {
            _formFieldsAppService = formFieldsAppService;
        }

        public ActionResult Index()
        {
            var model = new FormFieldsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_FormFields_Create, AppPermissions.Pages_FormFields_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetFormFieldForEditOutput getFormFieldForEditOutput;

				if (id.HasValue){
					getFormFieldForEditOutput = await _formFieldsAppService.GetFormFieldForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getFormFieldForEditOutput = new GetFormFieldForEditOutput{
						FormField = new CreateOrEditFormFieldDto()
					};
				}

				var viewModel = new CreateOrEditFormFieldModalViewModel()
				{
					FormField = getFormFieldForEditOutput.FormField,
					FieldTypeDescription = getFormFieldForEditOutput.FieldTypeDescription,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewFormFieldModal(long id)
        {
			var getFormFieldForViewDto = await _formFieldsAppService.GetFormFieldForView(id);

            var model = new FormFieldViewModel()
            {
                FormField = getFormFieldForViewDto.FormField
                , FieldTypeDescription = getFormFieldForViewDto.FieldTypeDescription 

            };

            return PartialView("_ViewFormFieldModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FormFields_Create, AppPermissions.Pages_FormFields_Edit)]
        public PartialViewResult FieldTypeLookupTableModal(long? id, string displayName)
        {
            var viewModel = new FormFieldFieldTypeLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FormFieldFieldTypeLookupTableModal", viewModel);
        }

    }
}