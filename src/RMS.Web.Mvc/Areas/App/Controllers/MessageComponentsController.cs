using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.MessageComponents;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_MessageComponents)]
    public class MessageComponentsController : RMSControllerBase
    {
        private readonly IMessageComponentsAppService _messageComponentsAppService;

        public MessageComponentsController(IMessageComponentsAppService messageComponentsAppService)
        {
            _messageComponentsAppService = messageComponentsAppService;
        }

        public ActionResult Index()
        {
            var model = new MessageComponentsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_MessageComponents_Create, AppPermissions.Pages_MessageComponents_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetMessageComponentForEditOutput getMessageComponentForEditOutput;

				if (id.HasValue){
					getMessageComponentForEditOutput = await _messageComponentsAppService.GetMessageComponentForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getMessageComponentForEditOutput = new GetMessageComponentForEditOutput{
						MessageComponent = new CreateOrEditMessageComponentDto()
					};
				}

				var viewModel = new CreateOrEditMessageComponentModalViewModel()
				{
					MessageComponent = getMessageComponentForEditOutput.MessageComponent,
					MessageTypeName = getMessageComponentForEditOutput.MessageTypeName,
					MessageComponentTypeName = getMessageComponentForEditOutput.MessageComponentTypeName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewMessageComponentModal(long id)
        {
			var getMessageComponentForViewDto = await _messageComponentsAppService.GetMessageComponentForView(id);

            var model = new MessageComponentViewModel()
            {
                MessageComponent = getMessageComponentForViewDto.MessageComponent
                , MessageTypeName = getMessageComponentForViewDto.MessageTypeName 

                , MessageComponentTypeName = getMessageComponentForViewDto.MessageComponentTypeName 

            };

            return PartialView("_ViewMessageComponentModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MessageComponents_Create, AppPermissions.Pages_MessageComponents_Edit)]
        public PartialViewResult MessageTypeLookupTableModal(long? id, string displayName)
        {
            var viewModel = new MessageComponentMessageTypeLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_MessageComponentMessageTypeLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_MessageComponents_Create, AppPermissions.Pages_MessageComponents_Edit)]
        public PartialViewResult MessageComponentTypeLookupTableModal(long? id, string displayName)
        {
            var viewModel = new MessageComponentMessageComponentTypeLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_MessageComponentMessageComponentTypeLookupTableModal", viewModel);
        }

    }
}