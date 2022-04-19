using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.ValueLists;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ValueLists)]
    public class ValueListsController : RMSControllerBase
    {
        private readonly IValueListsAppService _valueListsAppService;

        public ValueListsController(IValueListsAppService valueListsAppService)
        {
            _valueListsAppService = valueListsAppService;
        }

        public ActionResult Index()
        {
            var model = new ValueListsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ValueLists_Create, AppPermissions.Pages_ValueLists_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetValueListForEditOutput getValueListForEditOutput;

				if (id.HasValue){
					getValueListForEditOutput = await _valueListsAppService.GetValueListForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getValueListForEditOutput = new GetValueListForEditOutput{
						ValueList = new CreateOrEditValueListDto()
					};
				}

				var viewModel = new CreateOrEditValueListModalViewModel()
				{
					ValueList = getValueListForEditOutput.ValueList,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewValueListModal(long id)
        {
			var getValueListForViewDto = await _valueListsAppService.GetValueListForView(id);

            var model = new ValueListViewModel()
            {
                ValueList = getValueListForViewDto.ValueList
            };

            return PartialView("_ViewValueListModal", model);
        }


    }
}