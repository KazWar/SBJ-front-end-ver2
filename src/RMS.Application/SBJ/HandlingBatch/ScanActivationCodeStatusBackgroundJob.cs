using Abp.Authorization;
using Abp.Domain.Repositories;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.HandlingBatch.Models;
using RMS.SBJ.Messaging;
using RMS.SBJ.Messaging.Helpers;
using RMS.SBJ.Registrations;
using RMS.SBJ.Registrations.Helpers;
using RMS.EntityFrameworkCore.Repositories.RegistrationBulk;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch
{
    public class ScanActivationCodeStatusBackgroundJob : IScanActivationCodeStatusBackgroundJob
    {
        private readonly IRepository<Registration, long> _registrationRepository;
        private readonly IRepository<MessageHistory, long> _messageHistoryRepository;
        private readonly IRegistrationBulkRepository _registrationBulkRepository;
        private readonly IRegistrationHistoryBulkRepository _registrationHistoryBulkRepository;
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;
        private readonly IMessagingAppService _messagingAppService;

        public ScanActivationCodeStatusBackgroundJob(IRepository<Registration, long> registrationRepository,
                                                     IRepository<MessageHistory, long> messageHistoryRepository,
                                                     IRegistrationBulkRepository registrationBulkRepository,
                                                     IRegistrationHistoryBulkRepository registrationHistoryBulkRepository,
                                                     IRegistrationStatusesAppService registrationStatusesAppService,
                                                     IMessagingAppService messagingAppService)
        {
            _registrationRepository = registrationRepository;
            _messageHistoryRepository = messageHistoryRepository;
            _registrationBulkRepository = registrationBulkRepository;
            _registrationHistoryBulkRepository = registrationHistoryBulkRepository;
            _registrationStatusesAppService = registrationStatusesAppService;
            _messagingAppService = messagingAppService;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task ExecuteAsync(HandlingBatchJobParameters parameters)
        {
            var regiInProgress = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.InProgress);
            var regiSent = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Send);

            var registrationsInProgress = _registrationRepository.GetAll().Where(r => r.RegistrationStatusId == regiInProgress.RegistrationStatus.Id).OrderBy(r => r.Id).ToList();

            var bulkRegistrations = new List<Registration>();
            var newRegistrationHistories = new List<RegistrationHistory.RegistrationHistory>();

            foreach (var registration in registrationsInProgress)
            {
                var activationCodeMessageHistory = _messageHistoryRepository.GetAll().Where(h => h.RegistrationId == registration.Id && h.MessageName.ToLower().Trim() == "activationcode").FirstOrDefault();

                if (activationCodeMessageHistory != null)
                {
                    var activationCodeMessageStatusList = _messagingAppService.getMessageStatusList(new List<long>() { activationCodeMessageHistory.MessageId });
                    var activationCodeMessageStatus = activationCodeMessageStatusList.Any(s => s.MessageId == activationCodeMessageHistory.MessageId) ? activationCodeMessageStatusList.Where(s => s.MessageId == activationCodeMessageHistory.MessageId).First().StatusId : MessageStatusHelper.Unknown;

                    if (activationCodeMessageStatus == MessageStatusHelper.Sent)
                    {
                        registration.RegistrationStatusId = regiSent.RegistrationStatus.Id;

                        bulkRegistrations.Add(registration);
                        newRegistrationHistories.Add(new RegistrationHistory.RegistrationHistory()
                        {
                            RegistrationId = registration.Id,
                            RegistrationStatusId = regiSent.RegistrationStatus.Id,
                            AbpUserId = parameters.AbpUserId,
                            TenantId = parameters.TenantId,
                            DateCreated = DateTime.Now,
                            Remarks = "Sent by SendGrid"
                        });
                    }
                }
            }

            if (bulkRegistrations.Count > 0)
            {
                await _registrationBulkRepository.BulkUpdate(bulkRegistrations);
                await _registrationHistoryBulkRepository.BulkInsert(newRegistrationHistories);
            }
        }
    }
}
