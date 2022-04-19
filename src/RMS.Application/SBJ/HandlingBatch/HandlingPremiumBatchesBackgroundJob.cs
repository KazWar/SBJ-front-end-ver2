using Abp.Authorization;
using Abp.Domain.Repositories;
using RMS.Authorization;
using RMS.External;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.HandlingBatch.Helpers;
using RMS.SBJ.HandlingBatch.Models;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.Registrations;
using RMS.SBJ.Registrations.Helpers;
using RMS.SBJ.CampaignProcesses.Helpers;
using RMS.EntityFrameworkCore.Repositories.HandlingBatchBulk;
using RMS.EntityFrameworkCore.Repositories.RegistrationBulk;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WMSOrderService;

namespace RMS.SBJ.HandlingBatch
{
    public class HandlingPremiumBatchesBackgroundJob : IHandlingPremiumBatchesBackgroundJob
    {
        private readonly IRepository<HandlingBatch, long> _handlingBatchRepository;
        private readonly IRepository<HandlingBatchLine, long> _handlingBatchLineRepository;       
        private readonly IRepository<HandlingBatchLineHistory, long> _handlingBatchLineHistoryRepository;
        private readonly IRepository<Registration, long> _registrationRepository;
        private readonly IRepository<PurchaseRegistration, long> _purchaseRegistrationRepository;        
        private readonly IRepository<Country, long> _countryRepository;
        private readonly IRepository<Locale, long> _localeRepository;
        private readonly IHandlingBatchBulkRepository _handlingBatchBulkRepository;
        private readonly IHandlingBatchHistoryBulkRepository _handlingBatchHistoryBulkRepository;
        private readonly IHandlingBatchLineBulkRepository _handlingBatchLineBulkRepository;
        private readonly IHandlingBatchLineHistoryBulkRepository _handlingBatchLineHistoryBulkRepository;
        private readonly IRegistrationBulkRepository _registrationBulkRepository;
        private readonly IRegistrationHistoryBulkRepository _registrationHistoryBulkRepository;
        private readonly IHandlingBatchStatusesAppService _handlingBatchStatusAppService;
        private readonly IHandlingBatchLineStatusesAppService _handlingBatchLineStatusAppService;
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;
        private readonly ICampaignTypesAppService _campaignTypesAppService;
        private readonly IWmsOrder _wmsOrderClient;

