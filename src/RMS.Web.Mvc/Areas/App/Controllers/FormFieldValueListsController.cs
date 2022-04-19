using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.FormFieldValueLists;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_FormFieldValueLists)]
    public class FormFieldValueListsController : RMSControllerBase
    {
        private readonly IFormFieldValueListsAppService _formFieldValueListsAppService;

        public FormFieldValueListsController(IFormFieldValueListsAppService formFieldValueListsAppService)
        {
            _formFieldValueListsAppService = formFieldValueListsAppService;
        }

        public ActionResult Index()
        {
            var model = new FormFieldValueListsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_FormFieldValueLists_Create, AppPermissions.Pages_FormFieldValueLists_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetFormFieldValueListForEditOutput getFormFieldValueListForEditOutput;

				if (id.HasValue){
					getFormFieldValueListForEditOutput = await _formFieldValueListsAppService.GetFormFieldValueListForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getFormFieldValueListForEditOutput = new GetFormFieldValueListForEditOutput{
						FormFieldValueList = new CreateOrEditFormFieldValueListDto()
					};
				}

				var viewModel = new CreateOrEditFormFieldValueListModalViewModel()
				{
					FormFieldValueList = getFormFieldValueListForEditOutput.FormFieldValueList,
					FormFieldDescription = getFormFieldValueListForEditOutput.FormFieldDescription,
					ValueListDescription = getFormFieldValueListForEditOutput.ValueListDescription,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewFormFieldValueListModal(long id)
        {
			var getFormFieldValueListForViewDto = await _formFieldValueListsAppService.GetFormFieldValueListForView(id);

            var model = new FormFieldValueListViewModel()
            {
                FormFieldValueList = getFormFieldValueListForViewDto.FormFieldValueList
                , FormFieldDescription = getFormFieldValueListForViewDto.FormFieldDescription 

                , ValueListDescription = getFormFieldValueListForViewDto.ValueListDescription 

            };

            return PartialView("_ViewFormFieldValueListModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FormFieldValueLists_Create, AppPermissions.Pages_FormFieldValueLists_Edit)]
        public PartialViewResult FormFieldLookupTableModal(long? id, string displayName)
        {
            var viewModel = new FormFieldValueListFormFieldLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FormFieldValueListFormFieldLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_FormFieldValueLists_Create, AppPermissions.Pages_FormFieldValueLists_Edit)]
        public PartialViewResult ValueListLookupTableModal(long? id, string displayName)
        {
            var viewModel = new FormFieldValueListValueListLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FormFieldValueListValueListLookupTableModal", viewModel);
        }

    }
}