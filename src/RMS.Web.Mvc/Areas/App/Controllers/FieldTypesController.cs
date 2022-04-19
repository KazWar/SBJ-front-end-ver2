using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.FieldTypes;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_FieldTypes)]
    public class FieldTypesController : RMSControllerBase
    {
        private readonly IFieldTypesAppService _fieldTypesAppService;

        public FieldTypesController(IFieldTypesAppService fieldTypesAppService)
        {
            _fieldTypesAppService = fieldTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new FieldTypesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_FieldTypes_Create, AppPermissions.Pages_FieldTypes_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetFieldTypeForEditOutput getFieldTypeForEditOutput;

				if (id.HasValue){
					getFieldTypeForEditOutput = await _fieldTypesAppService.GetFieldTypeForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getFieldTypeForEditOutput = new GetFieldTypeForEditOutput{
						FieldType = new CreateOrEditFieldTypeDto()
					};
				}

				var viewModel = new CreateOrEditFieldTypeModalViewModel()
				{
					FieldType = getFieldTypeForEditOutput.FieldType,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewFieldTypeModal(long id)
        {
			var getFieldTypeForViewDto = await _fieldTypesAppService.GetFieldTypeForView(id);

            var model = new FieldTypeViewModel()
            {
                FieldType = getFieldTypeForViewDto.FieldType
            };

            return PartialView("_ViewFieldTypeModal", model);
        }


    }
}