        public HandlingPremiumBatchesBackgroundJob(IRepository<HandlingBatch, long> handlingBatchRepository,
                                                   IRepository<HandlingBatchLine, long> handlingBatchLineRepository,
                                                   IRepository<HandlingBatchLineHistory, long> handlingBatchLineHistoryRepository,
                                                   IRepository<Registration, long> registrationRepository,
                                                   IRepository<PurchaseRegistration, long> purchaseRegistrationRepository,
                                                   IRepository<Country, long> countryRepository,
                                                   IRepository<Locale, long> localeRepository,
                                                   IHandlingBatchBulkRepository handlingBatchBulkRepository,
                                                   IHandlingBatchHistoryBulkRepository handlingBatchHistoryBulkRepository,
                                                   IHandlingBatchLineBulkRepository handlingBatchLineBulkRepository,
                                                   IHandlingBatchLineHistoryBulkRepository handlingBatchLineHistoryBulkRepository,
                                                   IRegistrationBulkRepository registrationBulkRepository,
                                                   IRegistrationHistoryBulkRepository registrationHistoryBulkRepository,
                                                   IHandlingBatchStatusesAppService handlingBatchStatusAppService,
                                                   IHandlingBatchLineStatusesAppService handlingBatchLineStatusAppService,
                                                   IRegistrationStatusesAppService registrationStatusesAppService,
                                                   ICampaignTypesAppService campaignTypesAppService,
                                                   IWmsOrder wmsOrderClient)
        {
            _handlingBatchRepository = handlingBatchRepository;
            _handlingBatchLineRepository = handlingBatchLineRepository;
            _handlingBatchLineHistoryRepository = handlingBatchLineHistoryRepository;
            _registrationRepository = registrationRepository;
            _purchaseRegistrationRepository = purchaseRegistrationRepository;
            _countryRepository = countryRepository;
            _localeRepository = localeRepository;
            _handlingBatchBulkRepository = handlingBatchBulkRepository;
            _handlingBatchHistoryBulkRepository = handlingBatchHistoryBulkRepository;
            _handlingBatchLineBulkRepository = handlingBatchLineBulkRepository;
            _handlingBatchLineHistoryBulkRepository = handlingBatchLineHistoryBulkRepository;
            _registrationBulkRepository = registrationBulkRepository;
            _registrationHistoryBulkRepository = registrationHistoryBulkRepository;
            _handlingBatchStatusAppService = handlingBatchStatusAppService;
            _handlingBatchLineStatusAppService = handlingBatchLineStatusAppService;
            _registrationStatusesAppService = registrationStatusesAppService;
            _campaignTypesAppService = campaignTypesAppService;
            _wmsOrderClient = wmsOrderClient;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task ExecuteAsync(HandlingBatchJobParameters parameters)
        {     
            if (parameters.WarehouseId == null || parameters.OrderUserId == null || String.IsNullOrWhiteSpace(parameters.Password))
            {
                return;
            }

            var typePremium = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.Premium);

            var batchPending = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.Pending);
            //var batchInProgress = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.InProgress);
            var batchProcessedPartiallyWithFailedOrders = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.ProcessedPartiallyWithFailedOrders);
            var batchProcessedPartiallyWithBlockedLines = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.ProcessedPartiallyWithBlockedLines);
            var batchProcessedPartiallyWithFailedOrdersAndBlockedLines = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.ProcessedPartiallyWithFailedOrdersAndBlockedLines);
            var batchUnprocessedBecauseOfFailedOrders = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.UnprocessedBecauseOfFailedOrders);
            var batchUnprocessedBecauseOfBlockedLines = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.UnprocessedBecauseOfBlockedLines);
            var batchUnprocessedBecauseOfFailedOrdersAndBlockedLines = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.UnprocessedBecauseOfFailedOrdersAndBlockedLines);
            var batchFinished = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.Finished);

            var linePending = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Pending);
            //var lineInProgress = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.InProgress);
            var lineFinished = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Finished);
            var lineFailed = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Failed);
            var lineBlocked = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Blocked);

            var regiApproved = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Accepted);
            var regiInProgress = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.InProgress);

            //collect the batches that need to be processed, first the pending ones and after that the ones that need a retry...
            var validBatchStatusIds = new List<long>();

            validBatchStatusIds.Add(batchPending.HandlingBatchStatus.Id);
            validBatchStatusIds.Add(batchProcessedPartiallyWithFailedOrders.HandlingBatchStatus.Id);
            validBatchStatusIds.Add(batchProcessedPartiallyWithBlockedLines.HandlingBatchStatus.Id);
            validBatchStatusIds.Add(batchProcessedPartiallyWithFailedOrdersAndBlockedLines.HandlingBatchStatus.Id);
            validBatchStatusIds.Add(batchUnprocessedBecauseOfFailedOrders.HandlingBatchStatus.Id);
            validBatchStatusIds.Add(batchUnprocessedBecauseOfBlockedLines.HandlingBatchStatus.Id);
            validBatchStatusIds.Add(batchUnprocessedBecauseOfFailedOrdersAndBlockedLines.HandlingBatchStatus.Id);

            var handlingBatches = _handlingBatchRepository.GetAll().Where(h => h.CampaignTypeId == typePremium.CampaignType.Id && validBatchStatusIds.Contains(h.HandlingBatchStatusId)).OrderBy(h => h.HandlingBatchStatusId);

            if (handlingBatches == null || handlingBatches.Count() == 0)
            {
                return;
            }

            var bulkHandlingBatches = new List<HandlingBatch>();
            var bulkHandlingBatchLines = new List<HandlingBatchLine>();
            var bulkRegistrations = new List<Registration>();

            var newHandlingBatchHistories = new List<HandlingBatchHistory>();
            var newHandlingBatchLineHistories = new List<HandlingBatchLineHistory>();
            var newRegistrationHistories = new List<RegistrationHistory.RegistrationHistory>();

            foreach (var handlingBatch in handlingBatches)
            {               
                //pick up all the batch lines with status pending, blocked, or failed...
                var handlingBatchLines = _handlingBatchLineRepository.GetAll().Where(h => h.HandlingBatchId == handlingBatch.Id && (h.HandlingBatchLineStatusId == linePending.HandlingBatchLineStatus.Id ||
                                                                                                                                    h.HandlingBatchLineStatusId == lineBlocked.HandlingBatchLineStatus.Id ||
                                                                                                                                    h.HandlingBatchLineStatusId == lineFailed.HandlingBatchLineStatus.Id));

                if (handlingBatchLines == null || handlingBatchLines.Count() == 0)
                {
                    continue;
                }

                //keep counts for the final status
                var importCount = 0;
                var blockCount = 0;
                var failCount = 0;

                //go...
                var purchaseRegistrationIds = handlingBatchLines.Select(l => l.PurchaseRegistrationId).ToList();
                var registrationIds = (from o in handlingBatchLines
                                       join o1 in _purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()
                                       orderby s1.RegistrationId
                                       select s1.RegistrationId).Distinct().ToList();

                foreach (var registrationId in registrationIds)
                {
                    var registration = await _registrationRepository.GetAsync(registrationId);
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

                    //do the order...
                    var newOrder = new ImportOrder_Order
                    {
                        Reference = $"RMS2 {registration.Id}",
                        Language = registrationLocale.LanguageCode.ToUpper().Trim(),
                        MailingAllowed = false,
                        PartialDeliveryAllowed = false,
                    };

                    var shippingData = new ImportOrder_ShippingData
                    {
                        Gender = !String.IsNullOrEmpty(registration.Gender) && (registration.Gender.ToUpper().Trim() == "M" || registration.Gender.ToUpper().Trim() == "F") ? registration.Gender.ToUpper().Trim() : String.Empty,
                        Company = registration.CompanyName,
                        FirstName = !String.IsNullOrWhiteSpace(registration.FirstName) ? registration.FirstName : registration.CompanyName,
                        LastName = !String.IsNullOrWhiteSpace(registration.LastName) ? registration.LastName : registration.CompanyName,
                        Address = $"{registration.Street} {registration.HouseNr}",
                        PostalCode = registration.PostalCode,
                        City = registration.City,
                        Country = registrationCountry.CountryCode.ToUpper().Trim(),
                        Phone = registration.PhoneNumber,
                        Email = registration.EmailAddress,
                        CustomerInfo = String.Empty
                    };

                    newOrder.ShippingData = shippingData;

                    var orderElements = new List<ImportOrder_OrderElement>();

                    foreach (var registrationHandlingBatchLine in registrationHandlingBatchLines)
                    {
                        if (orderElements.Any(e => e.CustomerCode == registrationHandlingBatchLine.CustomerCode.Trim()))
                        {
                            var orderElement = orderElements.Where(e => e.CustomerCode == registrationHandlingBatchLine.CustomerCode.Trim()).First();

                            orderElement.Quantity += registrationHandlingBatchLine.Quantity.Value;
                        }
                        else
                        {
                            var new_orderElement = new ImportOrder_OrderElement();

                            new_orderElement.CustomerCode = registrationHandlingBatchLine.CustomerCode.Trim();
                            new_orderElement.Quantity = registrationHandlingBatchLine.Quantity.Value;

                            orderElements.Add(new_orderElement);
                        }
                    }

                    newOrder.OrderElements = orderElements.ToArray();

                    var importOrderSuccess = false;
                    var catchErrorMessage = String.Empty;

                    ImportOrderResult importOrderResult = null;

                    try
                    {
                        importOrderResult = _wmsOrderClient.ImportOrder(newOrder, parameters.WarehouseId.Value, parameters.OrderUserId, parameters.Password).Result;
                        importOrderSuccess = (importOrderResult.Succes && importOrderResult.Code == 0 && importOrderResult.ImportedOrder != null && importOrderResult.ImportedOrder.OrderId != 0);
                    }
                    catch (Exception ex)
                    {
                        catchErrorMessage = ex.Message;
                    }

                    //update external order feedback and the related batch line(s) status, and also update the history
                    foreach (var registrationHandlingBatchLine in registrationHandlingBatchLines)
                    {
                        registrationHandlingBatchLine.ExternalOrderId = importOrderSuccess && importOrderResult != null ? importOrderResult.ImportedOrder.OrderId.ToString() : String.Empty;
                        registrationHandlingBatchLine.HandlingBatchLineStatusId = importOrderSuccess ? lineFinished.HandlingBatchLineStatus.Id : lineFailed.HandlingBatchLineStatus.Id;

                        bulkHandlingBatchLines.Add(registrationHandlingBatchLine);
                        newHandlingBatchLineHistories.Add(new HandlingBatchLineHistory()
                        {
                            HandlingBatchLineId = registrationHandlingBatchLine.Id,
                            HandlingBatchLineStatusId = importOrderSuccess ? lineFinished.HandlingBatchLineStatus.Id : lineFailed.HandlingBatchLineStatus.Id,
                            AbpUserId = parameters.AbpUserId,
                            TenantId = parameters.TenantId,
                            DateCreated = DateTime.Now,
                            Remarks = !importOrderSuccess ? importOrderResult != null ? $"ERROR-CODE {importOrderResult.Code}" : catchErrorMessage : String.Empty
                        });

                        if (importOrderSuccess) { importCount += 1; } else { failCount += 1; }
                    }

                    //if success => if this registration does NOT contain any purchase line(s) that belong in ANOTHER handling batch, change it to "in progress"
                    //if (importOrderSuccess && registrationPurchaseRegsInBatch.Count == registrationPurchaseRegsInDBase.Count)
                    //update: screw the Count compare, just change it on success!
                    if (importOrderSuccess)
                    {
                        registration.RegistrationStatusId = regiInProgress.RegistrationStatus.Id;

                        bulkRegistrations.Add(registration);
                        newRegistrationHistories.Add(new RegistrationHistory.RegistrationHistory()
                        {
                            RegistrationId = registration.Id,
                            RegistrationStatusId = regiInProgress.RegistrationStatus.Id,
                            AbpUserId = parameters.AbpUserId,
                            TenantId = parameters.TenantId,
                            DateCreated = DateTime.Now,
                            Remarks = "Waiting to be sent by WMS"
                        });
                    }
                }

                //update the handling batch status and history
                var finalStatusId = batchFinished.HandlingBatchStatus.Id;

                if (importCount > 0)
                {
                    if (failCount > 0 && blockCount == 0)
                    {
                        finalStatusId = batchProcessedPartiallyWithFailedOrders.HandlingBatchStatus.Id;
                    }
                    else if (blockCount > 0 && failCount == 0)
                    {
                        finalStatusId = batchProcessedPartiallyWithBlockedLines.HandlingBatchStatus.Id;
                    }
                    else if (blockCount > 0 && failCount > 0)
                    {
                        finalStatusId = batchProcessedPartiallyWithFailedOrdersAndBlockedLines.HandlingBatchStatus.Id;
                    }
                }
                else
                {
                    if (failCount > 0 && blockCount == 0)
                    {
                        finalStatusId = batchUnprocessedBecauseOfFailedOrders.HandlingBatchStatus.Id;
                    }
                    else if (blockCount > 0 && failCount == 0)
                    {
                        finalStatusId = batchUnprocessedBecauseOfBlockedLines.HandlingBatchStatus.Id;
                    }
                    else if (blockCount > 0 && failCount > 0)
                    {
                        finalStatusId = batchUnprocessedBecauseOfFailedOrdersAndBlockedLines.HandlingBatchStatus.Id;
                    }
                }

                handlingBatch.HandlingBatchStatusId = finalStatusId;

                bulkHandlingBatches.Add(handlingBatch);
                newHandlingBatchHistories.Add(new HandlingBatchHistory()
                {
                    HandlingBatchId = handlingBatch.Id,
                    HandlingBatchStatusId = finalStatusId,
                    AbpUserId = parameters.AbpUserId,
                    TenantId = parameters.TenantId,
                    DateCreated = DateTime.Now,
                    Remarks = String.Empty
                });
            }

            await _handlingBatchLineBulkRepository.BulkUpdate(bulkHandlingBatchLines);
            await _registrationBulkRepository.BulkUpdate(bulkRegistrations);
            await _handlingBatchBulkRepository.BulkUpdate(bulkHandlingBatches);

            await _handlingBatchLineHistoryBulkRepository.BulkInsert(newHandlingBatchLineHistories);
            await _registrationHistoryBulkRepository.BulkInsert(newRegistrationHistories);
            await _handlingBatchHistoryBulkRepository.BulkInsert(newHandlingBatchHistories);
        }
    }
}
