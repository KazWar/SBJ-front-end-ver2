using Abp.Authorization;
using Abp.Domain.Repositories;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.HandlingBatch.Helpers;
using RMS.SBJ.HandlingBatch.Models;
using RMS.SBJ.HandlingBatch.Sepa;
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

namespace RMS.SBJ.HandlingBatch
{
    public class HandlingCashRefundBatchesBackgroundJob : IHandlingCashRefundBatchesBackgroundJob
    {
        private readonly IRepository<HandlingBatch, long> _handlingBatchRepository;
        private readonly IRepository<HandlingBatchLine, long> _handlingBatchLineRepository;
        private readonly IRepository<HandlingBatchHistory, long> _handlingBatchHistoryRepository;
        private readonly IRepository<Registration, long> _registrationRepository;
        private readonly IRepository<PurchaseRegistration, long> _purchaseRegistrationRepository;
        private readonly IRepository<Country, long> _countryRepository;
        private readonly IRepository<Company.Company, long> _companyRepository;
        private readonly IHandlingBatchLineBulkRepository _handlingBatchLineBulkRepository;
        private readonly IHandlingBatchLineHistoryBulkRepository _handlingBatchLineHistoryBulkRepository;
        private readonly IRegistrationBulkRepository _registrationBulkRepository;
        private readonly IRegistrationHistoryBulkRepository _registrationHistoryBulkRepository;
        private readonly IHandlingBatchStatusesAppService _handlingBatchStatusAppService;
        private readonly IHandlingBatchLineStatusesAppService _handlingBatchLineStatusAppService;
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;
        private readonly ICampaignTypesAppService _campaignTypesAppService;

        public HandlingCashRefundBatchesBackgroundJob(IRepository<HandlingBatch, long> handlingBatchRepository,
                                                      IRepository<HandlingBatchLine, long> handlingBatchLineRepository,
                                                      IRepository<HandlingBatchHistory, long> handlingBatchHistoryRepository,
                                                      IRepository<Registration, long> registrationRepository,
                                                      IRepository<PurchaseRegistration, long> purchaseRegistrationRepository,
                                                      IRepository<Country, long> countryRepository,
                                                      IRepository<Company.Company, long> companyRepository,
                                                      IHandlingBatchLineBulkRepository handlingBatchLineBulkRepository,
                                                      IHandlingBatchLineHistoryBulkRepository handlingBatchLineHistoryBulkRepository,
                                                      IRegistrationBulkRepository registrationBulkRepository,
                                                      IRegistrationHistoryBulkRepository registrationHistoryBulkRepository,
                                                      IHandlingBatchStatusesAppService handlingBatchStatusAppService,
                                                      IHandlingBatchLineStatusesAppService handlingBatchLineStatusAppService,
                                                      IRegistrationStatusesAppService registrationStatusesAppService,
                                                      ICampaignTypesAppService campaignTypesAppService)
        {
            _handlingBatchRepository = handlingBatchRepository;
            _handlingBatchLineRepository = handlingBatchLineRepository;
            _handlingBatchHistoryRepository = handlingBatchHistoryRepository;
            _registrationRepository = registrationRepository;
            _purchaseRegistrationRepository = purchaseRegistrationRepository;
            _countryRepository = countryRepository;
            _companyRepository = companyRepository;
            _handlingBatchLineBulkRepository = handlingBatchLineBulkRepository;
            _handlingBatchLineHistoryBulkRepository = handlingBatchLineHistoryBulkRepository;
            _registrationBulkRepository = registrationBulkRepository;
            _registrationHistoryBulkRepository = registrationHistoryBulkRepository;
            _handlingBatchStatusAppService = handlingBatchStatusAppService;
            _handlingBatchLineStatusAppService = handlingBatchLineStatusAppService;
            _registrationStatusesAppService = registrationStatusesAppService;
            _campaignTypesAppService = campaignTypesAppService;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<string> ExecuteSepaAsync(HandlingBatchJobParameters parameters)
        {
            if (parameters.HandlingBatchId == null)
            {
                return null;
            }

            var typeCashRefund = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.CashRefund);

            var handlingBatch = _handlingBatchRepository.GetAll().Where(h => h.Id == parameters.HandlingBatchId.Value).FirstOrDefault();

            if (handlingBatch == null || handlingBatch.CampaignTypeId != typeCashRefund.CampaignType.Id)
            {
                return null;
            }

            var handlingBatchLines = _handlingBatchLineRepository.GetAll().Where(l => l.HandlingBatchId == handlingBatch.Id);

            if (handlingBatchLines == null || handlingBatchLines.Count() == 0)
            {
                return null;
            }

            var company = _companyRepository.GetAll().First();
            var sourceData = (from o in handlingBatchLines
                              join o1 in _purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o1.Id into j1
                              from s1 in j1
                              join o2 in _registrationRepository.GetAll() on s1.RegistrationId equals o2.Id into j2
                              from s2 in j2
                              join o3 in _countryRepository.GetAll() on s2.CountryId equals o3.Id into j3
                              from s3 in j3
                              orderby s2.CampaignId, s1.RegistrationId
                              select new
                              {
                                  RegistrationId = s1.RegistrationId,
                                  Amount = o.Amount.Value,
                                  Bic = (s2.Bic ?? String.Empty).Trim(),
                                  Iban = (s2.Iban ?? String.Empty).Trim(),
                                  Name = !String.IsNullOrWhiteSpace($"{s2.FirstName} {s2.LastName}") ? $"{(s2.FirstName ?? String.Empty).Trim()} {(s2.LastName ?? String.Empty).Trim()}" : (s2.CompanyName ?? String.Empty).Trim(),
                                  Address = $"{(s2.Street ?? String.Empty).Trim()} {(s2.HouseNr ?? String.Empty).Trim()}",
                                  Postal = (s2.PostalCode ?? String.Empty).Trim(),
                                  City = (s2.City ?? String.Empty).Trim(),
                                  Country = (s3.CountryCode ?? String.Empty).Trim(),
                                  Email = (s2.EmailAddress ?? String.Empty).Trim(),
                                  Info = $"{company.Name} - Reg ID: {s1.RegistrationId}"
                               }).ToList();

            var registrationIds = sourceData.Select(x => x.RegistrationId).Distinct();

            var sepaPaymentBatch = new SepaPaymentBatch()
            {
                MessageId = handlingBatch.Id.ToString(),
                InitiatorName = parameters.SepaInitiator,
                RequestedExecutionDate = DateTime.Now,
                BIC = company.BicCashBack,
                IBAN = company.IbanCashBack
            };

            var payments = new List<Payment>();

            foreach (var registrationId in registrationIds)
            {
                var registrationLines = sourceData.Where(x => x.RegistrationId == registrationId);
                
                payments.Add(new Payment
                {
                    Id = (int)registrationLines.First().RegistrationId,
                    Amount = Math.Round(registrationLines.Sum(x => x.Amount), 2),
                    BIC = registrationLines.First().Bic,
                    IBAN = registrationLines.First().Iban,
                    Name = registrationLines.First().Name,
                    AddressLine = registrationLines.First().Address,
                    PostcalCode = registrationLines.First().Postal,
                    TownName = registrationLines.First().City,
                    Country = registrationLines.First().Country,
                    EmailAddress = registrationLines.First().Email,
                    UnstructuredInfo = registrationLines.First().Info
                });
            }

            sepaPaymentBatch.Payments = payments;

            var sepaServices = new SepaServices();
            var sepaXML = sepaServices.GenerateSepaXml(sepaPaymentBatch);

            return sepaXML;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<bool> ExecutePaidAsync(HandlingBatchJobParameters parameters)
        {
            if (parameters.HandlingBatchId == null)
            {
                return false;
            }

            var handlingBatch = _handlingBatchRepository.GetAll().Where(h => h.Id == parameters.HandlingBatchId.Value).FirstOrDefault();

            if (handlingBatch == null || handlingBatch.CampaignTypeId != 1)
            {
                return false;
            }

            var handlingBatchLines = _handlingBatchLineRepository.GetAll().Where(l => l.HandlingBatchId == handlingBatch.Id);

            if (handlingBatchLines == null || handlingBatchLines.Count() == 0)
            {
                return false;
            }

            var batchFinished = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.Finished);
            var lineFinished = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Finished);
            var regiSent = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Send);

