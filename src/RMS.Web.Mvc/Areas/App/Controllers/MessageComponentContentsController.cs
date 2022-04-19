using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.MessageComponentContents;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.SBJ.CodeTypeTables;
using System.Collections.Generic;
using Abp.Runtime.Validation;
using RMS.SBJ.SystemTables;
using System.Linq;
using Abp.Extensions;
using PayPalCheckoutSdk.Orders;
using NPOI.SS.Formula.Functions;
using Microsoft.Extensions.Azure;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Azure.Storage.Blobs;
using System.IO;
using RMS.Web.Models.AzureBlobStorageForMessages;
using Abp.MultiTenancy;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_MessageComponentContents)]
    public class MessageComponentContentsController : RMSControllerBase
    {
        private readonly IMessagesAppService _messagesAppService;
        private readonly IMessageTypesAppService _messageTypesAppService;
        private readonly IMessageComponentsAppService _messageComponentsAppService;
        private readonly IMessageComponentTypesAppService _messageComponentTypesAppService;
        private readonly IMessageComponentContentsAppService _messageComponentContentsAppService;
        private readonly IMessageContentTranslationsAppService _messageContentTranslationsAppService;
        private readonly IMessageVariablesAppService _messageVariablesAppService;
        private readonly ICampaignTypeEventsAppService _campaignTypeEventsAppService;
        private readonly ICampaignTypeEventRegistrationStatusesAppService _campaignTypeEventRegistrationStatusesAppService;
        private readonly ILocalesAppService _localesAppService;
        private readonly AzureBlobSettingsMessageModel _azureBlobSettingsMessageModel;
        private readonly ITenantCache _tenantCache;

        public MessageComponentContentsController(
            IMessagesAppService messagesAppService,
            IMessageTypesAppService messageTypesAppService,
            IMessageComponentsAppService messageComponentsAppService,
            IMessageComponentTypesAppService messageComponentTypesAppService,
            IMessageComponentContentsAppService messageComponentContentsAppService,
            IMessageContentTranslationsAppService messageContentTranslationsAppService,
            IMessageVariablesAppService messageVariablesAppService,
            ICampaignTypeEventsAppService campaignTypeEventsAppService,
            ICampaignTypeEventRegistrationStatusesAppService campaignTypeEventRegistrationStatusesAppService,
            ILocalesAppService localesAppService,
            AzureBlobSettingsMessageModel azureBlobSettingsMessageModel, 
            ITenantCache tenantCache)
        {
            _messagesAppService = messagesAppService;
            _messageTypesAppService = messageTypesAppService;
            _messageComponentsAppService = messageComponentsAppService;
            _messageComponentTypesAppService = messageComponentTypesAppService;
            _messageComponentContentsAppService = messageComponentContentsAppService;
            _messageContentTranslationsAppService = messageContentTranslationsAppService;
            _messageVariablesAppService = messageVariablesAppService;
            _campaignTypeEventsAppService = campaignTypeEventsAppService;
            _campaignTypeEventRegistrationStatusesAppService = campaignTypeEventRegistrationStatusesAppService;
            _localesAppService = localesAppService;
            _azureBlobSettingsMessageModel = azureBlobSettingsMessageModel;
            _tenantCache = tenantCache;
        }

        public async Task<ActionResult> Index()
        {
            var model = new MessageComponentContentsViewModel
            {
                FilterText = "",
                Locales = await GetAllActiveLocales()
            };
            return View(model);
        }

        public async Task<PagedResultDto<GetMessageComponentForViewDto>> GetMessageComponentsByMessageTypeId(long messageTypeId, string messageTypeName, long messageId)
        {
            var listing = await _messageComponentsAppService.GetMessageComponentsByMessageTypeId(messageTypeId, messageTypeName, messageId);
            if (listing?.TotalCount == 0)
            {
                return null;
            }
            return listing;
        }

        /// <summary>
        /// Gets a record of entity type <see cref="GetMessageComponentContentForEditOutput"/>.
        /// </summary>
        /// <param name="id">The id of <see cref="MessageComponentContent"/>.</param>
        /// <returns>The relevant <see cref="MessageComponentContent"/>.</returns>
        [HttpGet]
        public async Task<GetMessageComponentContentForEditOutput> GetMessageComponentContentById(long id)
        {
            var item = await _messageComponentContentsAppService.GetMessageComponentContentForEdit(new EntityDto<long> { Id = id });
            return item;
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MessageComponentContents_Create, AppPermissions.Pages_MessageComponentContents_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetMessageComponentContentForEditOutput getMessageComponentContentForEditOutput;

            if (id.HasValue)
            {
                getMessageComponentContentForEditOutput = await _messageComponentContentsAppService.GetMessageComponentContentForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getMessageComponentContentForEditOutput = new GetMessageComponentContentForEditOutput
                {
                    MessageComponentContent = new CreateOrEditMessageComponentContentDto()
                };
            }

            var viewModel = new CreateOrEditMessageComponentContentModalViewModel()
            {
                MessageComponentContent = getMessageComponentContentForEditOutput.MessageComponentContent,
                MessageComponentIsActive = getMessageComponentContentForEditOutput.MessageComponentIsActive,
                CampaignTypeEventRegistrationStatusSortOrder = getMessageComponentContentForEditOutput.CampaignTypeEventRegistrationStatusSortOrder,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MessageComponentContents_Create)]
        public async Task<PartialViewResult> CreateEntityModal() 
        {
            var viewModel = new CreateOrEditMessageComponentContentModalEntityViewModel()
            {
                Messaging = await PopulateCampaignTypeEventRegistrationStatus(),
                MessageTypes = await GetCompanyMessageTypes()
            };

            return PartialView("_CreateEntityModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MessageComponentContents_Edit)]
        public async Task<PartialViewResult> EditEntityModal(long? id, string messageType)
        {
            GetMessageComponentContentForEditOutput getMessageComponentContentForEditOutput;

            if (id.HasValue)
                getMessageComponentContentForEditOutput = await _messageComponentContentsAppService.GetMessageComponentContentForEdit(new EntityDto<long> { Id = (long)id });
            else
            {
                getMessageComponentContentForEditOutput = new GetMessageComponentContentForEditOutput
                {
                    MessageComponentContent = new CreateOrEditMessageComponentContentDto()
                };
            }
            var viewModel = new CreateOrEditMessageComponentContentModalEntityViewModel()
            {
                MessageComponentContent = getMessageComponentContentForEditOutput.MessageComponentContent,
            };
            ViewBag.MessageType = messageType;
            return PartialView("_EditEntityModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MessageComponentContents_Edit)]
        public async Task<JsonResult> EditMessageComponentContentTranslation(string messageType, long? id, long? campaignTypeEventRegistrationStatusId, long? localeId = null)
        {
            GetMessageComponentContentForEditOutput getMessageComponentContentForEditOutput;
            var allMessageComponentContentsPerCampaignTypeEventRegistrationStatusId = new List<GetMessageComponentContentForViewDto>();
            var selectedMessageContentTranslation = new List<GetMessageContentTranslationForViewDto>();
            var messageComponents = await GetAllMessageComponents(true);

            if (id.HasValue)
            {
                getMessageComponentContentForEditOutput = await _messageComponentContentsAppService.GetMessageComponentContentForEdit(new EntityDto<long> { Id = (long)id });
                var allMessageComponentContents = await GetAllMessageComponentContents();

                var messageComponentContentsPerCampaignTypeEventRegistrationStatusId = allMessageComponentContents.Items
                    .Where(item => item.MessageComponentContent.CampaignTypeEventRegistrationStatusId == campaignTypeEventRegistrationStatusId)
                    .ToList();
                foreach (var messageComponentContent in messageComponentContentsPerCampaignTypeEventRegistrationStatusId)
                {
                    var linkedMessageComponent = messageComponents.Items
                        .Where(mcItem => mcItem.MessageComponent.Id == messageComponentContent.MessageComponentContent.MessageComponentId)
                        .Select(mcItem => new { messageTypeName = mcItem.MessageTypeName, messageComponentTypeName = mcItem.MessageComponentTypeName })
                        .LastOrDefault();
                    messageComponentContent.MessageType = linkedMessageComponent?.messageTypeName;
                    messageComponentContent.MessageComponentType = linkedMessageComponent?.messageComponentTypeName;
                    if (messageComponentContent.MessageType.ToLower() == messageType.ToLower())
                    {
                        allMessageComponentContentsPerCampaignTypeEventRegistrationStatusId.Add(messageComponentContent);
                    }
                }
                if (localeId.HasValue && localeId.Value != 0)
                {
                    var getAllMessageContentTranslations = await GetAllMessageContentTranslation();
                    //selectedMessageContentTranslation = getAllMessageContentTranslations.Where(item => item.MessageContentTranslation.MessageComponentContentId == id && item.MessageContentTranslation.LocaleId == localeId.Value).LastOrDefault();
                    foreach (var messageComponentContentPerCampaignTypeEventRegistrationStatusId in allMessageComponentContentsPerCampaignTypeEventRegistrationStatusId)
                    {
                        selectedMessageContentTranslation.Add(getAllMessageContentTranslations.Where(item => item.MessageContentTranslation.MessageComponentContentId == messageComponentContentPerCampaignTypeEventRegistrationStatusId.MessageComponentContent.Id && item.MessageContentTranslation.LocaleId == localeId.Value).LastOrDefault());
                    }
                }
            }
            else
            {
                getMessageComponentContentForEditOutput = new GetMessageComponentContentForEditOutput
                {
                    MessageComponentContent = new CreateOrEditMessageComponentContentDto()
                };
            }

            var viewModel = new CreateOrEditMessageComponentContentModalEntityViewModel()
            {
                MessageComponentContent = getMessageComponentContentForEditOutput.MessageComponentContent,
                MessageComponentIsActive = getMessageComponentContentForEditOutput.MessageComponentIsActive,
                CampaignTypeEventRegistrationStatusSortOrder = getMessageComponentContentForEditOutput.CampaignTypeEventRegistrationStatusSortOrder,
                MessageComponentContentTranslation = selectedMessageContentTranslation,
                LocaleId = localeId ?? 0,
                MessageComponentContentsPerCampaignTypeEventRegistrationStatusId = allMessageComponentContentsPerCampaignTypeEventRegistrationStatusId
            };
            return Json(viewModel);
        }

        [HttpPut]
        [DisableValidation]
        public async Task<JsonResult> EditMessageComponentContentTranslation([FromBody] SaveExistingMessageComponentContentsViewModel viewModel)
        {
            if (viewModel.MessageComponentContentId <= 0) throw new Exception($"MessageComponentContentId is invalid. Value: ${viewModel.MessageComponentContentId}");

            try
            {
                if (viewModel.LocaleId != 0)
                {
                    if (viewModel.Content.Contains("<img"))
                    {
                        var documentList = UploadMessagingImage(
                            new ComponentEditor { Content = viewModel.Content, MessageComponentId = viewModel.MessageComponentId } , 
                            viewModel.CampaignTypeEventRegistrationStatusId
                            );
                        if (documentList?.Count() > 0)
                        {
                            foreach (var document in documentList)
                            {
                                await _messageContentTranslationsAppService.CreateOrEdit(new CreateOrEditMessageContentTranslationDto
                                {
                                    Id = viewModel.MessageContentTranslationId,
                                    MessageComponentContentId = viewModel.MessageComponentContentId,
                                    Content = document?.DocumentNode?.InnerHtml,
                                    LocaleId = viewModel.LocaleId
                                });
                            }
                        }
                        else 
                        {
                            await _messageContentTranslationsAppService.CreateOrEdit(new CreateOrEditMessageContentTranslationDto
                            {
                                Id = viewModel.MessageContentTranslationId,
                                MessageComponentContentId = viewModel.MessageComponentContentId,
                                Content = viewModel.Content,
                                LocaleId = viewModel.LocaleId
                            });
                        }
                    }
                    else 
                    {
                        await _messageContentTranslationsAppService.CreateOrEdit(new CreateOrEditMessageContentTranslationDto
                        {
                            Id = viewModel.MessageContentTranslationId,
                            MessageComponentContentId = viewModel.MessageComponentContentId,
                            Content = viewModel.Content,
                            LocaleId = viewModel.LocaleId
                        });
                    }
                    return Json(true);
                }
                else
                {
                    throw new Exception($"LocaleId is invalid. Value: ${viewModel.LocaleId}");
                }
            }
            catch (Exception exception)
            {
                return Json(exception);
            }
        }

        public async Task<PartialViewResult> ViewMessageComponentContentModal(long id)
        {
            var getMessageComponentContentForViewDto = await _messageComponentContentsAppService.GetMessageComponentContentForView(id);

            var model = new MessageComponentContentViewModel()
            {
                MessageComponentContent = getMessageComponentContentForViewDto.MessageComponentContent,
                MessageComponentIsActive = getMessageComponentContentForViewDto.MessageComponentIsActive,
                CampaignTypeEventRegistrationStatusSortOrder = getMessageComponentContentForViewDto.CampaignTypeEventRegistrationStatusSortOrder
            };

            return PartialView("_ViewMessageComponentContentModal", model);
        }

        public async Task<PagedResultDto<GetMessageComponentContentForViewDto>> GetMessageComponentContentsForCompany()
        {
            var companyMessageComponentList = new List<GetMessageComponentForViewDto>();
            var companyMessageComponentContent = new List<GetMessageComponentContentForViewDto>();

            var allMessageComponentContents = await _messageComponentContentsAppService.GetAllMessageComponentContents();

            #region Messages
            var allMessages = await _messagesAppService.GetAllMessages();
            var companyMessages = allMessages.Where(item => item.Message.SystemLevelId == MessageConsts.CompanySLId).LastOrDefault();
            #endregion

            #region Message Types
            var allMessageTypes = await _messageTypesAppService.GetAllMessageTypes();
            var companyMessageTypes = allMessageTypes.Where(item => item.MessageType.MessageId == companyMessages.Message.Id).ToList();
            #endregion

            #region Message Component
            var allMessageComponents = await GetAllMessageComponents(true);
            foreach (var companyMessageTypeItem in companyMessageTypes)
            {
                var companyMessageComponent = allMessageComponents.Items.Where(item => item.MessageTypeName == companyMessageTypeItem.MessageType.Name).ToList();
                companyMessageComponentList.AddRange(companyMessageComponent);
            }

            foreach (var companyMessageComponent in companyMessageComponentList)
            {
                var companyMessageComponentContentList = allMessageComponentContents.Where(item => item.MessageComponentContent.MessageComponentId == companyMessageComponent.MessageComponent.Id).ToList();
                companyMessageComponentContent.AddRange(companyMessageComponentContentList);
            }
            #endregion

            return new PagedResultDto<GetMessageComponentContentForViewDto> { Items = companyMessageComponentContent.OrderBy(item => item.MessageComponentContent.CampaignTypeEventRegistrationStatusId).ToList(), TotalCount = companyMessageComponentContent.Count };
        }

        public async Task<PartialViewResult> PopulateMessageComponentContent(long? localeId)
        {
            var allMessageContentTranslation = await GetAllMessageContentTranslation();
            var selectedLocaleMessageContentTranslation = allMessageContentTranslation.Where(item => item.MessageContentTranslation.LocaleId == localeId.Value).ToList();
            var model = new MessageComponentContentsViewModel
            {
                FilterText = "",
                CampaignTypeEvents = await GetAllCampaignTypeEvents(),
                CampaignTypeEventRegistrationStatuses = await GetAllCampaignTypeEventRegistrationStatuses(),
                MessageComponents = await GetAllMessageComponents(true),
                MessageTypes = await GetCompanyMessageTypes(),
                MessageComponentTypes = await GetAllMessageComponentTypes(),
                MessageComponentContents = await GetMessageComponentContentsForCompany(),
                Locales = await GetAllActiveLocales(),
                MessageContentTranslations = selectedLocaleMessageContentTranslation,
                LocaleId = localeId.Value
            };

            return PartialView("_MessageComponentContent", model);
        }

        public async Task<PartialViewResult> ChooseMessageType(long? campaignTypeEventRegistrationStatusId)
        {
            var messageComponents = await GetAllMessageComponents(true);
            var allMessageComponentIds = messageComponents.Items.Select(item => item.MessageComponent.Id);
            var allMessageComponentContents = await GetAllMessageComponentContents();
            var mappedMessageComponentId = allMessageComponentContents.Items.Where(item => item.MessageComponentContent.CampaignTypeEventRegistrationStatusId == campaignTypeEventRegistrationStatusId.Value).Select(item => item.MessageComponentContent.MessageComponentId);
            var unmappedMessageComponentId = allMessageComponentIds.Except(mappedMessageComponentId);

            var messageTypes = messageComponents.Items.Join(unmappedMessageComponentId, mc => mc.MessageComponent.Id, umc => umc, (mc, umc) => mc.MessageComponent.MessageTypeId).Distinct();

            var messageTypeList = new List<GetMessageTypeForViewDto>();
            foreach (var mt in messageTypes)
            {
                var messageTypeItem = await _messageTypesAppService.GetMessageTypeForView(mt);
                var messageTypeItemMessageId = messageTypeItem.MessageType.MessageId;
                var companyMessageType = await _messagesAppService.GetMessageForView(messageTypeItemMessageId);
                if (companyMessageType.SystemLevelDescription.ToLower() == MessageConsts.Company.ToLower())
                {
                    messageTypeList.Add(messageTypeItem);
                }
            }

            var viewModel = new CreateOrEditMessageComponentContentModalEntityViewModel
            {
                MessageTypes = new PagedResultDto<GetMessageTypeForViewDto> { TotalCount = messageTypeList.Count, Items = messageTypeList }
            };

            return PartialView("_ChooseMessageType", viewModel);
        }

        [HttpPost]
        [DisableValidation]
        public async Task<ActionResult> AddMessageComponentContents([FromBody] SaveMessageComponentContentsViewModel viewModel)
        {
            
            /// Concept is as follows:
            /// User gets presented a list of available registration status events to choose from, e.g. "Accepted", "Rejected"
            /// User gets presented a list of campaign types and process events based on the chosen registration status event
            /// User then chooses whether he wants to set the contents for an e-mail, letter or whatsapp
            /// User then gets to set the header, footer and body texts          

            // if (viewModel.MessageComponentIds == null) throw new Exception($"MessageComponentId is invalid. Values: ${viewModel.MessageComponentIds}");
            if (viewModel.CampaignTypeEventRegistrationStatusId <= 0) throw new Exception($"CampaignTypeRegistrationStatusId is invalid. Value: ${viewModel.CampaignTypeEventRegistrationStatusId}");

            foreach (var messageComponent in viewModel.MessageComponentDictionary)
            {
                if (messageComponent.Content.Contains("<img"))
                {
                    var documentList = UploadMessagingImage(messageComponent, viewModel.CampaignTypeEventRegistrationStatusId);
                    foreach (var document in documentList)
                    {
                        await _messageComponentContentsAppService.CreateOrEdit(new CreateOrEditMessageComponentContentDto
                        {
                            CampaignTypeEventRegistrationStatusId = viewModel.CampaignTypeEventRegistrationStatusId,
                            MessageComponentId = Convert.ToInt64(messageComponent.MessageComponentId),
                            Content = document?.DocumentNode?.InnerHtml
                        });
                    }
                }
                else
                {
                    await _messageComponentContentsAppService.CreateOrEdit(new CreateOrEditMessageComponentContentDto
                    {
                        CampaignTypeEventRegistrationStatusId = viewModel.CampaignTypeEventRegistrationStatusId,
                        MessageComponentId = Convert.ToInt64(messageComponent.MessageComponentId),
                        Content = messageComponent.Content
                    });
                }
            }
            return RedirectToAction("Index", "MessageComponentContents");
        }

        /// <summary>
        /// Gets all records of entity of type <see cref="CampaignTypeEventRegistrationStatus"/>.
        /// </summary>
        /// <returns>A listing of type <see cref="PagedResultDto{T}"/></returns>
        private async Task<PagedResultDto<GetCampaignTypeEventRegistrationStatusForViewDto>> GetAllCampaignTypeEventRegistrationStatuses()
        {
            return await _campaignTypeEventRegistrationStatusesAppService.GetAllCampaignTypeEventRegistrationStatus();
        }

        /// <summary>
        /// Gets all records of entity type <see cref="MessageComponent" />.
        /// </summary>
        /// <param name="filterByIsActive">Whether or not to filter by property <c>IsActive</c>.</param>
        /// <returns>A list of type <see cref="PagedResultDto{T}" /></returns>
        private async Task<PagedResultDto<GetMessageComponentForViewDto>> GetAllMessageComponents(bool filterByIsActive = false)
        {
            return await _messageComponentsAppService.GetAll(new GetAllMessageComponentsInput { IsActiveFilter = Convert.ToInt32(filterByIsActive) });
        }

        /// <summary>
        /// Gets all records of entity type <see cref="MessageType" /> where <c>IsActive</c> is set to <c>1</c>.
        /// </summary>
        /// <returns>A list of type <see cref="PagedResultDto{T}" /></returns>
        private async Task<PagedResultDto<GetMessageTypeForViewDto>> GetAllActiveMessageTypes()
        {
            return await _messageTypesAppService.GetAll(new GetAllMessageTypesInput { IsActiveFilter = 1 });
        }

        private async Task<PagedResultDto<GetMessageTypeForViewDto>> GetCompanyMessageTypes()
        {
            var allMessages = await _messagesAppService.GetAllMessages();
            var companyMessages = allMessages.Where(item => item.Message.SystemLevelId == MessageConsts.CompanySLId).LastOrDefault();
            var allMessageTypes = await GetAllActiveMessageTypes();

            var companyMessageTypes = allMessageTypes.Items.Where(item => item.MessageType.MessageId == companyMessages.Message.Id).ToList();
            return new PagedResultDto<GetMessageTypeForViewDto> {TotalCount = companyMessageTypes.Count(), Items = companyMessageTypes };
        }

        /// <summary>
        /// Gets all records of entity type <see cref="MessageComponentContent"/>.
        /// </summary>
        /// <returns>A list of type <see cref="PagedResultDto{T}"/></returns>
        private async Task<PagedResultDto<GetMessageComponentContentForViewDto>> GetAllMessageComponentContents(bool filterByIsActive = false)
        {
            var list = await _messageComponentContentsAppService.GetAll(new GetAllMessageComponentContentsInput { Filter = "" });
            return list;
        }

        /// <summary>
        /// Gets all records of entity type <see cref="CampaignTypeEvent"/>.
        /// </summary>
        /// <returns>A list of type <see cref="PagedResultDto{T}"/></returns>
        private async Task<PagedResultDto<GetCampaignTypeEventForViewDto>> GetAllCampaignTypeEvents()
        {
            var list = await _campaignTypeEventsAppService.GetAll(new GetAllCampaignTypeEventsInput { Filter = "" });
            return list;
        }

        private async Task<List<GetMessageContentTranslationForViewDto>> GetAllMessageContentTranslation()
        {
            return await _messageContentTranslationsAppService.GetAllMessageContentTranslations();
        }

        private async Task<List<GetLocaleForViewDto>> GetAllActiveLocales()
        {
            var allLocales = await _localesAppService.GetAllLocales();
            return allLocales.Where(item => item.Locale.IsActive).ToList();
        }

        private async Task<List<GetMessageComponentTypeForViewDto>> GetAllMessageComponentTypes()
        {
            return await _messageComponentTypesAppService.GetAllMessageComponentTypes();
        }

        private async Task<IReadOnlyCollection<MessagingViewModel>> PopulateCampaignTypeEventRegistrationStatus()
        {
            var messagingViewModelList = new List<MessagingViewModel>();

            //TO DO: Quite tricky to resolve MessageTypeId if messageTypes are newly assigned at the company/campaign level as the table - [DBO.MessageType] functions both as codetype and linking.
            //Need to change in future.
            #region Populate CampaignTypeEventRegistrationStatuses when there is no selection for all message Types
            var allMessageTypes = await GetAllActiveMessageTypes();
            var companyMessageTypes = await GetCompanyMessageTypes(); //MessageTypes assigned at company level
            var companyMessageTypeNames = companyMessageTypes.Items.Select(cmt => cmt.MessageType.Name); //Getting all the Names of the MessageType as unable to resolve the messageTypes based on MessageTypeId

            var templateMessageTypeIds = new List<long>(); //Considering first two records as the template/CodeType records, retrieving Ids based on Name property
            foreach (var cmtn in companyMessageTypeNames)
            {
                templateMessageTypeIds.Add(allMessageTypes.Items.Where(i => i.MessageType.Name == cmtn).Select(i => i.MessageType.Id).FirstOrDefault());
            }

            var messageComponents = await GetAllMessageComponents(true);
            var messageComponentMessageType = messageComponents.Items.Join(templateMessageTypeIds, mc => mc.MessageComponent.MessageTypeId, tmt => tmt, (mc, tmt) => mc.MessageComponent.Id).ToList();
            #endregion

            // Get all campaign type event registration statuses
            var campaignTypeEventRegistrationStatuses = await _campaignTypeEventRegistrationStatusesAppService.GetAllCampaignTypeEventRegistrationStatus();

            var allMessageComponentContents = await _messageComponentContentsAppService.GetAllMessageComponentContents();
            var lookupMCC = allMessageComponentContents.ToLookup(mcc => mcc.MessageComponentContent.CampaignTypeEventRegistrationStatusId).OrderBy(mcc => mcc.Key);
            var groupedMCC = allMessageComponentContents.GroupBy(item => item.MessageComponentContent.CampaignTypeEventRegistrationStatusId).ToList();

            var unmappedCTERSId = new List<long>();
            foreach (var mcc in lookupMCC)
            {
                if (mcc.Count() != messageComponentMessageType.Count())
                {
                    unmappedCTERSId.Add(mcc.Key);
                }
            }
            var groupedKeys = lookupMCC.Select(lmcc => lmcc.Key);
            var allCampaignTypeEventRegistrationStatusId = campaignTypeEventRegistrationStatuses.Items.Select(item => item.CampaignTypeEventRegistrationStatus.Id);
            var mappedCampaignTypeEventRegistrationStatusId = allMessageComponentContents.Select(item => item.MessageComponentContent.CampaignTypeEventRegistrationStatusId).Distinct().ToList();
            var otherCampaignTypeEventRegistrationStatusId = allCampaignTypeEventRegistrationStatusId.Except(mappedCampaignTypeEventRegistrationStatusId).ToList();
            if (unmappedCTERSId.Any())
                otherCampaignTypeEventRegistrationStatusId.AddRange(unmappedCTERSId);

            var otherCampaignTypeEventRegistrationStatuses = campaignTypeEventRegistrationStatuses.Items.Join(otherCampaignTypeEventRegistrationStatusId,
                cters => cters.CampaignTypeEventRegistrationStatus.Id, octers => octers,
                (cters, octers) => new { campaignTypeEventId = cters.CampaignTypeEventRegistrationStatus.CampaignTypeEventId, CampaignTypeEventRegistrationStatus = cters.CampaignTypeEventRegistrationStatus, RegistrationStatusDescription = cters.RegistrationStatusDescription });

            if (campaignTypeEventRegistrationStatuses.TotalCount <= 0) return null; // There are no items to loop through, so we can't get campaign type events based on the reg status

            foreach (var campaignTypeEventRegistrationStatus in otherCampaignTypeEventRegistrationStatuses)
            {
                var relatedCampaignTypeEvent = await _campaignTypeEventsAppService.GetByIdAsync(campaignTypeEventRegistrationStatus.campaignTypeEventId);
                if (relatedCampaignTypeEvent == null) continue;

                messagingViewModelList.Add(new MessagingViewModel
                {
                    CampaignTypeEvent = relatedCampaignTypeEvent,
                    CampaignTypeEventRegistrationStatus = new GetCampaignTypeEventRegistrationStatusForViewDto { CampaignTypeEventRegistrationStatus = campaignTypeEventRegistrationStatus.CampaignTypeEventRegistrationStatus },
                    RegistrationStatusDescription = campaignTypeEventRegistrationStatus.RegistrationStatusDescription
                });
            }

            return messagingViewModelList.AsReadOnly(); // returning the List<T> with a ReadOnlyCollection<T> wrapper
        }

        private List<HtmlDocument> UploadMessagingImage(ComponentEditor messageComponent, long campaignTypeEventRegistrationStatusId)
        {
            var document = new HtmlDocument();
            var documentList = new List<HtmlDocument>();
            var blobStorage = _azureBlobSettingsMessageModel.RMSBlobStorage;
            var blobContainer = AbpSession.TenantId.HasValue ? _tenantCache.GetOrNull(AbpSession.TenantId.Value).TenancyName.ToLower() : _azureBlobSettingsMessageModel.DefaultBlobContainer.ToLower();

            document.LoadHtml(messageComponent.Content);
            var uploadedImage = document.DocumentNode.SelectNodes("//img").ToList();
            foreach (var image in uploadedImage)
            {
                if (image.Attributes["src"].Value.StartsWith("data:image"))
                {
                    var imageSourceValue = image.Attributes["src"].Value.Split(',').ToList();
                    var messageComponentContentWithImage = new MessageComponentContentWithImageModel
                    {
                        Format = imageSourceValue[0].Split(new char[] { ':', '/', ';' })[2],
                        Base64String = imageSourceValue[1]
                    };
                    var imageBytes = Convert.FromBase64String(messageComponentContentWithImage.Base64String);
                    var filename = $"{campaignTypeEventRegistrationStatusId} - {messageComponent.MessageComponentId} - {Guid.NewGuid()}.{messageComponentContentWithImage.Format}";

                    var blobContainerClient = new BlobContainerClient(blobStorage, blobContainer);
                    blobContainerClient.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                    blobContainerClient.UploadBlob(filename, new MemoryStream(imageBytes));
                    var imagePath = blobContainerClient.GetBlobClient(filename);
                    image.Attributes["src"].Value = imagePath.Uri.AbsoluteUri;
                    documentList.Add(document);
                }
                else continue;
            }
            return documentList;
        }
    }
}