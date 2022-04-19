using Abp.Authorization;
using Abp.Domain.Repositories;
using RMS.Authorization;
using RMS.External;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.HandlingBatch.Models;
using RMS.SBJ.Registrations;
using RMS.SBJ.Registrations.Helpers;
using RMS.EntityFrameworkCore.Repositories.RegistrationBulk;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WMSOrderService;

namespace RMS.SBJ.HandlingBatch
{
    public class ScanWarehouseStatusBackgroundJob : IScanWarehouseStatusBackgroundJob
    {
        private readonly IRepository<Registration, long> _registrationRepository;
        private readonly IRegistrationBulkRepository _registrationBulkRepository;
        private readonly IRegistrationHistoryBulkRepository _registrationHistoryBulkRepository;
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;
        private readonly IWmsOrder _wmsOrderClient;

        public ScanWarehouseStatusBackgroundJob(IRepository<Registration, long> registrationRepository,
                                                IRegistrationBulkRepository registrationBulkRepository,
                                                IRegistrationHistoryBulkRepository registrationHistoryBulkRepository,
                                                IRegistrationStatusesAppService registrationStatusesAppService,
                                                IWmsOrder wmsOrderClient)
        {
            _registrationRepository = registrationRepository;
            _registrationBulkRepository = registrationBulkRepository;
            _registrationHistoryBulkRepository = registrationHistoryBulkRepository;
            _registrationStatusesAppService = registrationStatusesAppService;
            _wmsOrderClient = wmsOrderClient;
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
                var wmsOrderReference = $"RMS2 {registration.Id}";

                GetOrderStatusResult wmsOrderStatus = null;

                try
                {
                    wmsOrderStatus = _wmsOrderClient.GetOrderStatus(wmsOrderReference, parameters.WarehouseId.Value, parameters.OrderUserId, parameters.Password).Result;
                }
                catch (Exception ex)
                {
                    continue;
                }

                if (wmsOrderStatus != null && wmsOrderStatus.MainOrder != null && wmsOrderStatus.MainOrder.StatusId == 205)
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
                        Remarks = "Sent by WMS"
                    });
                }
            }

            if (bulkRegistrations.Count > 0) { 
                await _registrationBulkRepository.BulkUpdate(bulkRegistrations); 
                await _registrationHistoryBulkRepository.BulkInsert(newRegistrationHistories); 
            }
        }
    }
}
