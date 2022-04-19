using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.ListValueTranslations;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ListValueTranslations)]
    public class ListValueTranslationsController : RMSControllerBase
    {
        private readonly IListValueTranslationsAppService _listValueTranslationsAppService;

        public ListValueTranslationsController(IListValueTranslationsAppService listValueTranslationsAppService)
        {
            _listValueTranslationsAppService = listValueTranslationsAppService;
        }

        public ActionResult Index()
        {
            var model = new ListValueTranslationsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ListValueTranslations_Create, AppPermissions.Pages_ListValueTranslations_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetListValueTranslationForEditOutput getListValueTranslationForEditOutput;

				if (id.HasValue){
					getListValueTranslationForEditOutput = await _listValueTranslationsAppService.GetListValueTranslationForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getListValueTranslationForEditOutput = new GetListValueTranslationForEditOutput{
						ListValueTranslation = new CreateOrEditListValueTranslationDto()
					};
				}

				var viewModel = new CreateOrEditListValueTranslationModalViewModel()
				{
					ListValueTranslation = getListValueTranslationForEditOutput.ListValueTranslation,
					ListValueKeyValue = getListValueTranslationForEditOutput.ListValueKeyValue,
					LocaleLanguageCode = getListValueTranslationForEditOutput.LocaleLanguageCode,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewListValueTranslationModal(long id)
        {
			var getListValueTranslationForViewDto = await _listValueTranslationsAppService.GetListValueTranslationForView(id);

            var model = new ListValueTranslationViewModel()
            {
                ListValueTranslation = getListValueTranslationForViewDto.ListValueTranslation
                , ListValueKeyValue = getListValueTranslationForViewDto.ListValueKeyValue 

                , LocaleLanguageCode = getListValueTranslationForViewDto.LocaleLanguageCode 

            };

            return PartialView("_ViewListValueTranslationModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ListValueTranslations_Create, AppPermissions.Pages_ListValueTranslations_Edit)]
        public PartialViewResult ListValueLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ListValueTranslationListValueLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ListValueTranslationListValueLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_ListValueTranslations_Create, AppPermissions.Pages_ListValueTranslations_Edit)]
        public PartialViewResult LocaleLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ListValueTranslationLocaleLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ListValueTranslationLocaleLookupTableModal", viewModel);
        }

    }
}