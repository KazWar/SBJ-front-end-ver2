using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.MessageTypes;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_MessageTypes)]
    public class MessageTypesController : RMSControllerBase
    {
        private readonly IMessageTypesAppService _messageTypesAppService;

        public MessageTypesController(IMessageTypesAppService messageTypesAppService)
        {
            _messageTypesAppService = messageTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new MessageTypesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_MessageTypes_Create, AppPermissions.Pages_MessageTypes_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetMessageTypeForEditOutput getMessageTypeForEditOutput;

				if (id.HasValue){
					getMessageTypeForEditOutput = await _messageTypesAppService.GetMessageTypeForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getMessageTypeForEditOutput = new GetMessageTypeForEditOutput{
						MessageType = new CreateOrEditMessageTypeDto()
					};
				}

				var viewModel = new CreateOrEditMessageTypeModalViewModel()
				{
					MessageType = getMessageTypeForEditOutput.MessageType,
					MessageVersion = getMessageTypeForEditOutput.MessageVersion,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewMessageTypeModal(long id)
        {
			var getMessageTypeForViewDto = await _messageTypesAppService.GetMessageTypeForView(id);

            var model = new MessageTypeViewModel()
            {
                MessageType = getMessageTypeForViewDto.MessageType
                , MessageVersion = getMessageTypeForViewDto.MessageVersion 

            };

            return PartialView("_ViewMessageTypeModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MessageTypes_Create, AppPermissions.Pages_MessageTypes_Edit)]
        public PartialViewResult MessageLookupTableModal(long? id, string displayName)
        {
            var viewModel = new MessageTypeMessageLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_MessageTypeMessageLookupTableModal", viewModel);
        }

    }
}