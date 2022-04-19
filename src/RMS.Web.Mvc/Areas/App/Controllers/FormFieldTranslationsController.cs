using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.FormFieldTranslations;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_FormFieldTranslations)]
    public class FormFieldTranslationsController : RMSControllerBase
    {
        private readonly IFormFieldTranslationsAppService _formFieldTranslationsAppService;

        public FormFieldTranslationsController(IFormFieldTranslationsAppService formFieldTranslationsAppService)
        {
            _formFieldTranslationsAppService = formFieldTranslationsAppService;
        }

        public ActionResult Index()
        {
            var model = new FormFieldTranslationsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_FormFieldTranslations_Create, AppPermissions.Pages_FormFieldTranslations_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetFormFieldTranslationForEditOutput getFormFieldTranslationForEditOutput;

				if (id.HasValue){
					getFormFieldTranslationForEditOutput = await _formFieldTranslationsAppService.GetFormFieldTranslationForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getFormFieldTranslationForEditOutput = new GetFormFieldTranslationForEditOutput{
						FormFieldTranslation = new CreateOrEditFormFieldTranslationDto()
					};
				}

				var viewModel = new CreateOrEditFormFieldTranslationModalViewModel()
				{
					FormFieldTranslation = getFormFieldTranslationForEditOutput.FormFieldTranslation,
					FormFieldDescription = getFormFieldTranslationForEditOutput.FormFieldDescription,
					LocaleLanguageCode = getFormFieldTranslationForEditOutput.LocaleLanguageCode,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewFormFieldTranslationModal(long id)
        {
			var getFormFieldTranslationForViewDto = await _formFieldTranslationsAppService.GetFormFieldTranslationForView(id);

            var model = new FormFieldTranslationViewModel()
            {
                FormFieldTranslation = getFormFieldTranslationForViewDto.FormFieldTranslation
                , FormFieldDescription = getFormFieldTranslationForViewDto.FormFieldDescription 

                , LocaleLanguageCode = getFormFieldTranslationForViewDto.LocaleLanguageCode 

            };

            return PartialView("_ViewFormFieldTranslationModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FormFieldTranslations_Create, AppPermissions.Pages_FormFieldTranslations_Edit)]
        public PartialViewResult FormFieldLookupTableModal(long? id, string displayName)
        {
            var viewModel = new FormFieldTranslationFormFieldLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FormFieldTranslationFormFieldLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_FormFieldTranslations_Create, AppPermissions.Pages_FormFieldTranslations_Edit)]
        public PartialViewResult LocaleLookupTableModal(long? id, string displayName)
        {
            var viewModel = new FormFieldTranslationLocaleLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FormFieldTranslationLocaleLookupTableModal", viewModel);
        }

    }
}