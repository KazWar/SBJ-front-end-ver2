using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.FormBlockFields;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_FormBlockFields)]
    public class FormBlockFieldsController : RMSControllerBase
    {
        private readonly IFormBlockFieldsAppService _formBlockFieldsAppService;

        public FormBlockFieldsController(IFormBlockFieldsAppService formBlockFieldsAppService)
        {
            _formBlockFieldsAppService = formBlockFieldsAppService;
        }

        public ActionResult Index()
        {
            var model = new FormBlockFieldsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_FormBlockFields_Create, AppPermissions.Pages_FormBlockFields_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetFormBlockFieldForEditOutput getFormBlockFieldForEditOutput;

				if (id.HasValue){
					getFormBlockFieldForEditOutput = await _formBlockFieldsAppService.GetFormBlockFieldForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getFormBlockFieldForEditOutput = new GetFormBlockFieldForEditOutput{
						FormBlockField = new CreateOrEditFormBlockFieldDto()
					};
				}

				var viewModel = new CreateOrEditFormBlockFieldModalViewModel()
				{
					FormBlockField = getFormBlockFieldForEditOutput.FormBlockField,
					FormFieldDescription = getFormBlockFieldForEditOutput.FormFieldDescription,
					FormBlockDescription = getFormBlockFieldForEditOutput.FormBlockDescription,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewFormBlockFieldModal(long id)
        {
			var getFormBlockFieldForViewDto = await _formBlockFieldsAppService.GetFormBlockFieldForView(id);

            var model = new FormBlockFieldViewModel()
            {
                FormBlockField = getFormBlockFieldForViewDto.FormBlockField
                , FormFieldDescription = getFormBlockFieldForViewDto.FormFieldDescription 

                , FormBlockDescription = getFormBlockFieldForViewDto.FormBlockDescription 

            };

            return PartialView("_ViewFormBlockFieldModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FormBlockFields_Create, AppPermissions.Pages_FormBlockFields_Edit)]
        public PartialViewResult FormFieldLookupTableModal(long? id, string displayName)
        {
            var viewModel = new FormBlockFieldFormFieldLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FormBlockFieldFormFieldLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_FormBlockFields_Create, AppPermissions.Pages_FormBlockFields_Edit)]
        public PartialViewResult FormBlockLookupTableModal(long? id, string displayName)
        {
            var viewModel = new FormBlockFieldFormBlockLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FormBlockFieldFormBlockLookupTableModal", viewModel);
        }

    }
}