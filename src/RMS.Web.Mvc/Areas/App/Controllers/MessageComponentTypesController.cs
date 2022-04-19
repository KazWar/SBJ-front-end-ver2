using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.MessageComponentTypes;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_MessageComponentTypes)]
    public class MessageComponentTypesController : RMSControllerBase
    {
        private readonly IMessageComponentTypesAppService _messageComponentTypesAppService;

        public MessageComponentTypesController(IMessageComponentTypesAppService messageComponentTypesAppService)
        {
            _messageComponentTypesAppService = messageComponentTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new MessageComponentTypesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_MessageComponentTypes_Create, AppPermissions.Pages_MessageComponentTypes_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetMessageComponentTypeForEditOutput getMessageComponentTypeForEditOutput;

				if (id.HasValue){
					getMessageComponentTypeForEditOutput = await _messageComponentTypesAppService.GetMessageComponentTypeForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getMessageComponentTypeForEditOutput = new GetMessageComponentTypeForEditOutput{
						MessageComponentType = new CreateOrEditMessageComponentTypeDto()
					};
				}

				var viewModel = new CreateOrEditMessageComponentTypeModalViewModel()
				{
					MessageComponentType = getMessageComponentTypeForEditOutput.MessageComponentType,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewMessageComponentTypeModal(long id)
        {
			var getMessageComponentTypeForViewDto = await _messageComponentTypesAppService.GetMessageComponentTypeForView(id);

            var model = new MessageComponentTypeViewModel()
            {
                MessageComponentType = getMessageComponentTypeForViewDto.MessageComponentType
            };

            return PartialView("_ViewMessageComponentTypeModal", model);
        }


    }
}