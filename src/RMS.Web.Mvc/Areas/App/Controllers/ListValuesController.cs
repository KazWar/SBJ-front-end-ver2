using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.ListValues;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ListValues)]
    public class ListValuesController : RMSControllerBase
    {
        private readonly IListValuesAppService _listValuesAppService;

        public ListValuesController(IListValuesAppService listValuesAppService)
        {
            _listValuesAppService = listValuesAppService;
        }

        public ActionResult Index()
        {
            var model = new ListValuesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ListValues_Create, AppPermissions.Pages_ListValues_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetListValueForEditOutput getListValueForEditOutput;

				if (id.HasValue){
					getListValueForEditOutput = await _listValuesAppService.GetListValueForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getListValueForEditOutput = new GetListValueForEditOutput{
						ListValue = new CreateOrEditListValueDto()
					};
				}

				var viewModel = new CreateOrEditListValueModalViewModel()
				{
					ListValue = getListValueForEditOutput.ListValue,
					ValueListDescription = getListValueForEditOutput.ValueListDescription,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewListValueModal(long id)
        {
			var getListValueForViewDto = await _listValuesAppService.GetListValueForView(id);

            var model = new ListValueViewModel()
            {
                ListValue = getListValueForViewDto.ListValue
                , ValueListDescription = getListValueForViewDto.ValueListDescription 

            };

            return PartialView("_ViewListValueModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ListValues_Create, AppPermissions.Pages_ListValues_Edit)]
        public PartialViewResult ValueListLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ListValueValueListLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ListValueValueListLookupTableModal", viewModel);
        }

    }
}