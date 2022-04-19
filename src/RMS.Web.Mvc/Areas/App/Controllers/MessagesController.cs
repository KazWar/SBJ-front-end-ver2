using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.Messages;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.SystemTables;
using RMS.SBJ.SystemTables.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Messages)]
    public class MessagesController : RMSControllerBase
    {
        private readonly IMessagesAppService _messagesAppService;

        public MessagesController(IMessagesAppService messagesAppService)
        {
            _messagesAppService = messagesAppService;
        }

        public ActionResult Index()
        {
            var model = new MessagesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Messages_Create, AppPermissions.Pages_Messages_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetMessageForEditOutput getMessageForEditOutput;

				if (id.HasValue){
					getMessageForEditOutput = await _messagesAppService.GetMessageForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getMessageForEditOutput = new GetMessageForEditOutput{
						Message = new CreateOrEditMessageDto()
					};
				}

				var viewModel = new CreateOrEditMessageModalViewModel()
				{
					Message = getMessageForEditOutput.Message,
					SystemLevelDescription = getMessageForEditOutput.SystemLevelDescription,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewMessageModal(long id)
        {
			var getMessageForViewDto = await _messagesAppService.GetMessageForView(id);

            var model = new MessageViewModel()
            {
                Message = getMessageForViewDto.Message
                , SystemLevelDescription = getMessageForViewDto.SystemLevelDescription 

            };

            return PartialView("_ViewMessageModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Messages_Create, AppPermissions.Pages_Messages_Edit)]
        public PartialViewResult SystemLevelLookupTableModal(long? id, string displayName)
        {
            var viewModel = new MessageSystemLevelLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_MessageSystemLevelLookupTableModal", viewModel);
        }
    }
}