            if (handlingBatch.HandlingBatchStatusId == batchFinished.HandlingBatchStatus.Id)
            {
                return true;
            }

            var registrationIds = (from o in handlingBatchLines
                                   join o1 in _purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()
                                   orderby s1.RegistrationId
                                   select s1.RegistrationId).Distinct().ToList();

            var bulkHandlingBatchLines = new List<HandlingBatchLine>();
            var bulkRegistrations = new List<Registration>();
            
            var newHandlingBatchLineHistories = new List<HandlingBatchLineHistory>();
            var newRegistrationHistories = new List<RegistrationHistory.RegistrationHistory>();

            foreach (var handlingBatchLine in handlingBatchLines)
            {
                handlingBatchLine.HandlingBatchLineStatusId = lineFinished.HandlingBatchLineStatus.Id;

                bulkHandlingBatchLines.Add(handlingBatchLine);
                newHandlingBatchLineHistories.Add(new HandlingBatchLineHistory()
                {
                    HandlingBatchLineId = handlingBatchLine.Id,
                    HandlingBatchLineStatusId = lineFinished.HandlingBatchLineStatus.Id,
                    AbpUserId = parameters.AbpUserId,
                    TenantId = parameters.TenantId,
                    DateCreated = DateTime.Now,
                    Remarks = String.Empty
                });
            }

            foreach (var registrationId in registrationIds)
            {
                var registration = await _registrationRepository.GetAsync(registrationId);

                registration.RegistrationStatusId = regiSent.RegistrationStatus.Id;

                bulkRegistrations.Add(registration);
                newRegistrationHistories.Add(new RegistrationHistory.RegistrationHistory()
                {
                    RegistrationId = registration.Id,
                    RegistrationStatusId = regiSent.RegistrationStatus.Id,
                    AbpUserId = parameters.AbpUserId,
                    TenantId = parameters.TenantId,
                    DateCreated = DateTime.Now,
                    Remarks = "Sent to the Bank"
                });
            }

            handlingBatch.HandlingBatchStatusId = batchFinished.HandlingBatchStatus.Id;

            await _handlingBatchLineBulkRepository.BulkUpdate(bulkHandlingBatchLines);
            await _registrationBulkRepository.BulkUpdate(bulkRegistrations);

            await _handlingBatchLineHistoryBulkRepository.BulkInsert(newHandlingBatchLineHistories);
            await _registrationHistoryBulkRepository.BulkInsert(newRegistrationHistories);

            await _handlingBatchRepository.UpdateAsync(handlingBatch);
            await _handlingBatchHistoryRepository.InsertAsync(new HandlingBatchHistory()
            {
                HandlingBatchId = handlingBatch.Id,
                HandlingBatchStatusId = batchFinished.HandlingBatchStatus.Id,
                AbpUserId = parameters.AbpUserId,
                TenantId = parameters.TenantId,
                DateCreated = DateTime.Now,
                Remarks = String.Empty
            });

            return true;
        }
    }
}
