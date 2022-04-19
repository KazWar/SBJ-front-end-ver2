using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using RMS.Authorization;
using RMS.Web.Controllers;
using RMS.Web.Areas.App.Models.Campaigns;
using RMS.Web.Areas.App.Models.CampaignTypes;
using RMS.Web.Areas.App.Models.CampaignCampaignTypes;
using RMS.SBJ.Forms;
using RMS.SBJ.Forms.Dtos;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Web.Models;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using Newtonsoft.Json;
using RMS.SBJ.Registrations.Helpers;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Campaigns)]
    public class CampaignsController : RMSControllerBase
    {
        private readonly ICampaignsAppService _campaignsAppService;
		private readonly ICampaignCampaignTypesAppService _campaignCampaignTypesAppService;
		private readonly ICampaignTypesAppService _campaignTypesAppService;
		private readonly ICampaignTypeEventsAppService _campaignTypeEventsAppService;
		private readonly ICampaignTypeEventRegistrationStatusesAppService _campaignTypeEventRegistrationStatusesAppService;
		private readonly ICampaignMessagesAppService _campaignMessagesAppService;
		private readonly ICampaignFormsAppService _campaignFormsAppService;
		private readonly IFormLocalesAppService _formLocalesAppService;
		private readonly IFormsAppService _formsAppService;
		private readonly IFormBlocksAppService _formBlocksAppService;
		private readonly IFormBlockFieldsAppService _formBlockFieldsAppService;
		private readonly ILocalesAppService _localesAppService;
		private readonly IMessageComponentContentsAppService _messageComponentContentsAppService;
		private readonly IMessageVariablesAppService _messageVariablesAppService;
		private readonly IMessageComponentsAppService _messageComponentsAppService;
		private readonly IMessageComponentTypesAppService _messageComponentTypesAppService;
		private readonly IMessageTypesAppService _messageTypesAppService;
		private readonly IMessageContentTranslationsAppService _messageContentTranslationsAppService;

		public CampaignsController(ICampaignsAppService campaignsAppService, ICampaignCampaignTypesAppService campaignCampaignTypesAppService,
			ICampaignTypesAppService campaignTypesAppService, ICampaignFormsAppService campaignFormsAppService,
			ICampaignTypeEventsAppService campaignTypeEventsAppService,
			ICampaignTypeEventRegistrationStatusesAppService campaignTypeEventRegistrationStatusesAppService,
			ICampaignMessagesAppService campaignMessagesAppService,
			IFormLocalesAppService formLocalesAppService, IFormsAppService formsAppService,
			IFormBlocksAppService formBlocksAppService,
			IFormBlockFieldsAppService formBlockFieldsAppService,
			ILocalesAppService localesAppService,
			IMessageComponentContentsAppService messageComponentContentsAppService,
			IMessageVariablesAppService messageVariablesAppService,
			IMessageComponentsAppService messageComponentsAppService,
			IMessageComponentTypesAppService messageComponentTypesAppService,
			IMessageTypesAppService messageTypesAppService,
			IMessageContentTranslationsAppService messageContentTranslationsAppService
			)
        {
            _campaignsAppService = campaignsAppService;
			_campaignCampaignTypesAppService = campaignCampaignTypesAppService;
			_campaignTypesAppService = campaignTypesAppService;
			_campaignTypeEventsAppService = campaignTypeEventsAppService;
			_campaignTypeEventRegistrationStatusesAppService = campaignTypeEventRegistrationStatusesAppService;
			_campaignMessagesAppService = campaignMessagesAppService;
			_campaignFormsAppService = campaignFormsAppService;
			_formLocalesAppService = formLocalesAppService;
			_formsAppService = formsAppService;
			_formBlocksAppService = formBlocksAppService;
			_formBlockFieldsAppService = formBlockFieldsAppService;
			_localesAppService = localesAppService;
			_messageComponentContentsAppService = messageComponentContentsAppService;
			_messageContentTranslationsAppService = messageContentTranslationsAppService;
			_messageVariablesAppService = messageVariablesAppService;
			_messageComponentsAppService = messageComponentsAppService;
			_messageComponentTypesAppService = messageComponentTypesAppService;
			_messageTypesAppService = messageTypesAppService;
			
		}

        public ActionResult Index()
        {
            var model = new CampaignsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Campaigns_Create, AppPermissions.Pages_Campaigns_Edit)]
        public async Task<ViewResult> CreateOrEdit(long? id)
        {
            GetCampaignForEditOutput getCampaignForEditOutput;

            if (id.HasValue)
            {
                getCampaignForEditOutput = await _campaignsAppService.GetCampaignForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getCampaignForEditOutput = new GetCampaignForEditOutput
                {
                    Campaign = new CreateOrEditCampaignDto()
                    {
                        StartDate = DateTime.Today,
                        EndDate = DateTime.Today
                    }
                };

                var newCampaignCode = await _campaignsAppService.GetSuggestedNewCampaignCode();

                getCampaignForEditOutput.Campaign.CampaignCode = newCampaignCode;
            }

            var getAllCampaignCampaignTypeEntities = await GetAllCampaignCampaignTypes();
            var getCampaignCampaignTypePerCampaign = getAllCampaignCampaignTypeEntities.Where(item => item.CampaignCampaignType.CampaignId == id);
            var selectedCampaignTypeIds = getCampaignCampaignTypePerCampaign.Select(i => i.CampaignCampaignType.CampaignTypeId).ToList();
         
            var getAllActiveCampaignTypeEntities = await GetAllActiveCampaignType();
            var campaignTypeViewModelList = getAllActiveCampaignTypeEntities.Items.Select(item => new CampaignTypeViewModel { CampaignType = new CampaignTypeDto { Name = item.CampaignType.Name, Id = item.CampaignType.Id } }).ToList();

            var getAllActiveLocalesOnCompanyLevel = await GetAllActiveLocalesOnCompanyLevel();

            var campaignTypeViewModel = campaignTypeViewModelList.Select(i => new CampaignCampaignTypeMultiSelectModel
            {
                CampaignId = getCampaignForEditOutput.Campaign.Id,
                CampaignTypeId = i.CampaignType.Id,
                CampaignTypeName = i.CampaignType.Name,
                CampaignCampaignTypeId = getCampaignCampaignTypePerCampaign.Where(item => item.CampaignCampaignType.CampaignTypeId == i.CampaignType.Id).Select(i => i.CampaignCampaignType.Id).FirstOrDefault(),
                IsSelected = selectedCampaignTypeIds.Contains(i.CampaignType.Id)
            }).ToList();
           
            var viewModel = new CreateOrEditCampaignViewModel()
            {
                Campaign = getCampaignForEditOutput.Campaign,
                CampaignTypeViewModelList = campaignTypeViewModel,
                SelectableLocales = getAllActiveLocalesOnCompanyLevel
            };

            return View(viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Campaigns_Create, AppPermissions.Pages_Campaigns_Edit)]
        public async Task<ViewResult> Duplicate(long id)
        {
            var getCampaignToDuplicateFrom = await _campaignsAppService.GetCampaignForEdit(new EntityDto<long> { Id = (long)id });
            var getCampaignToDuplicateFromOutput = new GetCampaignForEditOutput
            {
                Campaign = new CreateOrEditCampaignDto()
                {
                    Id = getCampaignToDuplicateFrom.Campaign.Id,
                    Name = getCampaignToDuplicateFrom.Campaign.Name ?? String.Empty,
                    Description = getCampaignToDuplicateFrom.Campaign.Description ?? String.Empty,
                    ExternalCode = getCampaignToDuplicateFrom.Campaign.ExternalCode ?? String.Empty,
                    ExternalId = getCampaignToDuplicateFrom.Campaign.ExternalId ?? String.Empty,
                    ThumbnailImagePath = getCampaignToDuplicateFrom.Campaign.ThumbnailImagePath ?? String.Empty,
                    BannerImagePath = getCampaignToDuplicateFrom.Campaign.BannerImagePath ?? String.Empty,
                    CampaignCode = getCampaignToDuplicateFrom.Campaign.CampaignCode,
                    StartDate = getCampaignToDuplicateFrom.Campaign.StartDate,
                    EndDate = getCampaignToDuplicateFrom.Campaign.EndDate 
                }
            };

            var getCampaignToDuplicateToOutput = new GetCampaignForEditOutput
            {
                Campaign = new CreateOrEditCampaignDto()
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today
                }
            };

            var newCampaignCode = await _campaignsAppService.GetSuggestedNewCampaignCode();

            getCampaignToDuplicateToOutput.Campaign.CampaignCode = newCampaignCode;

            var viewModel = new CreateOrEditCampaignViewModel() 
            {
                Campaign = getCampaignToDuplicateToOutput.Campaign,
                DuplicateFrom = getCampaignToDuplicateFromOutput.Campaign 
            };

            return View(viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Campaigns_Create, AppPermissions.Pages_Campaigns_Edit)]
        public async Task<ViewResult> CheckCompanyAlert()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CreateCampaign([FromBody]CreateCampaignViewModel viewModel)
        {
            try 
            {
                if (viewModel == null) throw new ArgumentException("Parameter is Null", "viewModel");

                var campaignId = await CreateCampaignAndGetId(viewModel.Campaign);

                if (viewModel?.Campaign?.Id == null)
                {
                    #region CampaignForm
                    var latestFormId = viewModel?.CampaignFormViewModel?.CampaignForm?.FormId == 0 ? await _campaignsAppService.GetLatestFormIdForCampaign() : viewModel?.CampaignFormViewModel?.CampaignForm?.FormId;
                    var campaignForm = new CreateOrEditCampaignFormDto()
                    {
                        CampaignId = campaignId,
                        FormId = latestFormId.Value,
                        IsActive = true
                    };
                    await _campaignFormsAppService.CreateOrEdit(campaignForm);
                    #endregion

                    #region CampaignMessage
                    var latestMessageId = viewModel?.CampaignMessageViewModel?.CampaignMessage?.MessageId == 0 ? await _campaignsAppService.GetLatestMessageIdForCampaign() : viewModel?.CampaignMessageViewModel?.CampaignMessage?.MessageId;
                    var campaignMessage = new CreateOrEditCampaignMessageDto()
                    {
                        CampaignId = campaignId,
                        MessageId = latestMessageId.Value,
                        IsActive = true
                    };
                    await _campaignMessagesAppService.CreateOrEdit(campaignMessage);
                    #endregion
                }
                
                foreach (var selectedCampaignType in viewModel?.SelectedCampaignCampaignType)
                {
                    var campaignCampaignType = new CreateOrEditCampaignCampaignTypeDto()
                    { 
                        CampaignId = campaignId,
                        Id = selectedCampaignType.CampaignCampaignTypeId,
                        CampaignTypeId = selectedCampaignType.CampaignTypeId.Value,
                        Description = selectedCampaignType.CampaignTypeName
                    };
                    await _campaignCampaignTypesAppService.CreateOrEdit(campaignCampaignType);
                }

                foreach (var unselectedCampaignType in viewModel?.UnselectedCampaignCampaignType)
                {
                    if (unselectedCampaignType.CampaignCampaignTypeId != 0 && !unselectedCampaignType.IsSelected)
                    {
                        await _campaignCampaignTypesAppService.Delete(new EntityDto<long> { Id = unselectedCampaignType.CampaignCampaignTypeId.Value });
                    }
                }

                return Json(true);
            }
            catch (Exception exception)
            {
                return Json(exception);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCampaignLocales()
        {
            var campaignLocales = await _campaignsAppService.GetCampaignLocales();
            return Json(campaignLocales);
        }

        [HttpGet]
        public async Task<ActionResult> GetCampaignForForm(string currentLocale, long currentCampaignCode)
        {
            var campaignForForm = await _campaignsAppService.GetCampaignForForm(currentLocale, currentCampaignCode);
            return Json(campaignForForm);
        }

        public async Task<PartialViewResult> ViewCampaignModal(long id)
        {
            var getCampaignForViewDto = await _campaignsAppService.GetCampaignForView(id);

            var model = new CampaignViewModel()
            {
                Campaign = getCampaignForViewDto.Campaign
            };

            return PartialView("_ViewCampaignModal", model);
        }

		public async Task<ActionResult> CampaignOverview(long campaignId, bool editable)
		{
			var getCampaignForViewDto = await _campaignsAppService.GetCampaignForView(campaignId);
			var getAllCampaignCampaignTypeEntities = await GetAllCampaignCampaignTypes();
			var campaignCampaignTypePerCampaign = getAllCampaignCampaignTypeEntities.Where(item => item.CampaignCampaignType.CampaignId == campaignId).ToList();
			var model = new CampaignViewModel()
			{
				Campaign = getCampaignForViewDto.Campaign,
				CampaignCampaignType = campaignCampaignTypePerCampaign,
                Editable = editable 
			};
			return View(model);
		}

        public async Task<PartialViewResult> CampaignForms(long campaignId, bool editable)
        {
            var getAllCampaignFormEntities = await GetAllCampaignForms();
            var campaignCampaignForm = getAllCampaignFormEntities.Where(item => item.CampaignForm.CampaignId == campaignId).LastOrDefault();
            var formLocaleEntities = await _formLocalesAppService.GetAllFormLocales();
            var campaignFormLocale = formLocaleEntities.Where(item => item.FormLocale.FormId == campaignCampaignForm?.CampaignForm?.FormId).ToList();
            var model = new CampaignViewModel()
            {
                CampaignFormLocales = campaignFormLocale,
                Editable = editable 
            };

            return PartialView("_CampaignForms", model);
        }

        public async Task<PartialViewResult> CampaignMessages(long campaignId)
        {
            var getCampaignForViewDto = await _campaignsAppService.GetCampaignForView(campaignId);

            //NOTE: Requires change in schema to display locales for messages. Currently displaying all locales
            //TO DO: Update the campaign messages locales based on new schema
            #region Locales
            var allLocales = await GetAllActiveLocales();
            #endregion

            var model = new CampaignViewModel()
            {
                Campaign = getCampaignForViewDto.Campaign,
                Locales = allLocales,
            };

            return PartialView("_CampaignMessages", model);
        }

        /// <summary>
        /// TO DO: Need to consolidate all the service GetAll methods at the beginning of the function
        /// Currently all variables are initialised under regions. Remove regions after code development
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> PopulateMessageComponentContent(string messageLocaleText, long localeId, long campaignId)
        {
            var selectedCampaignCampaignTypeEvents = new List<GetCampaignTypeEventForViewDto>();
            var selectedCampaignCampaignTypeEventRegistrationStatuses = new List<GetCampaignTypeEventRegistrationStatusForViewDto>();
            var getCampaignForViewDto = await _campaignsAppService.GetCampaignForView(campaignId);

            #region Campaign Message
            var allCampaignMessages = await GetAllCampaignMessages();
            var selectedCampaignCampaignMessage = allCampaignMessages.Where(item => item.CampaignMessage.CampaignId == campaignId).LastOrDefault();
            #endregion

            #region Campaign Campaign Type and Campaign Type Events
            var allCampaignCampaignTypes = await GetAllCampaignCampaignTypes();
            var allCampaignTypeEvents = await GetAllCampaignTypeEvents();

            var selectedCampaignCampaignType = allCampaignCampaignTypes.Where(item => item.CampaignCampaignType.CampaignId == campaignId).ToList();
            foreach (var campaignType in selectedCampaignCampaignType)
            {
                var selectedCampaignTypeEvents = allCampaignTypeEvents.Items.Where(item => item.CampaignTypeEvent.CampaignTypeId == campaignType.CampaignCampaignType.CampaignTypeId).ToList();
                selectedCampaignCampaignTypeEvents.AddRange(selectedCampaignTypeEvents);
            }
            #endregion

            #region Campaign Type Event Registration Status
            var getAllCampaignTypeEventRegistrationStatuses = await GetAllCampaignTypeEventRegistrationStatus();
            foreach (var cte in selectedCampaignCampaignTypeEvents)
            {
                var selectedCampaignTypeEventRegistrationStatuses = getAllCampaignTypeEventRegistrationStatuses.Items.Where(item => item.CampaignTypeEventRegistrationStatus.CampaignTypeEventId == cte.CampaignTypeEvent.Id).ToList();
                selectedCampaignCampaignTypeEventRegistrationStatuses.AddRange(selectedCampaignTypeEventRegistrationStatuses);
            }
            #endregion

            #region Campaign Message Type
            var getAllMessageType = await GetAllMessageTypes();
            var getCampaignMessageType = new PagedResultDto<GetMessageTypeForViewDto> { Items = getAllMessageType.Where(item => item.MessageType.MessageId == selectedCampaignCampaignMessage?.CampaignMessage?.MessageId).ToList() };
            #endregion

            #region Campaign Message Component
            var getAllMessageComponents = await GetAllMessageComponents();
            var getAllMessageComponentContents = await GetAllMessageComponentContents();
            var getAllMessageContentTranslations = await GetAllMessageContentTranslations();
            var selectedLocaleMessageContentTranslations = getAllMessageContentTranslations.Where(item => item.MessageContentTranslation.LocaleId == localeId).ToList();
            var selectedCampaignMessageComponents = new List<GetMessageComponentForViewDto>();

            foreach (var cmt in getCampaignMessageType.Items)
            {
                var campaignMessageComponents = getAllMessageComponents.Items.Where(item => item.MessageComponent.MessageTypeId == cmt.MessageType.Id).ToList();
                selectedCampaignMessageComponents.AddRange(campaignMessageComponents);
            }
            #endregion

            var model = new CampaignViewModel()
            {
                Locales = new List<GetLocaleForViewDto> { new GetLocaleForViewDto { Locale = new LocaleDto { Id = localeId, Description = messageLocaleText } } },
                Campaign = getCampaignForViewDto.Campaign,
                CampaignTypeEvents = new PagedResultDto<GetCampaignTypeEventForViewDto>(selectedCampaignCampaignTypeEvents.Count, selectedCampaignCampaignTypeEvents),
                CampaignTypeEventRegistrationStatuses = new PagedResultDto<GetCampaignTypeEventRegistrationStatusForViewDto>(selectedCampaignCampaignTypeEventRegistrationStatuses.Count, selectedCampaignCampaignTypeEventRegistrationStatuses),
                MessageComponents = new PagedResultDto<GetMessageComponentForViewDto>(selectedCampaignMessageComponents.Count, selectedCampaignMessageComponents),
                MessageComponentContents = new PagedResultDto<GetMessageComponentContentForViewDto>(getAllMessageComponentContents.Count, getAllMessageComponentContents),
                MessageContentTranslations = new PagedResultDto<GetMessageContentTranslationForViewDto>(selectedLocaleMessageContentTranslations.Count, selectedLocaleMessageContentTranslations),
                MessageTypes = getCampaignMessageType,
                MessageVariables = await GetAllMessageVariables()
            };
            return PartialView("_CampaignMessageComponentContent", model);
        }

        public PartialViewResult CampaignFormCompanyLookupTableModal()
        {
            var viewModel = new CampaignFormCompanyLookupTableViewModel
            {

            };
            return PartialView("_CampaignFormCompanyLookupTableModal", viewModel);
        }

		public async Task<JsonResult> CampaignFormFromCompany(long formLocaleId, long localeId)
		{
			var companyFormBlocks = new List<GetFormBlockForViewDto>();
			var formLocales = await _formLocalesAppService.GetAllFormLocales();
			var forms = await _formsAppService.GetAllForms();
			var formBlocks = await _formBlocksAppService.GetAllFormBlocks();
			var formBlockFields = await _formBlockFieldsAppService.GetAllFormBlockFields(null);
			var latestCompanyForm = forms.Where(item => item.Form.SystemLevelId == FormConsts.CompanySLId).LastOrDefault();
			var companyFormLocales = formLocales.Where(item => item.FormLocale.LocaleId == localeId && item.FormLocale.FormId == latestCompanyForm.Form.Id).ToList();

			foreach (var companyFormLocale in companyFormLocales)
			{
				var companyFormBlockItem = formBlocks.Where(item => item.FormBlock.FormLocaleId == companyFormLocale.FormLocale.Id);
				companyFormBlocks.AddRange(companyFormBlockItem);
			}

			try
			{
				if (companyFormBlocks?.Count > 0)
				{
					foreach (var cfb in companyFormBlocks)
					{
						var companyFormBlockFields = formBlockFields.Where(item => item.FormBlockField.FormBlockId == cfb.FormBlock.Id).ToList();
						var campaignFormBlock = new CreateOrEditFormBlockDto { Description = cfb.FormBlock.Description, FormLocaleId = formLocaleId, IsPurchaseRegistration = cfb.FormBlock.IsPurchaseRegistration, SortOrder = cfb.FormBlock.SortOrder };
						var campaignFormBlockId = await _formBlocksAppService.CreateOrEditAndGetId(campaignFormBlock);

						foreach (var cfbf in companyFormBlockFields)
						{
							var campaignFormBlockField = new CreateOrEditFormBlockFieldDto {FormBlockId = campaignFormBlockId, FormFieldId = cfbf.FormBlockField.FormFieldId, SortOrder = cfbf.FormBlockField.SortOrder };
							await _formBlockFieldsAppService.CreateOrEdit(campaignFormBlockField);
						}
					}
					return Json(true);
				}
				return Json(true);
			}
			catch (Exception e)
			{
				return Json(e);
			}
			
		}

        [HttpPost]
        [DontWrapResult]
        public async Task<JsonResult> GetCampaignDropdownContent([FromBody] GetActiveCampaignsOnlyInput input)
        {
            var campaigns = await _campaignsAppService.GetAllCampaignsForDropdown(input.ActiveCampaignsOnly);

            return Json(campaigns);
        }

        [HttpGet]
        [DontWrapResult]
        public async Task<JsonResult> GetCampaignLocalesApi()
        {
            var campaignLocales = await _campaignsAppService.GetCampaignLocales();

            return Json(campaignLocales);
        }

        [HttpGet]
        [DontWrapResult]
        public async Task<JsonResult> GetCampaignOverviewApi(string currentLocale)
        {
            var campaignOverview = await _campaignsAppService.GetCampaignOverview(currentLocale);

            return Json(campaignOverview);
        }

        [HttpGet]
        [DontWrapResult]
        public async Task<JsonResult> GetCampaignForFormApi(string currentLocale, long currentCampaignCode)
        {
            var campaignForForm = await _campaignsAppService.GetCampaignForForm(currentLocale, currentCampaignCode);

            return Json(campaignForForm);
        }

        #region Private Methods - all methods Added for Message component content
        private async Task<PagedResultDto<GetMessageVariableForViewDto>> GetAllMessageVariables()
        {
            // No need to filter by anything as there isn't any IsActive filter present on this entity.
            return await _messageVariablesAppService.GetAll(new GetAllMessageVariablesInput { Filter = "" });
        }
        #endregion

        #region Private methods

        private async Task<List<GetCampaignFormForViewDto>> GetAllCampaignForms()
        {
            return await _campaignFormsAppService.GetAllCampaignForms();
        }

        private async Task<List<GetLocaleForViewDto>> GetAllActiveLocales()
        {
            var allLocales = await _localesAppService.GetAllLocales();
            return allLocales.Where(item => item.Locale.IsActive).ToList();
        }

        private async Task<List<GetLocaleForViewDto>> GetAllActiveLocalesOnCompanyLevel()
        {
            var allLocales = await _localesAppService.GetAllLocalesOnCompanyLevel();
            return allLocales.Where(item => item.Locale.IsActive).ToList();
        }

        private async Task<List<GetCampaignCampaignTypeForViewDto>> GetAllCampaignCampaignTypes()
        {
            return await _campaignCampaignTypesAppService.GetAllCampaignCampaignTypes();
        }

        private async Task<PagedResultDto<GetCampaignTypeEventForViewDto>> GetAllCampaignTypeEvents()
        {
            return await _campaignTypeEventsAppService.GetAll();
        }

        private async Task<PagedResultDto<GetCampaignTypeEventRegistrationStatusForViewDto>> GetAllCampaignTypeEventRegistrationStatus()
        {
            return await _campaignTypeEventRegistrationStatusesAppService.GetAllCampaignTypeEventRegistrationStatus();
        }

        private async Task<List<GetMessageTypeForViewDto>> GetAllMessageTypes()
        {
            return await _messageTypesAppService.GetAllMessageTypes();
        }

        private async Task<List<GetCampaignMessageForViewDto>> GetAllCampaignMessages()
        {
            return await _campaignMessagesAppService.GetAllCampaignMessage();
        }

        private async Task<PagedResultDto<GetMessageComponentForViewDto>> GetAllMessageComponents()
        {
            return await _messageComponentsAppService.GetAllMessageComponents();
        }

        private async Task<List<GetMessageComponentContentForViewDto>> GetAllMessageComponentContents()
        {
            return await _messageComponentContentsAppService.GetAllMessageComponentContents();
        }

        private async Task<List<GetMessageContentTranslationForViewDto>> GetAllMessageContentTranslations()
        {
            return await _messageContentTranslationsAppService.GetAllMessageContentTranslations();
        }

		private async Task<PagedResultDto<GetCampaignTypeForViewDto>> GetAllActiveCampaignType()
		{ 
			return await _campaignTypesAppService.GetAllActiveCampaignType();
		}

        private async Task<long> CreateCampaignAndGetId(CreateOrEditCampaignDto campaign)
        {
            if (campaign?.Id == null || campaign?.Id == 0)
            {
                return await _campaignsAppService.CreateOrEditAndGetId(campaign);
            }
            else return campaign?.Id != null ? campaign.Id.Value : default;
        }
		#endregion
	}
}