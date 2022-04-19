using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RMS.Web.Areas.App.Models.Forms;
using RMS.Web.Controllers;
using RMS.SBJ.CodeTypeTables.Dtos;
using System;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Forms)]
    public class FormsController : RMSControllerBase
    {
        private readonly IFormsAppService _formsAppService;
        private readonly IFormLocalesAppService _formLocalesAppService;
        private readonly IFormBlocksAppService _formBlocksAppService;
        private readonly IFormBlockFieldsAppService _formBlockFieldsAppService;
        private readonly IFormFieldsAppService _formFieldsAppService;
        private readonly IFormFieldTranslationsAppService _formFieldTranslationsAppService;

        public FormsController(IFormsAppService formsAppService,
            IFormLocalesAppService formLocalesAppService,
            IFormBlocksAppService formBlocksAppService,
            IFormBlockFieldsAppService formBlockFieldsAppService,
            IFormFieldsAppService formFieldsAppService,
            IFormFieldTranslationsAppService formFieldTranslationsAppService
            )
        {
            _formsAppService = formsAppService;
            _formLocalesAppService = formLocalesAppService;
            _formBlocksAppService = formBlocksAppService;
            _formBlockFieldsAppService = formBlockFieldsAppService;
            _formFieldsAppService = formFieldsAppService;
            _formFieldTranslationsAppService = formFieldTranslationsAppService;
        }


        /// <summary>
        /// Loads corresponding view elements based on the value of category
        /// </summary>
        /// <param name="category">
        /// A query string parameter is passed to distinguish and highlight the Form menu item in the navigation panel
        /// </param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string category)
        {
            var forms = await GetAllForms();
            var formLocales = await GetAllFormLocales();

            if (forms == null || formLocales == null)
            {
                return RedirectToRoute("default", new { controller = "Error", action = "E403" });
            }

            FormsViewModel model = null;

            switch(category)
            {
                case FormConsts.Company:
                    {
                        var companyForm = forms.FirstOrDefault(item => item.Form.SystemLevelId == FormConsts.CompanySLId);
                        var companyFormLocales = formLocales.Where(item => item.FormLocale.FormId == companyForm.Form.Id).ToList();

                        model = new FormsViewModel
                        {
                            FormLocale = companyFormLocales,
                        };

                        ViewBag.Category = category;

                        break;
                    }
                case null:
                    {
                        model = new FormsViewModel
                        {
                            FormLocale = formLocales.ToList(),
                        };

                        ViewBag.Category = category;

                        break;
                    }
            }

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Forms_Create, AppPermissions.Pages_Forms_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetFormForEditOutput getFormForEditOutput;

            if (id.HasValue)
            {
                getFormForEditOutput = await _formsAppService.GetFormForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getFormForEditOutput = new GetFormForEditOutput
                {
                    Form = new CreateOrEditFormDto()
                };
            }

            var viewModel = new CreateOrEditFormModalViewModel()
            {
                Form = getFormForEditOutput.Form,
                SystemLevelDescription = getFormForEditOutput.SystemLevelDescription,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFormModal(long id)
        {
            var getFormForViewDto = await _formsAppService.GetFormForView(id);

            var model = new FormViewModel()
            {
                Form = getFormForViewDto.Form
                ,
                SystemLevelDescription = getFormForViewDto.SystemLevelDescription

            };

            return PartialView("_ViewFormModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Forms_Create, AppPermissions.Pages_Forms_Edit)]
        public PartialViewResult SystemLevelLookupTableModal(long? id, string displayName)
        {
            var viewModel = new FormSystemLevelLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FormSystemLevelLookupTableModal", viewModel);
        }

        public async Task<PartialViewResult> DisplayFormLocaleBlocks(long? formLocaleId, string formLocaleText, long localeId, bool editable)
        {
            var mappedFormBlocks = new List<GetFormBlockForViewDto>();
            var mappedFormBlockFieldsPerFormBlock = new List<GetFormBlockFieldForViewDto>();
            var unmappedFormBlocks = new List<GetFormBlockForViewDto>();
            var unmappedFormBlockFieldsPerFormBlock = new List<GetFormBlockFieldForViewDto>();

            var formBlocks = await GetAllFormBlocks();
            var formFields = await GetAllFormFields();

            mappedFormBlocks = formBlocks.Where(item => item.FormBlock.FormLocaleId == formLocaleId && item.FormBlock.SortOrder != 0).OrderBy(b => b.FormBlock.SortOrder).ToList();
            unmappedFormBlocks = formBlocks.Where(item => item.FormBlock.FormLocaleId == formLocaleId && item.FormBlock.SortOrder == 0).OrderBy(b => b.FormBlock.Id).ToList();

            var formFieldTranslations = await GetAllFormFieldTranslations();
            var formFieldTranslationForSelectedLocale = formFieldTranslations.Where(item => item.FormFieldTranslation.LocaleId == localeId);

            var formBlocksFields = await GetAllFormBlockFields(localeId); //already sorted by SortOrder

            if (formLocaleId != null && formLocaleId.HasValue)
            {
                foreach (var formBlock in mappedFormBlocks)
                {
                    var formBlockFields = formBlocksFields.Where(item => item.FormBlockField.FormBlockId == formBlock.FormBlock.Id).OrderBy(item => item.FormBlockField.SortOrder).ToList();

                    foreach (var formBlockField in formBlockFields)
                    {
                        if (formFieldTranslationForSelectedLocale.Count(item => item.FormFieldTranslation.FormFieldId == formBlockField.FormBlockField.FormFieldId) > 0)
                        {
                            formBlockField.FormFieldDescription = formFieldTranslationForSelectedLocale.Where(item => item.FormFieldTranslation.FormFieldId == formBlockField.FormBlockField.FormFieldId).Select(item => item.FormFieldTranslation.Label).FirstOrDefault();
                            formBlockField.FormFieldTranslationId = formFieldTranslationForSelectedLocale.Where(item => item.FormFieldTranslation.FormFieldId == formBlockField.FormBlockField.FormFieldId).Select(item => item.FormFieldTranslation.Id).FirstOrDefault();
                        }
                        else
                        {
                            formBlockField.FormFieldDescription = String.Format("{0} *", formBlockField.FormFieldDescription);
                        }

                        mappedFormBlockFieldsPerFormBlock.Add(formBlockField);
                    }
                }

                foreach (var formBlock in unmappedFormBlocks)
                {
                    var formBlockFields = formBlocksFields.Where(item => item.FormBlockField.FormBlockId == formBlock.FormBlock.Id).OrderBy(item => item.FormBlockField.SortOrder).ToList();

                    foreach (var formBlockField in formBlockFields)
                    {
                        if (formFieldTranslationForSelectedLocale.Count(item => item.FormFieldTranslation.FormFieldId == formBlockField.FormBlockField.FormFieldId) > 0)
                        {
                            formBlockField.FormFieldDescription = formFieldTranslationForSelectedLocale.Where(item => item.FormFieldTranslation.FormFieldId == formBlockField.FormBlockField.FormFieldId).Select(item => item.FormFieldTranslation.Label).FirstOrDefault();
                        }
                        else
                        {
                            formBlockField.FormFieldDescription = String.Format("{0} *", formBlockField.FormFieldDescription);
                        }

                        unmappedFormBlockFieldsPerFormBlock.Add(formBlockField);
                    }
                }
            }
            
            var allFormFieldIds = formFields.Select(f => f.FormField.Id).ToList();
            var mappedFormBlockFieldIds = mappedFormBlockFieldsPerFormBlock.Select(f => f.FormBlockField.FormFieldId).Distinct().ToList();
            var unmappedFormBlockFieldIds = unmappedFormBlockFieldsPerFormBlock.Select(f => f.FormBlockField.FormFieldId).Distinct().ToList();
            var unmappedFormFieldIds = allFormFieldIds.Where(f => !mappedFormBlockFieldIds.Contains(f) && !unmappedFormBlockFieldIds.Contains(f)).ToList();
            
            var unmappedFormFields = new List<GetFormBlockFieldForEditDto>();

            foreach (var unmappedFormFieldId in unmappedFormFieldIds)
            {
                foreach (var formBlock in mappedFormBlocks)
                {
                    var unmappedFormField = await GetFormBlockFieldForEdit(unmappedFormFieldId, formBlock.FormBlock.Id, localeId);

                    unmappedFormField.DropDownDesc = formFieldTranslationForSelectedLocale.Where(item => item.FormFieldTranslation.FormFieldId == unmappedFormFieldId).Select(item => item.FormFieldTranslation.Label).FirstOrDefault() ?? String.Format("{0} *", formFields.Where(f => f.FormField.Id == unmappedFormFieldId).First().FormField.Description);
                    unmappedFormFields.Add(unmappedFormField);
                }
            }

            unmappedFormFields = unmappedFormFields.OrderBy(f => f.DropDownDesc).ToList();

            var model = new FormsViewModel
            {
                FormLocale = formLocaleId != null ? new List<GetFormLocaleForViewDto> { new GetFormLocaleForViewDto { FormLocale = new FormLocaleDto { Description = formLocaleText, Id = formLocaleId.Value } } } : null,
                Locale = new List<GetLocaleForViewDto> { new GetLocaleForViewDto { Locale = new LocaleDto { Id = localeId } } },
                MappedFormBlocks = mappedFormBlocks.OrderBy(item => item.FormBlock.SortOrder).ToList(),
                MappedFormBlockFields = mappedFormBlockFieldsPerFormBlock,
                UnmappedFormBlocks = unmappedFormBlocks,
                UnmappedFormBlockFields = unmappedFormBlockFieldsPerFormBlock,
                UnmappedFormFields = unmappedFormFields,
                Editable = editable 
            };

            return PartialView("_DisplayFormLocaleBlocks", model);
        }

        #region Private methods
        private async Task<List<GetFormForViewDto>> GetAllForms()
        {
            return await _formsAppService.GetAllForms();
        }

        private async Task<List<GetFormLocaleForViewDto>> GetAllFormLocales()
        {
            return await _formLocalesAppService.GetAllFormLocales();
        }

        private async Task<List<GetFormBlockForViewDto>> GetAllFormBlocks()
        {
            return await _formBlocksAppService.GetAllFormBlocks();
        }

        private async Task<List<GetFormBlockFieldForViewDto>> GetAllFormBlockFields(long localeId)
        {
            return await _formBlockFieldsAppService.GetAllFormBlockFields(localeId);
        }

        private async Task<List<GetFormFieldForViewDto>> GetAllFormFields()
        {
            return await _formFieldsAppService.GetAllFormFields();
        }

        private async Task<List<GetFormFieldTranslationForViewDto>> GetAllFormFieldTranslations()
        {
            return await _formFieldTranslationsAppService.GetAllFormFieldTranslations();
        }

        private async Task<GetFormBlockFieldForEditDto> GetFormBlockFieldForEdit(long fieldId, long blockId, long localeId)
        {
            return await _formBlockFieldsAppService.GetFormBlockFieldForEdit(fieldId, blockId, localeId);
        }
        #endregion
    }
}