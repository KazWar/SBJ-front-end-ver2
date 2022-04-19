using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.FormBlocks;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using System.Linq;
using System.Collections.Generic;
using RMS.Web.Areas.App.Models.FormBlockFields;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_FormBlocks)]
    public class FormBlocksController : RMSControllerBase
    {
        private readonly IFormBlocksAppService _formBlocksAppService;
        private readonly IFormFieldsAppService _formFieldsAppService;
        private readonly IFormBlockFieldsAppService _formBlockFieldsAppService;

        public FormBlocksController(IFormBlocksAppService formBlocksAppService,
            IFormFieldsAppService formFieldsAppService,
            IFormBlockFieldsAppService formBlockFieldsAppService)
        {
            _formBlocksAppService = formBlocksAppService;
            _formFieldsAppService = formFieldsAppService;
            _formBlockFieldsAppService = formBlockFieldsAppService;
        }

        public ActionResult Index()
        {
            var model = new FormBlocksViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_FormBlocks_Create, AppPermissions.Pages_FormBlocks_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetFormBlockForEditOutput getFormBlockForEditOutput;

				if (id.HasValue){
					getFormBlockForEditOutput = await _formBlocksAppService.GetFormBlockForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getFormBlockForEditOutput = new GetFormBlockForEditOutput{
						FormBlock = new CreateOrEditFormBlockDto()
					};
				}

				var viewModel = new CreateOrEditFormBlockModalViewModel()
				{
					FormBlock = getFormBlockForEditOutput.FormBlock,
					FormLocaleDescription = getFormBlockForEditOutput.FormLocaleDescription,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewFormBlockModal(long id)
        {
			var getFormBlockForViewDto = await _formBlocksAppService.GetFormBlockForView(id);

            var model = new FormBlockViewModel()
            {
                FormBlock = getFormBlockForViewDto.FormBlock
                , FormLocaleDescription = getFormBlockForViewDto.FormLocaleDescription 

            };

            return PartialView("_ViewFormBlockModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FormBlocks_Create, AppPermissions.Pages_FormBlocks_Edit)]
        public PartialViewResult FormLocaleLookupTableModal(long? id, string displayName)
        {
            var viewModel = new FormBlockFormLocaleLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FormBlockFormLocaleLookupTableModal", viewModel);
        }

        public async Task<PartialViewResult> ChooseFormFieldsModal(long id, string description)
        {
            var unSelectedFormBlockFields = new List<GetFormFieldForViewDto>();

            var allFormFields = await _formFieldsAppService.GetAllFormFields();
            var allFormBlockFields = await _formBlockFieldsAppService.GetAllFormBlockFields(null);

            #region Retrieve already selected FormFieldIds (if any) of Form Block from FormBlockField 

            /* FormFieldId property is of nullable long type. So have used below two LINQ queries to get FormFieldIds of already associated FormFields for the selected FormBlock*/
            var selectedFormFields = allFormBlockFields.Where(item => item.FormBlockField.FormBlockId == id).Select(item => item.FormBlockField.FormFieldId);
            var selectedFormFieldsId = selectedFormFields.Where(item => item.HasValue).Select(item => item.Value).ToList();

            #endregion

            var allFormFieldsId = allFormFields.Select(item => item.FormField.Id).ToList();
            var unSelectedFormFieldsId = allFormFieldsId.Except(selectedFormFieldsId);

            foreach (var unselectedId in unSelectedFormFieldsId)
            {
                var unselectedFormFieldItem = allFormFields.Where(item => item.FormField.Id == unselectedId).Select(item => item).FirstOrDefault();
                unSelectedFormBlockFields.Add(unselectedFormFieldItem);
            }

            var selectedFormBlockFields = allFormBlockFields.Where(fbfItem => fbfItem.FormBlockField.FormBlockId == id).OrderBy(fbfItem => fbfItem.FormBlockField.SortOrder).ToList();

            var viewModel = new FormBlockViewModel()
            {
                FormBlock = new FormBlockDto { Id = id, Description = description },
                UnSelectedFormBlockFields = unSelectedFormBlockFields,
                SelectedFormBlockFields = selectedFormBlockFields
            };

            ViewBag.Description = description;
            return PartialView("_ChooseFormFieldsModal", viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateFormBlockFields([FromBody] UpdateFormBlockFieldsViewModel viewModel)
        {
            var allFormBlockFields = await _formBlockFieldsAppService.GetAllFormBlockFields(null);
            var formBlockFieldsInDBPerFormBlock = allFormBlockFields.Where(item => item.FormBlockField.FormBlockId == viewModel.FormBlockId).ToList();
            try
            {
                foreach (var formField in viewModel.SelectedFormFields)
                {
                    formField.Id = formBlockFieldsInDBPerFormBlock.Where(item => item.FormBlockField.FormFieldId == formField.FormFieldId && item.FormBlockField.FormBlockId == formField.FormBlockId).Select(item => item.FormBlockField.Id).FirstOrDefault();
                }

                if (formBlockFieldsInDBPerFormBlock.Count() > viewModel.SelectedFormFields.Count())
                {
                    foreach (var formField in viewModel.AvailableFormFields)
                    {
                        formField.Id = formBlockFieldsInDBPerFormBlock.Where(item => item.FormBlockField.FormFieldId == formField.FormFieldId && item.FormBlockField.FormBlockId == formField.FormBlockId).Select(item => item.FormBlockField.Id).FirstOrDefault();
                        //Delete the deselected form block field
                        if (formField.Id != null && formField.Id.HasValue)
                            await _formBlockFieldsAppService.Delete(new EntityDto<long>(formField.Id.Value));
                    }
                }

                //Create new form block fields or update existing form block fields's order
                foreach (var formField in viewModel.SelectedFormFields)
                {
                    await _formBlockFieldsAppService.CreateOrEdit(formField);
                }
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }
    }
}