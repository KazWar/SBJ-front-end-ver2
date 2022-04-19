using Abp.Authorization;
using Abp.Domain.Repositories;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.HandlingBatch.Models;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.Registrations;
using RMS.SBJ.HandlingBatch.Helpers;
using RMS.SBJ.Registrations.Helpers;
using RMS.SBJ.CampaignProcesses.Helpers;
using RMS.SBJ.ActivationCodes;
using RMS.SBJ.Messaging.Dtos;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.Messaging;
using RMS.SBJ.SystemTables;
using RMS.EntityFrameworkCore.Repositories.HandlingBatchBulk;
using RMS.EntityFrameworkCore.Repositories.RegistrationBulk;
using RMS.EntityFrameworkCore.Repositories.MessagingBulk;
using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch
{
    public class HandlingActivationCodeBatchesBackgroundJob : IHandlingActivationCodeBatchesBackgroundJob
    {
        private readonly IRepository<HandlingBatch, long> _handlingBatchRepository;
        private readonly IRepository<HandlingBatchLine, long> _handlingBatchLineRepository;
        private readonly IRepository<HandlingBatchHistory, long> _handlingBatchHistoryRepository;
        private readonly IRepository<HandlingBatchLineHistory, long> _handlingBatchLineHistoryRepository;
        private readonly IRepository<Registration, long> _registrationRepository;
        private readonly IRepository<PurchaseRegistration, long> _purchaseRegistrationRepository;
        private readonly IRepository<RegistrationHistory.RegistrationHistory, long> _registrationHistoryRepository;
        private readonly IRepository<Country, long> _countryRepository;
        private readonly IRepository<Locale, long> _localeRepository;
        private readonly IRepository<ActivationCode, long> _activationCodeRepository;
        private readonly IRepository<ProcessEvent, long> _processEventRepository;
        private readonly IRepository<Campaign, long> _campaignRepository;
        private readonly IRepository<CampaignMessage, long> _campaignMessageRepository;
        private readonly IRepository<CampaignTranslation, long> _campaignTranslationRepository;
        private readonly IRepository<CampaignTypeEvent, long> _campaignTypeEventRepository;
        private readonly IRepository<CampaignTypeEventRegistrationStatus, long> _campaignTypeEventRegistrationStatusRepository;
        private readonly IRepository<Company.Company, long> _companyRepository;
        private readonly IRepository<Message, long> _messageRepository;
        private readonly IRepository<MessageType, long> _messageTypeRepository;
        private readonly IRepository<MessageComponent, long> _messageComponentRepository;
        private readonly IRepository<MessageComponentType, long> _messageComponentTypeRepository;       
        private readonly IRepository<MessageComponentContent, long> _messageComponentContentRepository;
        private readonly IRepository<MessageContentTranslation, long> _messageContentTranslationRepository;
        private readonly IRepository<MessageVariable, long> _messageVariableRepository;
        private readonly IRepository<MessageHistory, long> _messageHistoryRepository;
        private readonly IHandlingBatchLineBulkRepository _handlingBatchLineBulkRepository;
        private readonly IHandlingBatchLineHistoryBulkRepository _handlingBatchLineHistoryBulkRepository;
        private readonly IRegistrationBulkRepository _registrationBulkRepository;
        private readonly IRegistrationHistoryBulkRepository _registrationHistoryBulkRepository;
        private readonly IMessageHistoryBulkRepository _messageHistoryBulkRepository;
        private readonly IHandlingBatchStatusesAppService _handlingBatchStatusAppService;
        private readonly IHandlingBatchLineStatusesAppService _handlingBatchLineStatusAppService;
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;
        private readonly ICampaignTypesAppService _campaignTypesAppService;
        private readonly IMessagingAppService _messagingAppService;

        public HandlingActivationCodeBatchesBackgroundJob(IRepository<HandlingBatch, long> handlingBatchRepository,
                                                   IRepository<HandlingBatchLine, long> handlingBatchLineRepository,
                                                   IRepository<HandlingBatchHistory, long> handlingBatchHistoryRepository,
                                                   IRepository<HandlingBatchLineHistory, long> handlingBatchLineHistoryRepository,
                                                   IRepository<Registration, long> registrationRepository,
                                                   IRepository<PurchaseRegistration, long> purchaseRegistrationRepository,
                                                   IRepository<RegistrationHistory.RegistrationHistory, long> registrationHistoryRepository,
                                                   IRepository<Country, long> countryRepository,
                                                   IRepository<Locale, long> localeRepository,
                                                   IRepository<ActivationCode, long> activationCodeRepository,
                                                   IRepository<ProcessEvent, long> processEventRepository,
                                                   IRepository<Campaign, long> campaignRepository,
                                                   IRepository<CampaignMessage, long> campaignMessageRepository,
                                                   IRepository<CampaignTranslation, long> campaignTranslationRepository,
                                                   IRepository<CampaignTypeEvent, long> campaignTypeEventRepository,
                                                   IRepository<CampaignTypeEventRegistrationStatus, long> campaignTypeEventRegistrationStatusRepository,
                                                   IRepository<Company.Company, long> companyRepository,
                                                   IRepository<Message, long> messageRepository,
                                                   IRepository<MessageType, long> messageTypeRepository,
                                                   IRepository<MessageComponent, long> messageComponentRepository,
                                                   IRepository<MessageComponentType, long> messageComponentTypeRepository,
                                                   IRepository<MessageComponentContent, long> messageComponentContentRepository,
                                                   IRepository<MessageContentTranslation, long> messageContentTranslationRepository,
                                                   IRepository<MessageVariable, long> messageVariableRepository,
                                                   IRepository<MessageHistory, long> messageHistoryRepository,
                                                   IHandlingBatchLineBulkRepository handlingBatchLineBulkRepository,
                                                   IHandlingBatchLineHistoryBulkRepository handlingBatchLineHistoryBulkRepository,
                                                   IRegistrationBulkRepository registrationBulkRepository,
                                                   IRegistrationHistoryBulkRepository registrationHistoryBulkRepository,
                                                   IMessageHistoryBulkRepository messageHistoryBulkRepository,
                                                   IHandlingBatchStatusesAppService handlingBatchStatusAppService,
                                                   IHandlingBatchLineStatusesAppService handlingBatchLineStatusAppService,
                                                   IRegistrationStatusesAppService registrationStatusesAppService,
                                                   ICampaignTypesAppService campaignTypesAppService,
                                                   IMessagingAppService messagingAppService)
        {
            _handlingBatchRepository = handlingBatchRepository;
            _handlingBatchLineRepository = handlingBatchLineRepository;
            _handlingBatchHistoryRepository = handlingBatchHistoryRepository;
            _handlingBatchLineHistoryRepository = handlingBatchLineHistoryRepository;
            _registrationRepository = registrationRepository;
            _purchaseRegistrationRepository = purchaseRegistrationRepository;
            _registrationHistoryRepository = registrationHistoryRepository;
            _countryRepository = countryRepository;
            _localeRepository = localeRepository;
            _activationCodeRepository = activationCodeRepository;
            _processEventRepository = processEventRepository;
            _campaignRepository = campaignRepository;
            _campaignMessageRepository = campaignMessageRepository;
            _campaignTranslationRepository = campaignTranslationRepository;
            _campaignTypeEventRepository = campaignTypeEventRepository;
            _campaignTypeEventRegistrationStatusRepository = campaignTypeEventRegistrationStatusRepository;
            _companyRepository = companyRepository;
            _messageRepository = messageRepository;
            _messageTypeRepository = messageTypeRepository;
            _messageComponentRepository = messageComponentRepository;
            _messageComponentTypeRepository = messageComponentTypeRepository;
            _messageComponentContentRepository = messageComponentContentRepository;
            _messageContentTranslationRepository = messageContentTranslationRepository;
            _messageVariableRepository = messageVariableRepository;
            _messageHistoryRepository = messageHistoryRepository;
            _handlingBatchLineBulkRepository = handlingBatchLineBulkRepository;
            _handlingBatchLineHistoryBulkRepository = handlingBatchLineHistoryBulkRepository;
            _registrationBulkRepository = registrationBulkRepository;
            _registrationHistoryBulkRepository = registrationHistoryBulkRepository;
            _messageHistoryBulkRepository = messageHistoryBulkRepository;
            _handlingBatchStatusAppService = handlingBatchStatusAppService;
            _handlingBatchLineStatusAppService = handlingBatchLineStatusAppService;
            _registrationStatusesAppService = registrationStatusesAppService;
            _campaignTypesAppService = campaignTypesAppService;
            _messagingAppService = messagingAppService;
        }

        public class MessageComponentSource
        {
            public long LocaleId { get; set; }

            public long MessageId { get; set; }

            public string ComponentType { get; set; }

            public string ComponentContent { get; set; }
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task ExecuteAsync(HandlingBatchJobParameters parameters)
        {
            if (parameters.HandlingBatchId == null)
            {
                return;
            }

            var typeActivationCode = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.ActivationCode);

            var batchPending = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.Pending);
            //var batchInProgress = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.InProgress);
            var batchProcessedPartiallyWithBlockedLines = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.ProcessedPartiallyWithBlockedLines);
            var batchUnprocessedBecauseOfBlockedLines = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.UnprocessedBecauseOfBlockedLines);
            var batchFinished = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.Finished);

            var linePending = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Pending);
            //var lineInProgress = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.InProgress);
            var lineFinished = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Finished);
            var lineFailed = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Failed);
            var lineBlocked = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Blocked);

            var regiApproved = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Accepted);
            var regiInProgress = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.InProgress);

            //the handling batch that needs to be processed...
            var validBatchStatusIds = new List<long>();

            validBatchStatusIds.Add(batchPending.HandlingBatchStatus.Id);
            validBatchStatusIds.Add(batchProcessedPartiallyWithBlockedLines.HandlingBatchStatus.Id);
            validBatchStatusIds.Add(batchUnprocessedBecauseOfBlockedLines.HandlingBatchStatus.Id);

            var handlingBatch = await _handlingBatchRepository.GetAsync(parameters.HandlingBatchId.Value);

            if (handlingBatch == null || handlingBatch.CampaignTypeId != typeActivationCode.CampaignType.Id || !validBatchStatusIds.Contains(handlingBatch.HandlingBatchStatusId))
            {
                return;
            }

            //pick up all the batch lines with status pending or blocked... 
            var handlingBatchLines = _handlingBatchLineRepository.GetAll().Where(h => h.HandlingBatchId == handlingBatch.Id && (h.HandlingBatchLineStatusId == linePending.HandlingBatchLineStatus.Id ||
                                                                                                                                h.HandlingBatchLineStatusId == lineBlocked.HandlingBatchLineStatus.Id));

            if (handlingBatchLines == null || handlingBatchLines.Count() == 0 || handlingBatchLines.Any(h => h.ActivationCodeId == null))
            {
                return;
            }

            //lookup of ActivationCode message components...
            var campaignTypeEventRegistrationStatus = (from o in _campaignTypeEventRegistrationStatusRepository.GetAll()
                                                       join o1 in _campaignTypeEventRepository.GetAll() on o.CampaignTypeEventId equals o1.Id into j1
                                                       from s1 in j1.DefaultIfEmpty()
                                                       join o2 in _processEventRepository.GetAll() on s1.ProcessEventId equals o2.Id into j2
                                                       from s2 in j2.DefaultIfEmpty()
                                                       where s1.CampaignTypeId == typeActivationCode.CampaignType.Id
                                                          && s2.Name.ToLower().Trim() == "activationcode"
                                                          && o.RegistrationStatusId == regiInProgress.RegistrationStatus.Id
                                                       select o).FirstOrDefault();

            if (campaignTypeEventRegistrationStatus == null)
            {
                return;
            }

            var localeIds = (from o in handlingBatchLines
                             join o1 in _purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()
                             join o2 in _registrationRepository.GetAll() on s1.RegistrationId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()
                             select s2.LocaleId).Distinct().ToList();

            var messageComponents = new List<MessageComponentSource>();
            var messageComponentsDefault = (from o in _messageComponentContentRepository.GetAll()
                                                 join o1 in _messageComponentRepository.GetAll() on o.MessageComponentId equals o1.Id into j1
                                                 from s1 in j1.DefaultIfEmpty()
                                                 join o2 in _messageComponentTypeRepository.GetAll() on s1.MessageComponentTypeId equals o2.Id into j2
                                                 from s2 in j2.DefaultIfEmpty()
                                                 join o3 in _messageTypeRepository.GetAll() on s1.MessageTypeId equals o3.Id into j3
                                                 from s3 in j3.DefaultIfEmpty()
                                                 where o.CampaignTypeEventRegistrationStatusId == campaignTypeEventRegistrationStatus.Id
                                                 orderby s1.MessageComponentTypeId 
                                                 select new
                                                 {
                                                    MessageId = s3.MessageId,
                                                    ComponentType = s2.Name,
                                                    ComponentContent = o.Content
                                                 }).ToList();

            foreach (var messageComponent in messageComponentsDefault)
            {
                messageComponents.Add(new MessageComponentSource()
                {
                    LocaleId = 0,
                    MessageId = messageComponent.MessageId,
                    ComponentType = messageComponent.ComponentType,
                    ComponentContent = messageComponent.ComponentContent
                });
            }

            var messageComponentsLocalized = (from o in _messageContentTranslationRepository.GetAll()
                                              join o1 in _messageComponentContentRepository.GetAll() on o.MessageComponentContentId equals o1.Id into j1
                                              from s1 in j1.DefaultIfEmpty()
                                              join o2 in _messageComponentRepository.GetAll() on s1.MessageComponentId equals o2.Id into j2
                                              from s2 in j2.DefaultIfEmpty()
                                              join o3 in _messageComponentTypeRepository.GetAll() on s2.MessageComponentTypeId equals o3.Id into j3
                                              from s3 in j3.DefaultIfEmpty()
                                              join o4 in _messageTypeRepository.GetAll() on s2.MessageTypeId equals o4.Id into j4
                                              from s4 in j4.DefaultIfEmpty()
                                              where s1.CampaignTypeEventRegistrationStatusId == campaignTypeEventRegistrationStatus.Id
                                              orderby o.LocaleId, s2.MessageComponentTypeId
                                              select new
                                              {
                                                  LocaleId = o.LocaleId,
                                                  MessageId = s4.MessageId,
                                                  ComponentType = s3.Name,
                                                  ComponentContent = o.Content
                                              }).ToList();

            foreach (var messageComponent in messageComponentsLocalized)
            {
                messageComponents.Add(new MessageComponentSource()
                {
                    LocaleId = messageComponent.LocaleId,
                    MessageId = messageComponent.MessageId,
                    ComponentType = messageComponent.ComponentType,
                    ComponentContent = messageComponent.ComponentContent
                });
            }

            var messageVariables = _messageVariableRepository.GetAll();

            if (!messageVariables.Any(v => v.Description.ToLower().Trim() == "activationcode"))
            {
                return;
            }

            //keep counts for the final status
            var importCount = 0;
            var blockCount = 0;

            //go...
            var purchaseRegistrationIds = handlingBatchLines.Select(l => l.PurchaseRegistrationId).ToList();
            var registrationIds = (from o in handlingBatchLines
                                   join o1 in _purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()
                                   orderby s1.RegistrationId
                                   select s1.RegistrationId).Distinct().ToList();

            var bulkHandlingBatchLines = new List<HandlingBatchLine>();
            var bulkRegistrations = new List<Registration>();

            var newHandlingBatchLineHistories = new List<HandlingBatchLineHistory>();
            var newRegistrationHistories = new List<RegistrationHistory.RegistrationHistory>();
            var newMessageHistories = new List<MessageHistory>();

            foreach (var registrationId in registrationIds)
            {
                var registration = await _registrationRepository.GetAsync(registrationId);
                var registrationCampaign = await _campaignRepository.GetAsync(registration.CampaignId);
                var registrationCampaignCompany = _companyRepository.GetAll().First();
                var registrationPurchaseRegsInDBase = await _purchaseRegistrationRepository.GetAllListAsync(p => p.RegistrationId == registration.Id);
                var registrationPurchaseRegsInBatch = registrationPurchaseRegsInDBase.Where(p => purchaseRegistrationIds.Contains(p.Id)).ToList();
                var registrationPurchaseRegIdsInBatch = registrationPurchaseRegsInBatch.Select(p => p.Id).ToList();
                var registrationCountry = await _countryRepository.GetAsync(registration.CountryId);
                var registrationLocale = await _localeRepository.GetAsync(registration.LocaleId);
                var registrationHandlingBatchLines = handlingBatchLines.Where(l => registrationPurchaseRegIdsInBatch.Contains(l.PurchaseRegistrationId)).ToList();

                //exception handling: if registration is not "approved" right now, block the related batch line(s) and skip the registration
                if (registration.RegistrationStatusId != regiApproved.RegistrationStatus.Id)
                {
                    //put the related batch line(s) on "blocked" and also update the history
                    foreach (var registrationHandlingBatchLine in registrationHandlingBatchLines)
                    {
                        registrationHandlingBatchLine.HandlingBatchLineStatusId = lineBlocked.HandlingBatchLineStatus.Id;

                        await _handlingBatchLineRepository.UpdateAsync(registrationHandlingBatchLine);
                        await _handlingBatchLineHistoryRepository.InsertAsync(new HandlingBatchLineHistory()
                        {
                            HandlingBatchLineId = registrationHandlingBatchLine.Id,
                            HandlingBatchLineStatusId = lineBlocked.HandlingBatchLineStatus.Id,
                            AbpUserId = parameters.AbpUserId,
                            TenantId = parameters.TenantId,
                            DateCreated = DateTime.Now,
                            Remarks = String.Empty
                        });

                        blockCount += 1;
                    }

                    //go to the next registration...
                    continue;
                }

                //do the message...
                var registrationMessageBody = new StringBuilder();
                var registrationMessageSubject = String.Empty;
                long registrationMessageId = 0;

                var registrationActivationCodeIds = registrationHandlingBatchLines.Select(l => l.ActivationCodeId).ToList();
                var registrationActivationCodes = _activationCodeRepository.GetAll().Where(x => registrationActivationCodeIds.Contains(x.Id)).Select(x => x.Code).ToList();
                var registrationActivationCodeString = String.Join(", ", registrationActivationCodes);

                var registrationMessageComponentsDefault = messageComponents.Where(c => c.LocaleId == 0).ToList();
                var registrationSourceMessageIds = registrationMessageComponentsDefault.Select(c => c.MessageId).Distinct();
                long registrationSourceMessageId = registrationSourceMessageIds.Min();

                if (registrationSourceMessageIds.Count() > 1)
                {
                    //choose the right registrationSourceMessageId
                    var campaignMessage = _campaignMessageRepository.GetAll().Where(m => m.CampaignId == registrationCampaign.Id && registrationSourceMessageIds.Contains(m.MessageId)).FirstOrDefault();

                    if (campaignMessage != null)
                    {
                        registrationSourceMessageId = campaignMessage.MessageId;
                    } 
                    else
                    {
                        var companyMessage = _messageRepository.GetAll().Where(m => m.SystemLevelId == 1 && registrationSourceMessageIds.Contains(m.Id)).FirstOrDefault();

                        if (companyMessage != null)
                        {
                            registrationSourceMessageId = companyMessage.Id;
                        }
                    }
                }

                registrationMessageComponentsDefault = messageComponents.Where(c => c.LocaleId == 0 && c.MessageId == registrationSourceMessageId).ToList();

                foreach (var registrationMessageComponent in registrationMessageComponentsDefault)
                {
                    var registrationMessageComponentContent = registrationMessageComponent.ComponentContent;

                    if (messageComponents.Any(c => c.ComponentType == registrationMessageComponent.ComponentType && c.LocaleId == registration.LocaleId && c.MessageId == registrationSourceMessageId))
                    {
                        registrationMessageComponentContent = messageComponents.Where(c => c.ComponentType == registrationMessageComponent.ComponentType && c.LocaleId == registration.LocaleId && c.MessageId == registrationSourceMessageId).First().ComponentContent;
                    }                   

                    foreach (var mvd in messageVariables)
                    {
                        if (registrationMessageComponentContent.Contains(mvd.Description))
                        {
                            if (mvd.Description.ToLower().Trim() == "activationcode")
                            {
                                registrationMessageComponentContent = registrationMessageComponentContent.Replace($"#{{{mvd.Description}}}", registrationActivationCodeString);
                            }
                            else
                            {
                                switch (mvd.RmsTable)
                                {
                                    case nameof(Registration):
                                        registrationMessageComponentContent = registrationMessageComponentContent
                                        .Replace($"#{{{mvd.Description}}}", registration
                                        .GetType()
                                        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                                        .Where(property => mvd.TableField.Contains(property.Name))
                                        .Select(property => property.GetValue(registration)).FirstOrDefault().ToString());

                                        break;

                                    case nameof(Campaign):
                                        //get translated value out of the CampaignTranslations table, if available there...
                                        var translatedVariable = _campaignTranslationRepository.GetAll().Where(t => t.CampaignId == registration.CampaignId && t.LocaleId == registration.LocaleId && t.Name.ToLower().Trim() == mvd.Description.ToLower().Trim()).FirstOrDefault();

                                        if (translatedVariable != null)
                                        {
                                            registrationMessageComponentContent = registrationMessageComponentContent
                                            .Replace($"#{{{mvd.Description}}}", translatedVariable.Description);
                                        }
                                        else
                                        {
                                            registrationMessageComponentContent = registrationMessageComponentContent
                                            .Replace($"#{{{mvd.Description}}}", registrationCampaign
                                            .GetType()
                                            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                                            .Where(property => mvd.TableField.Contains(property.Name))
                                            .Select(property => property.GetValue(registrationCampaign)).FirstOrDefault().ToString());
                                        }

                                        break;
                                }
                            }
                        }
                    }

                    if (registrationMessageComponent.ComponentType.ToLower().Trim() == "subject")
                    {
                        registrationMessageSubject = registrationMessageComponentContent;
                    }
                    else
                    {
                        registrationMessageBody.Append(registrationMessageComponentContent);
                    }
                }

                var messagingEntity = new MessagesDto
                {
                    Source = RegistrationConsts.Source,
                    Reference = registration.Id.ToString(),
                    InitiatorReference = RegistrationConsts.InitiatorReference,
                    MessageInfo = RegistrationConsts.MessageInfo,
                    MessageCollectionId = RegistrationConsts.MessageCollectionId,
                    CurrentStepId = RegistrationConsts.CurrentStepId,
                    TemplateId = RegistrationConsts.TemplateId,
                    Subject = registrationMessageSubject,
                    DisplayName = registrationCampaignCompany.Name,
                    From = RegistrationConsts.From,
                    To = registration.EmailAddress.Trim(),
                    Body = registrationMessageBody.ToString(),
                    ExpirationTime = DateTime.Now,
                    Priority = RegistrationConsts.Priority,
                    AwaitingSend = false,
                    SendError = false,
                    Finished = false,
                    CreatedAt = DateTime.Now,
                };

                registrationMessageId = _messagingAppService.CreateOrEditAndGetId(messagingEntity);

                newMessageHistories.Add(new MessageHistory()
                {
                    RegistrationId = registration.Id,
                    AbpUserId = parameters.AbpUserId,
                    TenantId = parameters.TenantId,
                    Content = registrationMessageBody.ToString(),
                    TimeStamp = DateTime.Now,
                    MessageName = "ActivationCode",
                    MessageId = registrationMessageId,
                    Subject = registrationMessageSubject,
                    To = registration.EmailAddress
                });

                importCount += 1;

                //update external order feedback and the related batch line(s) status, and also update the history
                foreach (var registrationHandlingBatchLine in registrationHandlingBatchLines)
                {
                    registrationHandlingBatchLine.HandlingBatchLineStatusId = lineFinished.HandlingBatchLineStatus.Id;

                    bulkHandlingBatchLines.Add(registrationHandlingBatchLine);
                    newHandlingBatchLineHistories.Add(new HandlingBatchLineHistory()
                    {
                        HandlingBatchLineId = registrationHandlingBatchLine.Id,
                        HandlingBatchLineStatusId = lineFinished.HandlingBatchLineStatus.Id,
                        AbpUserId = parameters.AbpUserId,
                        TenantId = parameters.TenantId,
                        DateCreated = DateTime.Now,
                        Remarks = $"MessageID: {registrationMessageId}"
                    });
                }

                //if this registration does NOT contain any purchase line(s) that belong in ANOTHER handling batch, change it to "in progress"
                //if (registrationPurchaseRegsInBatch.Count == registrationPurchaseRegsInDBase.Count)
                //update: screw that, just change it!
                registration.RegistrationStatusId = regiInProgress.RegistrationStatus.Id;

                bulkRegistrations.Add(registration);
                newRegistrationHistories.Add(new RegistrationHistory.RegistrationHistory()
                {
                    RegistrationId = registration.Id,
                    RegistrationStatusId = regiInProgress.RegistrationStatus.Id,
                    AbpUserId = parameters.AbpUserId,
                    TenantId = parameters.TenantId,
                    DateCreated = DateTime.Now,
                    Remarks = "Waiting to be sent by SendGrid"
                });
            }

            //update the handling batch status and history
            var finalStatusId = batchFinished.HandlingBatchStatus.Id;

            if (importCount > 0 && blockCount > 0)
            {
                finalStatusId = batchProcessedPartiallyWithBlockedLines.HandlingBatchStatus.Id;
            }
            else if (importCount == 0 && blockCount > 0)
            {
                finalStatusId = batchUnprocessedBecauseOfBlockedLines.HandlingBatchStatus.Id;
            }

            handlingBatch.HandlingBatchStatusId = finalStatusId;

            await _handlingBatchLineBulkRepository.BulkUpdate(bulkHandlingBatchLines);
            await _registrationBulkRepository.BulkUpdate(bulkRegistrations);

            await _handlingBatchLineHistoryBulkRepository.BulkInsert(newHandlingBatchLineHistories);
            await _registrationHistoryBulkRepository.BulkInsert(newRegistrationHistories);
            await _messageHistoryBulkRepository.BulkInsert(newMessageHistories);

            await _handlingBatchRepository.UpdateAsync(handlingBatch);
            await _handlingBatchHistoryRepository.InsertAsync(new HandlingBatchHistory()
            {
                HandlingBatchId = handlingBatch.Id,
                HandlingBatchStatusId = finalStatusId,
                AbpUserId = parameters.AbpUserId,
                TenantId = parameters.TenantId,
                DateCreated = DateTime.Now,
                Remarks = String.Empty
            });
        }
    }
}