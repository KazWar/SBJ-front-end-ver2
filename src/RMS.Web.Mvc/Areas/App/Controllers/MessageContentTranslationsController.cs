using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.MessageContentTranslations;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_MessageContentTranslations)]
    public class MessageContentTranslationsController : RMSControllerBase
    {
        private readonly IMessageContentTranslationsAppService _messageContentTranslationsAppService;

        public MessageContentTranslationsController(IMessageContentTranslationsAppService messageContentTranslationsAppService)
        {
            _messageContentTranslationsAppService = messageContentTranslationsAppService;
        }

        public ActionResult Index()
        {
            var model = new MessageContentTranslationsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_MessageContentTranslations_Create, AppPermissions.Pages_MessageContentTranslations_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetMessageContentTranslationForEditOutput getMessageContentTranslationForEditOutput;

				if (id.HasValue){
					getMessageContentTranslationForEditOutput = await _messageContentTranslationsAppService.GetMessageContentTranslationForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getMessageContentTranslationForEditOutput = new GetMessageContentTranslationForEditOutput{
						MessageContentTranslation = new CreateOrEditMessageContentTranslationDto()
					};
				}

				var viewModel = new CreateOrEditMessageContentTranslationModalViewModel()
				{
					MessageContentTranslation = getMessageContentTranslationForEditOutput.MessageContentTranslation,
					LocaleDescription = getMessageContentTranslationForEditOutput.LocaleDescription,
					MessageComponentContentContent = getMessageContentTranslationForEditOutput.MessageComponentContentContent,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewMessageContentTranslationModal(long id)
        {
			var getMessageContentTranslationForViewDto = await _messageContentTranslationsAppService.GetMessageContentTranslationForView(id);

            var model = new MessageContentTranslationViewModel()
            {
                MessageContentTranslation = getMessageContentTranslationForViewDto.MessageContentTranslation
                , LocaleDescription = getMessageContentTranslationForViewDto.LocaleDescription 

                , MessageComponentContentContent = getMessageContentTranslationForViewDto.MessageComponentContentContent 

            };

            return PartialView("_ViewMessageContentTranslationModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MessageContentTranslations_Create, AppPermissions.Pages_MessageContentTranslations_Edit)]
        public PartialViewResult LocaleLookupTableModal(long? id, string displayName)
        {
            var viewModel = new MessageContentTranslationLocaleLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_MessageContentTranslationLocaleLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MessageContentTranslations_Create, AppPermissions.Pages_MessageContentTranslations_Edit)]
        public PartialViewResult MessageComponentContentLookupTableModal(long? id, string displayName)
        {
            var viewModel = new MessageContentTranslationMessageComponentContentLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_MessageContentTranslationMessageComponentContentLookupTableModal", viewModel);
        }


    }
}