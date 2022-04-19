using RMS.SBJ.CodeTypeTables;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.HandlingBatch.Exporting;
using RMS.SBJ.HandlingBatch.Dtos;
using RMS.SBJ.HandlingBatch.Dtos.Premium;
using RMS.SBJ.HandlingBatch.Models;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.Registrations;
using RMS.External;
using RMS.SBJ.Registrations.Helpers;
using RMS.SBJ.CampaignProcesses.Helpers;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.HandlingLines;
using System;
using Newtonsoft.Json;
using RMS.SBJ.HandlingBatch.Helpers;
using RMS.SBJ.HandlingBatch.Dtos.CashRefund;
using Abp.AspNetZeroCore.Net;
using System.IO;
using RMS.Storage;
using RMS.SBJ.HandlingBatch.Dtos.ActivationCode;
using RMS.SBJ.ActivationCodes;
using RMS.SBJ.Messaging;
using RMS.SBJ.Messaging.Helpers;
using RMS.EntityFrameworkCore.Repositories.HandlingBatchBulk;
using RMS.EntityFrameworkCore.Repositories.RegistrationBulk;

namespace RMS.SBJ.HandlingBatch
{
    [AbpAuthorize(AppPermissions.Pages_HandlingBatches, AppPermissions.Pages_HandlingBatches_ViewMenu)]
    public class HandlingBatchesAppService : RMSAppServiceBase, IHandlingBatchesAppService
    {
        private readonly IRepository<HandlingBatch, long> _handlingBatchRepository;
        private readonly IRepository<HandlingBatchLine, long> _handlingBatchLineRepository;
        private readonly IRepository<HandlingBatchHistory, long> _handlingBatchHistoryRepository;
        private readonly IRepository<HandlingBatchLineStatus, long> _handlingBatchLineStatusRepository;
        private readonly IRepository<CampaignType, long> _lookup_campaignTypeRepository;
        private readonly IRepository<HandlingBatchStatus, long> _lookup_handlingBatchStatusRepository;
        private readonly IRepository<Campaign, long> _lookup_campaignRepository;
        private readonly IRepository<Registration, long> _lookup_registrationRepository;
        private readonly IRepository<PurchaseRegistration, long> _lookup_purchaseRegistrationRepository;
        private readonly IRepository<HandlingLine, long> _lookup_handlingLineRepository;
        private readonly IRepository<Company.Company, long> _lookup_companyRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<Locale, long> _lookup_localeRepository;
        private readonly IRepository<ActivationCode, long> _lookup_activationCodeRepository;
        private readonly IRepository<MessageHistory, long> _lookup_messageHistoryRepository;
        private readonly IHandlingBatchLineBulkRepository _handlingBatchLineBulkRepository;
        private readonly IHandlingBatchLineHistoryBulkRepository _handlingBatchLineHistoryBulkRepository;
        private readonly IRegistrationBulkRepository _registrationBulkRepository;
        private readonly IRegistrationHistoryBulkRepository _registrationHistoryBulkRepository;
        private readonly IHandlingBatchesExcelExporter _handlingBatchesExcelExporter;
        private readonly IHandlingBatchStatusesAppService _handlingBatchStatusAppService;
        private readonly IHandlingBatchLineStatusesAppService _handlingBatchLineStatusAppService;
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;
        private readonly ICampaignTypesAppService _campaignTypesAppService;
        private readonly IMessagingAppService _messagingAppService;
        private readonly IHandlingPremiumBatchesBackgroundJob _handlingPremiumBatchesBackgroundJob;
        private readonly IHandlingCashRefundBatchesBackgroundJob _handlingCashRefundBatchesBackgroundJob;
        private readonly IHandlingActivationCodeBatchesBackgroundJob _handlingActivationCodeBatchesBackgroundJob;
        private readonly IScanWarehouseStatusBackgroundJob _scanWarehouseStatusBackgroundJob;
        private readonly IScanActivationCodeStatusBackgroundJob _scanActivationCodeStatusBackgroundJob;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IWmsProduct _wmsProductClient;
        private readonly IWmsOrder _wmsOrderClient;

        public HandlingBatchesAppService(IRepository<HandlingBatch, long> handlingBatchRepository,
                                         IRepository<HandlingBatchLine, long> handlingBatchLineRepository,
                                         IRepository<HandlingBatchHistory, long> handlingBatchHistoryRepository,
                                         IRepository<HandlingBatchLineStatus, long> handlingBatchLineStatusRepository,
                                         IRepository<CampaignType, long> lookup_campaignTypeRepository,
                                         IRepository<HandlingBatchStatus, long> lookup_handlingBatchStatusRepository,
                                         IRepository<Campaign, long> lookup_campaignRepository,
                                         IRepository<Registration, long> lookup_registrationRepository,
                                         IRepository<PurchaseRegistration, long> lookup_purchaseRegistrationRepository,
                                         IRepository<HandlingLine, long> lookup_handlingLineRepository,
                                         IRepository<Company.Company, long> lookup_companyRepository,
                                         IRepository<Country, long> lookup_countryRepository,
                                         IRepository<Locale, long> lookup_localeRepository,
                                         IRepository<ActivationCode, long> lookup_activationCodeRepository,
                                         IRepository<MessageHistory, long> lookup_messageHistoryRepository,
                                         IHandlingBatchLineBulkRepository handlingBatchLineBulkRepository,
                                         IHandlingBatchLineHistoryBulkRepository handlingBatchLineHistoryBulkRepository,
                                         IRegistrationBulkRepository registrationBulkRepository,
                                         IRegistrationHistoryBulkRepository registrationHistoryBulkRepository,
                                         IHandlingBatchesExcelExporter handlingBatchesExcelExporter,
                                         IHandlingBatchStatusesAppService handlingBatchStatusAppService,
                                         IHandlingBatchLineStatusesAppService handlingBatchLineStatusAppService,
                                         IRegistrationStatusesAppService registrationStatusesAppService,
                                         ICampaignTypesAppService campaignTypesAppService,
                                         IMessagingAppService messagingAppService,
                                         IHandlingPremiumBatchesBackgroundJob handlingPremiumBatchesBackgroundJob,
                                         IHandlingCashRefundBatchesBackgroundJob handlingCashRefundBatchesBackgroundJob,
                                         IHandlingActivationCodeBatchesBackgroundJob handlingActivationCodeBatchesBackgroundJob,
                                         IScanWarehouseStatusBackgroundJob scanWarehouseStatusBackgroundJob,
                                         IScanActivationCodeStatusBackgroundJob scanActivationCodeStatusBackgroundJob,
                                         ITempFileCacheManager tempFileCacheManager,
                                         IWmsProduct wmsProductClient,
                                         IWmsOrder wmsOrderClient)
        {
            _handlingBatchRepository = handlingBatchRepository;
            _handlingBatchLineRepository = handlingBatchLineRepository;
            _handlingBatchHistoryRepository = handlingBatchHistoryRepository;
            _handlingBatchLineStatusRepository = handlingBatchLineStatusRepository;
            _lookup_campaignTypeRepository = lookup_campaignTypeRepository;
            _lookup_handlingBatchStatusRepository = lookup_handlingBatchStatusRepository;
            _lookup_campaignRepository = lookup_campaignRepository;
            _lookup_registrationRepository = lookup_registrationRepository;
            _lookup_purchaseRegistrationRepository = lookup_purchaseRegistrationRepository;
            _lookup_handlingLineRepository = lookup_handlingLineRepository;
            _lookup_companyRepository = lookup_companyRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_localeRepository = lookup_localeRepository;
            _lookup_activationCodeRepository = lookup_activationCodeRepository;
            _lookup_messageHistoryRepository = lookup_messageHistoryRepository;
            _handlingBatchLineBulkRepository = handlingBatchLineBulkRepository;
            _handlingBatchLineHistoryBulkRepository = handlingBatchLineHistoryBulkRepository;
            _registrationBulkRepository = registrationBulkRepository;
            _registrationHistoryBulkRepository = registrationHistoryBulkRepository;
            _handlingBatchesExcelExporter = handlingBatchesExcelExporter;
            _handlingBatchStatusAppService = handlingBatchStatusAppService;
            _handlingBatchLineStatusAppService = handlingBatchLineStatusAppService;
            _registrationStatusesAppService = registrationStatusesAppService;
            _campaignTypesAppService = campaignTypesAppService;
            _messagingAppService = messagingAppService;
            _handlingPremiumBatchesBackgroundJob = handlingPremiumBatchesBackgroundJob;
            _handlingCashRefundBatchesBackgroundJob = handlingCashRefundBatchesBackgroundJob;
            _handlingActivationCodeBatchesBackgroundJob = handlingActivationCodeBatchesBackgroundJob;
            _scanWarehouseStatusBackgroundJob = scanWarehouseStatusBackgroundJob;
            _scanActivationCodeStatusBackgroundJob = scanActivationCodeStatusBackgroundJob;
            _tempFileCacheManager = tempFileCacheManager;
            _wmsProductClient = wmsProductClient;
            _wmsOrderClient = wmsOrderClient;
        }

        public async Task<PagedResultDto<GetHandlingBatchForViewDto>> GetAll(GetAllHandlingBatchesInput input)
        {
            var filteredHandlingBatches = _handlingBatchRepository.GetAll()
                        .Include(e => e.CampaignTypeFk)
                        .Include(e => e.HandlingBatchStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remarks.Contains(input.Filter))
                        .WhereIf(input.MinDateCreatedFilter != null, e => e.DateCreated >= input.MinDateCreatedFilter)
                        .WhereIf(input.MaxDateCreatedFilter != null, e => e.DateCreated <= input.MaxDateCreatedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeFilter), e => e.CampaignTypeFk != null && e.CampaignTypeFk.Code == input.CampaignTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HandlingBatchStatusFilter), e => e.HandlingBatchStatusFk != null && e.HandlingBatchStatusFk.StatusCode == input.HandlingBatchStatusFilter);
            
            var pagedAndFilteredHandlingBatches = filteredHandlingBatches
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var handlingBatches = from o in pagedAndFilteredHandlingBatches
                                  join o1 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_handlingBatchStatusRepository.GetAll() on o.HandlingBatchStatusId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  select new GetHandlingBatchForViewDto()
                                  {
                                      HandlingBatch = new HandlingBatchDto
                                      {
                                          Id = o.Id,
                                          Remarks = o.Remarks,
                                          DateCreated = o.DateCreated,
                                          CampaignTypeCode = s1.Code
                                      },
                                      CampaignTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      HandlingBatchStatusStatusDescription = s2 == null || s2.StatusDescription == null ? "" : s2.StatusDescription.ToString()
                                  };

            var totalCount = await filteredHandlingBatches.CountAsync();

            return new PagedResultDto<GetHandlingBatchForViewDto>(
                totalCount,
                await handlingBatches.ToListAsync()
            );
        }

        public async Task<GetHandlingBatchForViewDto> GetHandlingBatchForView(long id)
        {
            var handlingBatch = await _handlingBatchRepository.GetAsync(id);

            var output = new GetHandlingBatchForViewDto { HandlingBatch = ObjectMapper.Map<HandlingBatchDto>(handlingBatch) };

            var _lookupCampaignType = await _lookup_campaignTypeRepository.FirstOrDefaultAsync((long)output.HandlingBatch.CampaignTypeId);
            output.CampaignTypeName = _lookupCampaignType?.Name?.ToString();

            var _lookupHandlingBatchStatus = await _lookup_handlingBatchStatusRepository.FirstOrDefaultAsync((long)output.HandlingBatch.HandlingBatchStatusId);
            output.HandlingBatchStatusStatusDescription = _lookupHandlingBatchStatus?.StatusDescription?.ToString();

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<GetHandlingBatchForEditOutput> GetHandlingBatchForEdit(EntityDto<long> input)
        {
            var handlingBatch = await _handlingBatchRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHandlingBatchForEditOutput { HandlingBatch = ObjectMapper.Map<CreateOrEditHandlingBatchDto>(handlingBatch) };

            var _lookupCampaignType = await _lookup_campaignTypeRepository.FirstOrDefaultAsync((long)output.HandlingBatch.CampaignTypeId);
            output.CampaignTypeName = _lookupCampaignType?.Name?.ToString();

            var _lookupHandlingBatchStatus = await _lookup_handlingBatchStatusRepository.FirstOrDefaultAsync((long)output.HandlingBatch.HandlingBatchStatusId);
            output.HandlingBatchStatusStatusDescription = _lookupHandlingBatchStatus?.StatusDescription?.ToString();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHandlingBatchDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Create, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        protected virtual async Task Create(CreateOrEditHandlingBatchDto input)
        {
            var handlingBatch = ObjectMapper.Map<HandlingBatch>(input);

            if (AbpSession.TenantId != null)
            {
                handlingBatch.TenantId = AbpSession.TenantId;
            }

            await _handlingBatchRepository.InsertAsync(handlingBatch);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        protected virtual async Task Update(CreateOrEditHandlingBatchDto input)
        {
            var handlingBatch = await _handlingBatchRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, handlingBatch);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Delete, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task Delete(EntityDto<long> input)
        {
            await _handlingBatchRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task ScanWarehouseStatus(int warehouseId, string orderUserId, string password)
        {
            var tenantAndWarehouseParameters = new HandlingBatchJobParameters()
            {
                AbpUserId = AbpSession.UserId ?? 1,
                TenantId = AbpSession.TenantId,
                WarehouseId = warehouseId,
                OrderUserId = orderUserId,
                Password = password
            };

            await _scanWarehouseStatusBackgroundJob.ExecuteAsync(tenantAndWarehouseParameters);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task ScanSendgridStatus()
        {
            var tenantParameters = new HandlingBatchJobParameters()
            {
                AbpUserId = AbpSession.UserId ?? 1,
                TenantId = AbpSession.TenantId
            };

            await _scanActivationCodeStatusBackgroundJob.ExecuteAsync(tenantParameters);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task ProcessPremiums(int warehouseId, string orderUserId, string password)
        {
            var handlingBatchJobParameters = new HandlingBatchJobParameters()
            {
                AbpUserId = AbpSession.UserId ?? 1,
                TenantId = AbpSession.TenantId,
                WarehouseId = warehouseId,
                OrderUserId = orderUserId,
                Password = password
            };

            await _handlingPremiumBatchesBackgroundJob.ExecuteAsync(handlingBatchJobParameters);

            //await _backgroundJobManager.EnqueueAsync<HandlingPremiumBatchesBackgroundJob, PremiumBatchParameters>(premiumBatchParameters);
            //var manager = new RecurringJobManager();
            //manager.AddOrUpdate("ProcessPremiums", Job.FromExpression(() => _handlingPremiumBatchesBackgroundJob.ExecuteAsync(premiumBatchParameters)), Cron.Minutely);
            //RecurringJob.AddOrUpdate<IHandlingPremiumBatchesBackgroundJob>("ProcessPremiums", t => t.ExecuteAsync(premiumBatchParameters), Cron.Minutely);
            //CurrentUnitOfWork.SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, AbpSession.TenantId);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<FileDto> ProcessCashRefunds(long handlingBatchId, string sepaInitiator)
        {
            var handlingBatchJobParameters = new HandlingBatchJobParameters()
            {
                AbpUserId = AbpSession.UserId ?? 1,
                TenantId = AbpSession.TenantId,
                HandlingBatchId = handlingBatchId,
                SepaInitiator = !String.IsNullOrWhiteSpace(sepaInitiator) ? sepaInitiator : "Service Bureau Jansen"
            };

            var sepaXmlString = await _handlingCashRefundBatchesBackgroundJob.ExecuteSepaAsync(handlingBatchJobParameters);

            FileDto sepaXmlFile = null;

            if (!String.IsNullOrWhiteSpace(sepaXmlString))
            {
                var company = _lookup_companyRepository.GetAll().First();

                sepaXmlFile = new FileDto($"Sepa_{company.Name}_{handlingBatchId}.xml", MimeTypeNames.TextXml);

                var stream = new MemoryStream();

                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(sepaXmlString);
                    streamWriter.Flush();
                    stream.Position = 0;
                }

                _tempFileCacheManager.SetFile(sepaXmlFile.FileToken, stream.ToArray());
            }

            return sepaXmlFile;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<bool> FinishCashRefunds(long handlingBatchId)
        {
            var handlingBatchJobParameters = new HandlingBatchJobParameters()
            {
                AbpUserId = AbpSession.UserId ?? 1,
                TenantId = AbpSession.TenantId,
                HandlingBatchId = handlingBatchId
            };

            var result = await _handlingCashRefundBatchesBackgroundJob.ExecutePaidAsync(handlingBatchJobParameters);

            return result;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task ProcessActivationCodes(long handlingBatchId)
        {
            var handlingBatchJobParameters = new HandlingBatchJobParameters()
            {
                AbpUserId = AbpSession.UserId ?? 1,
                TenantId = AbpSession.TenantId,
                HandlingBatchId = handlingBatchId
            };

            await _handlingActivationCodeBatchesBackgroundJob.ExecuteAsync(handlingBatchJobParameters);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<bool> CreateAndEnqueueNewPremiumBatch(long[] input)
        {
            var typePremium = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.Premium);
            var batchPending = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.Pending);
            //var batchInProgress = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.InProgress);
            var linePending = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Pending);
            var lineCancelled = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Cancelled);
            var regiApproved = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Accepted);

            List<long> batchableRegistrationIds = input.ToList();

            if (batchableRegistrationIds == null || batchableRegistrationIds.Count == 0) { return false; }

            var batchablePurchaseRegs = (from o in _lookup_purchaseRegistrationRepository.GetAll()
                                         join o1 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()
                                         join o2 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()
                                         where batchableRegistrationIds.Contains(o.RegistrationId)
                                            && s1.RegistrationStatusId == regiApproved.RegistrationStatus.Id
                                            && (!String.IsNullOrWhiteSpace(s2.CustomerCode) && s2.CustomerCode.ToUpper().Trim() != "UNKNOWN" && s2.CustomerCode.ToUpper().Trim() != "POINTS")
                                            && s2.CampaignTypeId == typePremium.CampaignType.Id
                                         orderby o.RegistrationId, o.Id
                                         select new
                                         {
                                            PurchaseRegistrationId = o.Id,
                                            CustomerCode = s2.CustomerCode,
                                            Quantity = s2.Fixed ? s2.Quantity.Value : s2.Quantity.Value * o.Quantity
                                         }).ToList();

            var newHandlingBatchId = await _handlingBatchRepository.InsertAndGetIdAsync(new HandlingBatch()
            {
                CampaignTypeId = typePremium.CampaignType.Id,
                HandlingBatchStatusId = batchPending.HandlingBatchStatus.Id,
                TenantId = AbpSession.TenantId,
                DateCreated = DateTime.Now,
                Remarks = String.Empty
            });

            var newHandlingBatchLines = new List<HandlingBatchLine>();
            var newHandlingBatchLineHistories = new List<HandlingBatchLineHistory>();

            foreach (var batchablePurchaseReg in batchablePurchaseRegs)
            {
                newHandlingBatchLines.Add(new HandlingBatchLine() 
                {
                    HandlingBatchId = newHandlingBatchId,
                    PurchaseRegistrationId = batchablePurchaseReg.PurchaseRegistrationId,
                    HandlingBatchLineStatusId = linePending.HandlingBatchLineStatus.Id,
                    TenantId = AbpSession.TenantId,
                    CustomerCode = batchablePurchaseReg.CustomerCode,
                    Quantity = batchablePurchaseReg.Quantity
                });

                newHandlingBatchLineHistories.Add(new HandlingBatchLineHistory()
                {
                    HandlingBatchLineStatusId = linePending.HandlingBatchLineStatus.Id,
                    AbpUserId = AbpSession.UserId ?? 1,
                    TenantId = AbpSession.TenantId,
                    DateCreated = DateTime.Now,
                    Remarks = String.Empty
                });
            }

            var bulkHandlingBatchLines = await _handlingBatchLineBulkRepository.BulkInsert(newHandlingBatchLines);
            for (var i = 0; i< bulkHandlingBatchLines.Count; i++)
            {
                newHandlingBatchLineHistories[i].HandlingBatchLineId = bulkHandlingBatchLines[i].Id;
            }
            await _handlingBatchLineHistoryBulkRepository.BulkInsert(newHandlingBatchLineHistories);
            await _handlingBatchHistoryRepository.InsertAsync(new HandlingBatchHistory()
            {
                HandlingBatchId = newHandlingBatchId,
                HandlingBatchStatusId = batchPending.HandlingBatchStatus.Id,
                AbpUserId = AbpSession.UserId ?? 1,
                TenantId = AbpSession.TenantId,
                DateCreated = DateTime.Now,
                Remarks = String.Empty
            });

            return true;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<bool> CreateAndEnqueueNewCashRefundBatch(long[] input)
        {
            var typeCashRefund = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.CashRefund);
            var batchPending = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.Pending);
            //var batchInProgress = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.InProgress);
            var linePending = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Pending);
            var lineCancelled = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Cancelled);
            var regiApproved = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Accepted);
            var regiInProgress = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.InProgress);

            List<long> batchableRegistrationIds = input.ToList();

            if (batchableRegistrationIds == null || batchableRegistrationIds.Count == 0) { return false; }

            var batchablePurchaseRegs = (from o in _lookup_purchaseRegistrationRepository.GetAll()
                                         join o1 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()
                                         join o2 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()
                                         where batchableRegistrationIds.Contains(o.RegistrationId)
                                            && s1.RegistrationStatusId == regiApproved.RegistrationStatus.Id
                                            && (s2.Amount != null && s2.Amount > 0)
                                            && s2.CampaignTypeId == typeCashRefund.CampaignType.Id
                                         orderby o.RegistrationId, o.Id
                                         select new
                                         {
                                            PurchaseRegistrationId = o.Id,
                                            RefundAmount = s2.Percentage ? Math.Round(o.TotalAmount * (s2.Amount.Value / 100), 2) : s2.Fixed ? Math.Round(s2.Amount.Value, 2) : Math.Round(s2.Amount.Value * o.Quantity, 2)
                                         }).ToList();

            var newHandlingBatchId = await _handlingBatchRepository.InsertAndGetIdAsync(new HandlingBatch()
            {
                CampaignTypeId = typeCashRefund.CampaignType.Id,
                HandlingBatchStatusId = batchPending.HandlingBatchStatus.Id,
                TenantId = AbpSession.TenantId,
                DateCreated = DateTime.Now,
                Remarks = String.Empty
            });

            var newHandlingBatchLines = new List<HandlingBatchLine>();
            var newHandlingBatchLineHistories = new List<HandlingBatchLineHistory>();

            foreach (var batchablePurchaseReg in batchablePurchaseRegs)
            {
                newHandlingBatchLines.Add(new HandlingBatchLine()
                {
                    HandlingBatchId = newHandlingBatchId,
                    PurchaseRegistrationId = batchablePurchaseReg.PurchaseRegistrationId,
                    HandlingBatchLineStatusId = linePending.HandlingBatchLineStatus.Id,
                    TenantId = AbpSession.TenantId,
                    Amount = batchablePurchaseReg.RefundAmount
                });

                newHandlingBatchLineHistories.Add(new HandlingBatchLineHistory()
                {
                    HandlingBatchLineStatusId = linePending.HandlingBatchLineStatus.Id,
                    AbpUserId = AbpSession.UserId ?? 1,
                    TenantId = AbpSession.TenantId,
                    DateCreated = DateTime.Now,
                    Remarks = String.Empty
                });
            }

            var bulkHandlingBatchLines = await _handlingBatchLineBulkRepository.BulkInsert(newHandlingBatchLines);
            for (var i = 0; i < bulkHandlingBatchLines.Count; i++)
            {
                newHandlingBatchLineHistories[i].HandlingBatchLineId = bulkHandlingBatchLines[i].Id;
            }
            await _handlingBatchLineHistoryBulkRepository.BulkInsert(newHandlingBatchLineHistories);

            //deviation from other CampaignTypes: set the registrations on "in progress" immediately, because the process-equivalent "Download SEPA" is a non-committal kind of action
            var bulkRegistrations = await _lookup_registrationRepository.GetAll().Where(r => batchableRegistrationIds.Contains(r.Id)).ToListAsync();
            var newRegistrationHistories = new List<RegistrationHistory.RegistrationHistory>();
            foreach (var bulkRegistration in bulkRegistrations)
            {
                bulkRegistration.RegistrationStatusId = regiInProgress.RegistrationStatus.Id;
                newRegistrationHistories.Add(new RegistrationHistory.RegistrationHistory()
                {
                    RegistrationId = bulkRegistration.Id,
                    RegistrationStatusId = regiInProgress.RegistrationStatus.Id,
                    AbpUserId = AbpSession.UserId ?? 1,
                    TenantId = AbpSession.TenantId,
                    DateCreated = DateTime.Now,
                    Remarks = "Ready to be sent to the Bank"
                });
            }
            await _registrationBulkRepository.BulkUpdate(bulkRegistrations);
            await _registrationHistoryBulkRepository.BulkInsert(newRegistrationHistories);
            await _handlingBatchHistoryRepository.InsertAsync(new HandlingBatchHistory()
            {
                HandlingBatchId = newHandlingBatchId,
                HandlingBatchStatusId = batchPending.HandlingBatchStatus.Id,
                AbpUserId = AbpSession.UserId ?? 1,
                TenantId = AbpSession.TenantId,
                DateCreated = DateTime.Now,
                Remarks = String.Empty
            });

            return true;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<bool> CreateAndEnqueueNewActivationCodeBatch(long[] input)
        {
            var typeActivationCode = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.ActivationCode);
            var batchPending = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.Pending);
            //var batchInProgress = await _handlingBatchStatusAppService.GetByStatusCode(HandlingBatchStatusCodeHelper.InProgress);
            var linePending = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Pending);
            var lineCancelled = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Cancelled);
            var regiApproved = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Accepted);
            
            List<long> batchableRegistrationIds = input.ToList();

            if (batchableRegistrationIds == null || batchableRegistrationIds.Count == 0) { return false; }

            var occupiedActivationCodes = (from o in _handlingBatchLineRepository.GetAll()
                                           join o1 in _handlingBatchRepository.GetAll() on o.HandlingBatchId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()
                                           where s1.CampaignTypeId == typeActivationCode.CampaignType.Id
                                               && o.HandlingBatchLineStatusId != lineCancelled.HandlingBatchLineStatus.Id
                                           select o);

            var availableActivationCodes = (from o in _lookup_activationCodeRepository.GetAll()
                                            join o1 in _lookup_localeRepository.GetAll() on o.LocaleId equals o1.Id into j1
                                            from s1 in j1.DefaultIfEmpty()
                                            where !occupiedActivationCodes.Any(x => x.ActivationCodeId == o.Id)
                                            select new
                                            {
                                               ActivationCodeId = o.Id,
                                               CampaignId = o.CampaignId,
                                               LocaleId = o.LocaleId,
                                               Locale = s1.Description
                                            }).ToList();

            var activationCodeStore = new List<ActivationCodeStore>();

            foreach (var activationCode in availableActivationCodes)
            {               
                if (!activationCodeStore.Any(x => x.CampaignId == activationCode.CampaignId))
                {
                    activationCodeStore.Add(new ActivationCodeStore()
                    {
                        CampaignId = activationCode.CampaignId,
                        ActivationCodeLocalStore = new List<ActivationCodeLocalStore>()
                    });
                }

                var activationCodeStoreForCampaign = activationCodeStore.Where(x => x.CampaignId == activationCode.CampaignId).First();

                if (!activationCodeStoreForCampaign.ActivationCodeLocalStore.Any(x => x.LocaleId == activationCode.LocaleId))
                {
                    activationCodeStoreForCampaign.ActivationCodeLocalStore.Add(new ActivationCodeLocalStore()
                    {
                        LocaleId = activationCode.LocaleId,
                        Locale = activationCode.Locale,
                        ActivationCodeIds = new List<long>()
                    });
                }

                var activationCodeStoreForCampaignLocale = activationCodeStoreForCampaign.ActivationCodeLocalStore.Where(x => x.LocaleId == activationCode.LocaleId).First();

                activationCodeStoreForCampaignLocale.ActivationCodeIds.Add(activationCode.ActivationCodeId);
            }

            var batchablePurchaseRegs = (from o in _lookup_purchaseRegistrationRepository.GetAll()
                                         join o1 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()
                                         join o2 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()
                                         where batchableRegistrationIds.Contains(o.RegistrationId)
                                            && s1.RegistrationStatusId == regiApproved.RegistrationStatus.Id
                                            && (s2.ActivationCode == true)
                                            && s2.CampaignTypeId == typeActivationCode.CampaignType.Id
                                         orderby o.RegistrationId, o.Id
                                         select new
                                         {
                                            PurchaseRegId = o.Id,
                                            RegistrationId = o.RegistrationId,
                                            CampaignId = s1.CampaignId,
                                            LocaleId = s1.LocaleId,
                                            Fixed = s2.Fixed
                                         }).ToList();

            var newHandlingBatchId = await _handlingBatchRepository.InsertAndGetIdAsync(new HandlingBatch()
            {
                CampaignTypeId = typeActivationCode.CampaignType.Id,
                HandlingBatchStatusId = batchPending.HandlingBatchStatus.Id,
                TenantId = AbpSession.TenantId,
                DateCreated = DateTime.Now,
                Remarks = String.Empty
            });

            var newHandlingBatchLines = new List<HandlingBatchLine>();
            var newHandlingBatchLineHistories = new List<HandlingBatchLineHistory>();

            foreach (var batchableRegistrationId in batchableRegistrationIds)
            {
                var registrationBatchablePurchaseRegs = batchablePurchaseRegs.Where(p => p.RegistrationId == batchableRegistrationId);

                if (registrationBatchablePurchaseRegs.First().Fixed)
                {
                    //Fixed = true  => pick 1 activation code for this registration, simply link to the first purchaseReg of this registration
                    //Fixed = false => pick 1 activation code for each purchaseReg of this registration
                    registrationBatchablePurchaseRegs = registrationBatchablePurchaseRegs.Take(1);
                }

                foreach (var registrationBatchablePurchaseReg in registrationBatchablePurchaseRegs)
                {
                    var activationCodeStoreForCampaign = activationCodeStore.Where(x => x.CampaignId == registrationBatchablePurchaseReg.CampaignId).FirstOrDefault();

                    if (activationCodeStoreForCampaign == null || activationCodeStoreForCampaign.ActivationCodeLocalStore.Count() == 0)
                    {
                        continue;
                    }

                    var activationCodeStoreForCampaignLocale = activationCodeStoreForCampaign.ActivationCodeLocalStore.Where(x => x.LocaleId == registrationBatchablePurchaseReg.LocaleId).FirstOrDefault();

                    if (activationCodeStoreForCampaignLocale == null || activationCodeStoreForCampaignLocale.ActivationCodeIds.Count() == 0)
                    {
                        //there are no codes available for this registration's original locale, so check for "global" availability
                        activationCodeStoreForCampaignLocale = activationCodeStoreForCampaign.ActivationCodeLocalStore.Where(x => x.Locale.ToLower().Contains("global")).FirstOrDefault();

                        if (activationCodeStoreForCampaignLocale == null || activationCodeStoreForCampaignLocale.ActivationCodeIds.Count() == 0)
                        {
                            continue;
                        }
                    }

                    var pickActivationCodeId = activationCodeStoreForCampaignLocale.ActivationCodeIds.First();

                    newHandlingBatchLines.Add(new HandlingBatchLine()
                    {
                        HandlingBatchId = newHandlingBatchId,
                        PurchaseRegistrationId = registrationBatchablePurchaseReg.PurchaseRegId,
                        HandlingBatchLineStatusId = linePending.HandlingBatchLineStatus.Id,
                        TenantId = AbpSession.TenantId,
                        ActivationCodeId = pickActivationCodeId
                    });

                    newHandlingBatchLineHistories.Add(new HandlingBatchLineHistory()
                    {
                        HandlingBatchLineStatusId = linePending.HandlingBatchLineStatus.Id,
                        AbpUserId = AbpSession.UserId ?? 1,
                        TenantId = AbpSession.TenantId,
                        DateCreated = DateTime.Now,
                        Remarks = String.Empty
                    });

                    //picked, so remove from the store
                    activationCodeStoreForCampaignLocale.ActivationCodeIds.Remove(pickActivationCodeId);
                }
            }

            var bulkHandlingBatchLines = await _handlingBatchLineBulkRepository.BulkInsert(newHandlingBatchLines);
            for (var i = 0; i < bulkHandlingBatchLines.Count; i++)
            {
                newHandlingBatchLineHistories[i].HandlingBatchLineId = bulkHandlingBatchLines[i].Id;
            }
            await _handlingBatchLineHistoryBulkRepository.BulkInsert(newHandlingBatchLineHistories);
            await _handlingBatchHistoryRepository.InsertAsync(new HandlingBatchHistory()
            {
                HandlingBatchId = newHandlingBatchId,
                HandlingBatchStatusId = batchPending.HandlingBatchStatus.Id,
                AbpUserId = AbpSession.UserId ?? 1,
                TenantId = AbpSession.TenantId,
                DateCreated = DateTime.Now,
                Remarks = String.Empty
            });

            return true;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<GetInformationForNewPremiumBatchOutput> GetInformationForNewPremiumBatch(GetInformationForNewPremiumBatchInput input)
        {
            var info = new GetInformationForNewPremiumBatchOutput();
            var infoCampaigns = new List<CampaignInformationForNewPremiumBatch>();
            var infoTotalPremiums = new List<PremiumInformationForNewHandlingBatch>();
            var infoBatchableRegistrationIds = new List<long>();
            var infoTotalApproved = 0;

            List<CampaignBatchable> inputCampaignBatchables = null;

            try
            {
                inputCampaignBatchables = !String.IsNullOrWhiteSpace(input.CampaignBatchables) ? JsonConvert.DeserializeObject<List<CampaignBatchable>>(input.CampaignBatchables) : null;
            }
            catch (Exception ex)
            {
                //never mind, just carry on...
            }

            //gather the data, including the available stock (WMS_WCF call)
            var typePremium = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.Premium);
            var lineCancelled = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Cancelled);
            var regiApproved = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Accepted);

            var stockInfo = _wmsProductClient.GetStockInfo(input.WarehouseId, input.OrderUserId, input.Password).Result;

            var occupiedPurchaseRegs = (from o in _handlingBatchLineRepository.GetAll()
                                        join o1 in _handlingBatchRepository.GetAll() on o.HandlingBatchId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()
                                        where s1.CampaignTypeId == typePremium.CampaignType.Id
                                            && o.HandlingBatchLineStatusId != lineCancelled.HandlingBatchLineStatus.Id
                                        select o);

            //temporary Makita filter: s1.Id > 3118204
            var approvedPurchaseRegsWithDeliveries = (from o in _lookup_purchaseRegistrationRepository.GetAll()
                                                      join o1 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                                                      from s1 in j1.DefaultIfEmpty()
                                                      join o2 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o2.Id into j2
                                                      from s2 in j2.DefaultIfEmpty()
                                                      where s1.RegistrationStatusId == regiApproved.RegistrationStatus.Id
                                                         && (!String.IsNullOrWhiteSpace(s2.CustomerCode) && s2.CustomerCode.ToUpper().Trim() != "UNKNOWN" && s2.CustomerCode.ToUpper().Trim() != "POINTS")
                                                         && !occupiedPurchaseRegs.Any(x => x.PurchaseRegistrationId == o.Id)
                                                         && s2.CampaignTypeId == typePremium.CampaignType.Id
                                                      orderby o.RegistrationId, o.Id
                                                      select new
                                                      {
                                                         PurchaseRegId = o.Id,
                                                         RegistrationId = o.RegistrationId,
                                                         CampaignId = s1.CampaignId,
                                                         CustomerCode = s2.CustomerCode,
                                                         QuantityToDeliver = s2.Fixed ? s2.Quantity.Value : s2.Quantity.Value * o.Quantity
                                                      }).ToList();

            infoTotalApproved = approvedPurchaseRegsWithDeliveries.Select(p => p.RegistrationId).Distinct().Count();

            if (infoTotalApproved == 0)
            {
                info.CampaignInformation = infoCampaigns;
                info.TotalPremiumInformation = infoTotalPremiums;
                info.TotalApprovedRegistrationsCount = infoTotalApproved;
                info.TotalBatchableRegistrationsCount = infoBatchableRegistrationIds.Count();
                info.TotalBatchableRegistrations = String.Join(",", infoBatchableRegistrationIds);
                info.AllIsDeliverable = !infoTotalPremiums.Any(p => p.QuantityToDeliver > p.QuantityInStock);

                //exit!
                return info;
            }

            //recount infoTotalApproved due to the 500-limit
            infoTotalApproved = 0;

            var campaignIdsWithMinRegistrationIds = (from r in approvedPurchaseRegsWithDeliveries
                                                     group r by r.CampaignId into c
                                                     select new
                                                     {
                                                        CampaignId = c.Key,
                                                        MinRegistrationId = c.Min(x => x.RegistrationId)
                                                     }).ToList();

            var campaignIdsWithMinRegistrationIdsSorted = campaignIdsWithMinRegistrationIds.OrderBy(c => c.MinRegistrationId);

            foreach (var campaignKey in campaignIdsWithMinRegistrationIdsSorted)
            {
                var campaign = await _lookup_campaignRepository.GetAsync(campaignKey.CampaignId);
                var campaignApprovedPurchaseRegsWithDeliveries = approvedPurchaseRegsWithDeliveries.Where(r => r.CampaignId == campaign.Id);
                var campaignApprovedRegistrationIds = campaignApprovedPurchaseRegsWithDeliveries.Select(p => p.RegistrationId).Distinct().Take(500).ToList();
                var campaignBatchableRegistrationIds = new List<long>();
                var campaignBatchableLimit = campaignApprovedRegistrationIds.Count;

                infoTotalApproved += campaignApprovedRegistrationIds.Count;

                var infoCampaignPremiums = new List<PremiumInformationForNewHandlingBatch>();

                if (inputCampaignBatchables != null && inputCampaignBatchables.Any(i => i.CampaignId == campaign.Id))
                {
                    var inputCampaignBatchableRegistrationsCount = inputCampaignBatchables.Where(i => i.CampaignId == campaignKey.CampaignId).First().BatchableRegistrationsCount;

                    if (inputCampaignBatchableRegistrationsCount < 0) { inputCampaignBatchableRegistrationsCount = 0; }

                    if (inputCampaignBatchableRegistrationsCount < campaignApprovedRegistrationIds.Count)
                    {
                        campaignBatchableLimit = inputCampaignBatchableRegistrationsCount;
                    }
                }

                //premiums identification
                foreach (var campaignApprovedRegistrationId in campaignApprovedRegistrationIds)
                {
                    var registrationPurchaseRegsWithDeliveries = campaignApprovedPurchaseRegsWithDeliveries.Where(p => p.RegistrationId == campaignApprovedRegistrationId);

                    foreach (var registrationPurchaseRegWithDeliveries in registrationPurchaseRegsWithDeliveries)
                    {
                        var customerCode = registrationPurchaseRegWithDeliveries.CustomerCode;
                        var premiumStock = stockInfo != null && stockInfo.ProductStockList != null ? stockInfo.ProductStockList.Where(s => s.CustomerCode == customerCode).FirstOrDefault() : null;
                        var quantityInStock = premiumStock != null ? premiumStock.AmountOnStock : 0;

                        //campaign premiums
                        if (!infoCampaignPremiums.Any(p => p.CustomerCode == customerCode))
                        {
                            infoCampaignPremiums.Add(new PremiumInformationForNewHandlingBatch()
                            {
                                CustomerCode = customerCode,
                                QuantityToDeliver = 0,
                                QuantityInStock = quantityInStock
                            });
                        }

                        //total premiums
                        if (!infoTotalPremiums.Any(p => p.CustomerCode == customerCode))
                        {
                            infoTotalPremiums.Add(new PremiumInformationForNewHandlingBatch()
                            {
                                CustomerCode = customerCode,
                                QuantityToDeliver = 0,
                                QuantityInStock = quantityInStock
                            });
                        }
                    }
                }

                //start building campaignBatchableRegistrationIds
                foreach (var campaignApprovedRegistrationId in campaignApprovedRegistrationIds)
                {
                    //only include this registration if we are still within the limit AND if it is FULLY deliverable (look at infoTotalPremiums while calculating)
                    if (campaignBatchableRegistrationIds.Count == campaignBatchableLimit) { break; }

                    var includeRegistration = true;
                    var registrationPurchaseRegsWithDeliveries = campaignApprovedPurchaseRegsWithDeliveries.Where(p => p.RegistrationId == campaignApprovedRegistrationId);

                    foreach (var registrationPurchaseRegWithDeliveries in registrationPurchaseRegsWithDeliveries)
                    {
                        var customerCode = registrationPurchaseRegWithDeliveries.CustomerCode;
                        var totalPremium = infoTotalPremiums.Where(p => p.CustomerCode == customerCode).First();
                        var quantityInStock = totalPremium.QuantityInStock;
                        var quantityToDeliver = registrationPurchaseRegWithDeliveries.QuantityToDeliver;

                        if (!totalPremium.QuantityCalculation.HasValue) { totalPremium.QuantityCalculation = totalPremium.QuantityToDeliver; }

                        quantityToDeliver += totalPremium.QuantityCalculation.Value;
                        totalPremium.QuantityCalculation = quantityToDeliver;
                        
                        if (quantityInStock < quantityToDeliver)
                        {
                            includeRegistration = false;
                            break;
                        }
                    }

                    if (includeRegistration)
                    {
                        campaignBatchableRegistrationIds.Add(campaignApprovedRegistrationId);

                        foreach (var registrationPurchaseRegWithDeliveries in registrationPurchaseRegsWithDeliveries)
                        {
                            var customerCode = registrationPurchaseRegWithDeliveries.CustomerCode;

                            //update campaign premiums
                            var campaignPremium = infoCampaignPremiums.Where(p => p.CustomerCode == customerCode).First();

                            campaignPremium.QuantityToDeliver += registrationPurchaseRegWithDeliveries.QuantityToDeliver;


                            //update total premiums
                            var totalPremium = infoTotalPremiums.Where(p => p.CustomerCode == customerCode).First();

                            totalPremium.QuantityToDeliver += registrationPurchaseRegWithDeliveries.QuantityToDeliver;
                        }
                    }

                    //reset QuantityCalculations
                    foreach (var totalPremium in infoTotalPremiums)
                    {
                        totalPremium.QuantityCalculation = null;
                    }
                }

                infoBatchableRegistrationIds.AddRange(campaignBatchableRegistrationIds);

                infoCampaigns.Add(new CampaignInformationForNewPremiumBatch()
                {
                    CampaignId = campaign.Id,
                    CampaignName = campaign.Name,
                    ApprovedRegistrationsCount = campaignApprovedRegistrationIds.Count,
                    BatchableRegistrationsCount = campaignBatchableRegistrationIds.Count,
                    PremiumInformation = infoCampaignPremiums
                });
            }

            info.CampaignInformation = infoCampaigns;
            info.TotalPremiumInformation = infoTotalPremiums;
            info.TotalApprovedRegistrationsCount = infoTotalApproved;
            info.TotalBatchableRegistrationsCount = infoBatchableRegistrationIds.Count();
            info.TotalBatchableRegistrations = String.Join(",", infoBatchableRegistrationIds);
            info.AllIsDeliverable = !infoTotalPremiums.Any(p => p.QuantityToDeliver > p.QuantityInStock);
            info.WarehouseId = input.WarehouseId;
            info.OrderUserId = input.OrderUserId;
            info.Password = input.Password;

            return info;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<GetInformationForNewCashRefundBatchOutput> GetInformationForNewCashRefundBatch(GetInformationForNewCashRefundBatchInput input)
        {
            var info = new GetInformationForNewCashRefundBatchOutput();
            var infoCampaigns = new List<CampaignInformationForNewCashRefundBatch>();
            var infoBatchableRegistrationIds = new List<long>();
            var infoTotalApproved = 0;

            decimal infoTotalRefund = 0;

            List<CampaignBatchable> inputCampaignBatchables = null;

            try
            {
                inputCampaignBatchables = !String.IsNullOrWhiteSpace(input.CampaignBatchables) ? JsonConvert.DeserializeObject<List<CampaignBatchable>>(input.CampaignBatchables) : null;
            }
            catch (Exception ex)
            {
                //never mind, just carry on...
            }

            var typeCashRefund = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.CashRefund);
            var lineCancelled = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Cancelled);
            var regiApproved = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Accepted);

            var occupiedPurchaseRegs = (from o in _handlingBatchLineRepository.GetAll()
                                        join o1 in _handlingBatchRepository.GetAll() on o.HandlingBatchId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()
                                        where s1.CampaignTypeId == typeCashRefund.CampaignType.Id
                                            && o.HandlingBatchLineStatusId != lineCancelled.HandlingBatchLineStatus.Id
                                        select o);

            var approvedPurchaseRegsWithRefunds = (from o in _lookup_purchaseRegistrationRepository.GetAll()
                                                   join o1 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                                                   from s1 in j1.DefaultIfEmpty()
                                                   join o2 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o2.Id into j2
                                                   from s2 in j2.DefaultIfEmpty()
                                                   where s1.RegistrationStatusId == regiApproved.RegistrationStatus.Id
                                                      && (s2.Amount != null && s2.Amount > 0)
                                                      && !occupiedPurchaseRegs.Any(x => x.PurchaseRegistrationId == o.Id)
                                                      && s2.CampaignTypeId == typeCashRefund.CampaignType.Id
                                                   orderby o.RegistrationId, o.Id
                                                   select new
                                                   {
                                                      PurchaseRegId = o.Id,
                                                      RegistrationId = o.RegistrationId,
                                                      CampaignId = s1.CampaignId,
                                                      RefundAmount = s2.Percentage ? Math.Round(o.TotalAmount * (s2.Amount.Value / 100), 2) : s2.Fixed ? Math.Round(s2.Amount.Value, 2) : Math.Round(s2.Amount.Value * o.Quantity, 2)
                                                   }).ToList();

            infoTotalApproved = approvedPurchaseRegsWithRefunds.Select(p => p.RegistrationId).Distinct().Count();

            if (infoTotalApproved == 0)
            {
                info.CampaignInformation = infoCampaigns;
                info.TotalApprovedRegistrationsCount = infoTotalApproved;
                info.TotalBatchableRegistrationsCount = infoBatchableRegistrationIds.Count();
                info.TotalBatchableRegistrations = String.Join(",", infoBatchableRegistrationIds);
                info.TotalRefundAmount = infoTotalRefund;

                //exit!
                return info;
            }

            var campaignIdsWithMinRegistrationIds = (from r in approvedPurchaseRegsWithRefunds
                                                     group r by r.CampaignId into c
                                                     select new
                                                     {
                                                        CampaignId = c.Key,
                                                        MinRegistrationId = c.Min(x => x.RegistrationId)
                                                     }).ToList();

            var campaignIdsWithMinRegistrationIdsSorted = campaignIdsWithMinRegistrationIds.OrderBy(c => c.MinRegistrationId);

            foreach (var campaignKey in campaignIdsWithMinRegistrationIdsSorted)
            {
                var campaign = await _lookup_campaignRepository.GetAsync(campaignKey.CampaignId);
                var campaignApprovedPurchaseRegsWithRefunds = approvedPurchaseRegsWithRefunds.Where(r => r.CampaignId == campaign.Id);
                var campaignApprovedRegistrationIds = campaignApprovedPurchaseRegsWithRefunds.Select(p => p.RegistrationId).Distinct().ToList();
                var campaignBatchableRegistrationIds = new List<long>();
                var campaignBatchableLimit = campaignApprovedRegistrationIds.Count;

                decimal infoCampaignRefund = 0;

                if (inputCampaignBatchables != null && inputCampaignBatchables.Any(i => i.CampaignId == campaign.Id))
                {
                    var inputCampaignBatchableRegistrationsCount = inputCampaignBatchables.Where(i => i.CampaignId == campaignKey.CampaignId).First().BatchableRegistrationsCount;

                    if (inputCampaignBatchableRegistrationsCount < 0) { inputCampaignBatchableRegistrationsCount = 0; }

                    if (inputCampaignBatchableRegistrationsCount < campaignApprovedRegistrationIds.Count)
                    {
                        campaignBatchableLimit = inputCampaignBatchableRegistrationsCount;
                    }
                }

                foreach (var campaignApprovedRegistrationId in campaignApprovedRegistrationIds)
                {
                    if (campaignBatchableRegistrationIds.Count == campaignBatchableLimit) { break; }

                    var registrationPurchaseRegsWithRefunds = campaignApprovedPurchaseRegsWithRefunds.Where(p => p.RegistrationId == campaignApprovedRegistrationId);

                    campaignBatchableRegistrationIds.Add(campaignApprovedRegistrationId);

                    foreach (var registrationPurchaseRegWithRefunds in registrationPurchaseRegsWithRefunds)
                    {
                        infoCampaignRefund += registrationPurchaseRegWithRefunds.RefundAmount;
                        infoTotalRefund += registrationPurchaseRegWithRefunds.RefundAmount;
                    }
                }

                infoBatchableRegistrationIds.AddRange(campaignBatchableRegistrationIds);

                infoCampaigns.Add(new CampaignInformationForNewCashRefundBatch()
                {
                    CampaignId = campaign.Id,
                    CampaignName = campaign.Name,
                    ApprovedRegistrationsCount = campaignApprovedRegistrationIds.Count,
                    BatchableRegistrationsCount = campaignBatchableRegistrationIds.Count,
                    RefundAmount = infoCampaignRefund
                });
            }

            info.CampaignInformation = infoCampaigns;
            info.TotalApprovedRegistrationsCount = infoTotalApproved;
            info.TotalBatchableRegistrationsCount = infoBatchableRegistrationIds.Count();
            info.TotalBatchableRegistrations = String.Join(",", infoBatchableRegistrationIds);
            info.TotalRefundAmount = infoTotalRefund;

            return info;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches_Edit, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<GetInformationForNewActivationCodeBatchOutput> GetInformationForNewActivationCodeBatch(GetInformationForNewActivationCodeBatchInput input)
        {
            var info = new GetInformationForNewActivationCodeBatchOutput();
            var infoCampaigns = new List<CampaignInformationForNewActivationCodeBatch>();
            var infoTotalActivationCodes = new List<ActivationCodeInformationForNewHandlingBatch>();
            var infoBatchableRegistrationIds = new List<long>();
            var infoTotalApproved = 0;

            List<CampaignBatchable> inputCampaignBatchables = null;

            try
            {
                inputCampaignBatchables = !String.IsNullOrWhiteSpace(input.CampaignBatchables) ? JsonConvert.DeserializeObject<List<CampaignBatchable>>(input.CampaignBatchables) : null;
            }
            catch (Exception ex)
            {
                //never mind, just carry on...
            }

            //gather the data, including the available activation codes per campaign & locale
            var typeActivationCode = await _campaignTypesAppService.GetByCode(CampaignTypeHelper.ActivationCode);
            var lineCancelled = await _handlingBatchLineStatusAppService.GetByStatusCode(HandlingBatchLineStatusCodeHelper.Cancelled);
            var regiApproved = await _registrationStatusesAppService.GetByStatusCode(RegistrationStatusCodeHelper.Accepted);

            var occupiedActivationCodes = (from o in _handlingBatchLineRepository.GetAll()
                                           join o1 in _handlingBatchRepository.GetAll() on o.HandlingBatchId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()
                                           where s1.CampaignTypeId == typeActivationCode.CampaignType.Id
                                               && o.HandlingBatchLineStatusId != lineCancelled.HandlingBatchLineStatus.Id
                                           select o);

            var availableActivationCodesPerCampaignLocale = (from o in _lookup_activationCodeRepository.GetAll()
                                                             where !occupiedActivationCodes.Any(x => x.ActivationCodeId == o.Id)
                                                             group o by new { o.CampaignId, o.LocaleId } into c
                                                             select new
                                                             {
                                                                CampaignId = c.Key.CampaignId,
                                                                LocaleId = c.Key.LocaleId,
                                                                AvailableCodes = c.Count()
                                                             }).ToList();

            var availableActivationCodesPerLocale = (from o in availableActivationCodesPerCampaignLocale
                                                     group o by o.LocaleId into c
                                                     select new
                                                     {
                                                        LocaleId = c.Key,
                                                        AvailableCodes = c.Sum(x => x.AvailableCodes)
                                                     }).ToList();

            var occupiedPurchaseRegs = (from o in _handlingBatchLineRepository.GetAll()
                                        join o1 in _handlingBatchRepository.GetAll() on o.HandlingBatchId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()
                                        where s1.CampaignTypeId == typeActivationCode.CampaignType.Id
                                            && o.HandlingBatchLineStatusId != lineCancelled.HandlingBatchLineStatus.Id
                                        select o);

            var approvedPurchaseRegsWithDeliveries = (from o in _lookup_purchaseRegistrationRepository.GetAll()
                                                      join o1 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                                                      from s1 in j1.DefaultIfEmpty()
                                                      join o2 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o2.Id into j2
                                                      from s2 in j2.DefaultIfEmpty()
                                                      join o3 in _lookup_localeRepository.GetAll() on s1.LocaleId equals o3.Id into j3
                                                      from s3 in j3.DefaultIfEmpty()
                                                      where s1.RegistrationStatusId == regiApproved.RegistrationStatus.Id
                                                         && (s2.ActivationCode == true)
                                                         && !occupiedPurchaseRegs.Any(x => x.PurchaseRegistrationId == o.Id)
                                                         && s2.CampaignTypeId == typeActivationCode.CampaignType.Id
                                                      orderby o.RegistrationId, o.Id
                                                      select new
                                                      {
                                                         PurchaseRegId = o.Id,
                                                         RegistrationId = o.RegistrationId,
                                                         CampaignId = s1.CampaignId,
                                                         LocaleId = s1.LocaleId,
                                                         Locale = s3.Description,
                                                         Fixed = s2.Fixed
                                                      }).ToList();

            infoTotalApproved = approvedPurchaseRegsWithDeliveries.Select(p => p.RegistrationId).Distinct().Count();

            if (infoTotalApproved == 0)
            {
                info.CampaignInformation = infoCampaigns;
                info.TotalActivationCodeInformation = infoTotalActivationCodes;
                info.TotalApprovedRegistrationsCount = infoTotalApproved;
                info.TotalBatchableRegistrationsCount = infoBatchableRegistrationIds.Count();
                info.TotalBatchableRegistrations = String.Join(",", infoBatchableRegistrationIds);
                info.AllIsDeliverable = !infoTotalActivationCodes.Any(p => p.ActivationCodesToDeliver > p.ActivationCodesInStore);

                //exit!
                return info;
            }

            foreach (var localeKey in availableActivationCodesPerLocale)
            {
                var locale = await _lookup_localeRepository.GetAsync(localeKey.LocaleId);
                
                infoTotalActivationCodes.Add(new ActivationCodeInformationForNewHandlingBatch()
                {
                    Locale = locale.Description,
                    ActivationCodesToDeliver = 0,
                    ActivationCodesInStore = localeKey.AvailableCodes 
                });
            }

            var campaignIdsWithMinRegistrationIds = (from r in approvedPurchaseRegsWithDeliveries
                                                     group r by r.CampaignId into c
                                                     select new
                                                     {
                                                        CampaignId = c.Key,
                                                        MinRegistrationId = c.Min(x => x.RegistrationId)
                                                     }).ToList();

            var campaignIdsWithMinRegistrationIdsSorted = campaignIdsWithMinRegistrationIds.OrderBy(c => c.MinRegistrationId);
            var campaignInfoRemarks = new List<string>();

            foreach (var campaignKey in campaignIdsWithMinRegistrationIdsSorted)
            {
                var campaign = await _lookup_campaignRepository.GetAsync(campaignKey.CampaignId);
                var campaignApprovedPurchaseRegsWithDeliveries = approvedPurchaseRegsWithDeliveries.Where(r => r.CampaignId == campaign.Id);
                var campaignApprovedRegistrationIds = campaignApprovedPurchaseRegsWithDeliveries.Select(p => p.RegistrationId).Distinct().ToList();
                var campaignBatchableRegistrationIds = new List<long>();
                var campaignBatchableLimit = campaignApprovedRegistrationIds.Count;

                var infoCampaignActivationCodes = new List<ActivationCodeInformationForNewHandlingBatch>();

                if (inputCampaignBatchables != null && inputCampaignBatchables.Any(i => i.CampaignId == campaign.Id))
                {
                    var inputCampaignBatchableRegistrationsCount = inputCampaignBatchables.Where(i => i.CampaignId == campaignKey.CampaignId).First().BatchableRegistrationsCount;

                    if (inputCampaignBatchableRegistrationsCount < 0) { inputCampaignBatchableRegistrationsCount = 0; }

                    if (inputCampaignBatchableRegistrationsCount < campaignApprovedRegistrationIds.Count)
                    {
                        campaignBatchableLimit = inputCampaignBatchableRegistrationsCount;
                    }
                }

                foreach (var campaignLocaleKey in availableActivationCodesPerCampaignLocale.Where(x => x.CampaignId == campaign.Id))
                {
                    var locale = await _lookup_localeRepository.GetAsync(campaignLocaleKey.LocaleId);
                    
                    infoCampaignActivationCodes.Add(new ActivationCodeInformationForNewHandlingBatch()
                    {
                        Locale = locale.Description,
                        ActivationCodesToDeliver = 0,
                        ActivationCodesInStore = campaignLocaleKey.AvailableCodes 
                    });
                }

                foreach (var campaignApprovedRegistrationId in campaignApprovedRegistrationIds)
                {
                    //only include this registration if we are still within the limit AND if it is FULLY deliverable
                    if (campaignBatchableRegistrationIds.Count == campaignBatchableLimit) { break; }

                    var registrationPurchaseRegsWithDeliveries = campaignApprovedPurchaseRegsWithDeliveries.Where(p => p.RegistrationId == campaignApprovedRegistrationId);
                    var activationCodesInLocalStore = infoCampaignActivationCodes.Where(x => x.Locale == registrationPurchaseRegsWithDeliveries.First().Locale).FirstOrDefault();

                    if (activationCodesInLocalStore == null)
                    {
                        //there are no codes available for this registration's original locale, so check for "global" availability
                        activationCodesInLocalStore = infoCampaignActivationCodes.Where(x => x.Locale.ToLower().Contains("global")).FirstOrDefault();

                        if (activationCodesInLocalStore == null)
                        {
                            var campaignInfoRemark = $"no activation codes available for either global or {registrationPurchaseRegsWithDeliveries.First().Locale}";

                            if (!campaignInfoRemarks.Contains(campaignInfoRemark))
                            {
                                campaignInfoRemarks.Add(campaignInfoRemark);
                            }

                            continue;
                        }
                    }

                    //Fixed = true  => pick 1 activation code for this registration
                    //Fixed = false => pick 1 activation code for each purchaseReg of this registration
                    var registrationActivationCodesToDeliver = registrationPurchaseRegsWithDeliveries.First().Fixed ? 1 : registrationPurchaseRegsWithDeliveries.Count();
                    var campaignActivationCodesToDeliver = activationCodesInLocalStore.ActivationCodesToDeliver + registrationActivationCodesToDeliver;

                    if (campaignActivationCodesToDeliver <= activationCodesInLocalStore.ActivationCodesInStore)
                    {
                        //include registration
                        campaignBatchableRegistrationIds.Add(campaignApprovedRegistrationId);

                        infoCampaignActivationCodes.Where(x => x.Locale == activationCodesInLocalStore.Locale).First().ActivationCodesToDeliver += registrationActivationCodesToDeliver;
                        infoTotalActivationCodes.Where(x => x.Locale == activationCodesInLocalStore.Locale).First().ActivationCodesToDeliver += registrationActivationCodesToDeliver;
                    }
                }

                infoBatchableRegistrationIds.AddRange(campaignBatchableRegistrationIds);

                infoCampaigns.Add(new CampaignInformationForNewActivationCodeBatch()
                {
                    CampaignId = campaign.Id,
                    CampaignName = campaign.Name,
                    ApprovedRegistrationsCount = campaignApprovedRegistrationIds.Count,
                    BatchableRegistrationsCount = campaignBatchableRegistrationIds.Count,
                    ActivationCodeInformation = infoCampaignActivationCodes,
                    Remarks = campaignInfoRemarks
                });
            }

            info.CampaignInformation = infoCampaigns;
            info.TotalActivationCodeInformation = infoTotalActivationCodes;
            info.TotalApprovedRegistrationsCount = infoTotalApproved;
            info.TotalBatchableRegistrationsCount = infoBatchableRegistrationIds.Count();
            info.TotalBatchableRegistrations = String.Join(",", infoBatchableRegistrationIds);
            info.AllIsDeliverable = !infoTotalActivationCodes.Any(p => p.ActivationCodesToDeliver > p.ActivationCodesInStore);

            return info;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<GetPremiumBatchForView> GetPremiumBatchForView(GetPremiumBatchForData input)
        {
            var premiumBatch = await _handlingBatchRepository.FirstOrDefaultAsync(input.Id);
            if (premiumBatch == null)
            {
                return null;
            }

            var premiumBatchStatus = await _handlingBatchStatusAppService.GetById(premiumBatch.HandlingBatchStatusId);
            var premiumBatchForView = new GetPremiumBatchForView()
            {
                Id = premiumBatch.Id,
                StatusDescription = premiumBatchStatus.HandlingBatchStatus.StatusDescription,
                CreatedOn = premiumBatch.DateCreated,
                Remarks = premiumBatch.Remarks,
                WarehouseId = input.WarehouseId,
                OrderUserId = input.OrderUserId,
                Password = input.Password 
            };

            var premiumBatchCampaignsForView = new List<PremiumBatchCampaignForView>();
            var premiumBatchDataForView = (from o in _handlingBatchLineRepository.GetAll()
                                           join o1 in _handlingBatchLineStatusRepository.GetAll() on o.HandlingBatchLineStatusId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()
                                           join o2 in _lookup_purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o2.Id into j2
                                           from s2 in j2.DefaultIfEmpty()
                                           where o.HandlingBatchId == premiumBatch.Id
                                           select new
                                           {
                                               RegistrationId = s2.RegistrationId,
                                               OrderId = o.ExternalOrderId
                                           }).ToList();

            premiumBatchForView.RegistrationsCount = premiumBatchDataForView.Select(x => x.RegistrationId).Distinct().Count();
            premiumBatchForView.OrdersCount = premiumBatchDataForView.Where(x => !String.IsNullOrWhiteSpace(x.OrderId)).Select(x => x.OrderId).Distinct().Count();

            return premiumBatchForView;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<FileDto> GetPremiumBatchToExcel(GetPremiumBatchForData input)
        {
            var premiumBatch = await _handlingBatchRepository.FirstOrDefaultAsync(input.Id);
            if (premiumBatch == null)
            {
                return null;
            }

            var premiumBatchCampaignsForView = new List<PremiumBatchCampaignForView>();
            var premiumBatchDataForView = (from o in _handlingBatchLineRepository.GetAll()
                                           join o1 in _handlingBatchLineStatusRepository.GetAll() on o.HandlingBatchLineStatusId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()
                                           join o2 in _lookup_purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o2.Id into j2
                                           from s2 in j2.DefaultIfEmpty()
                                           join o3 in _lookup_registrationRepository.GetAll() on s2.RegistrationId equals o3.Id into j3
                                           from s3 in j3.DefaultIfEmpty()
                                           join o4 in _lookup_campaignRepository.GetAll() on s3.CampaignId equals o4.Id into j4
                                           from s4 in j4.DefaultIfEmpty()
                                           join o5 in _lookup_countryRepository.GetAll() on s3.CountryId equals o5.Id into j5
                                           from s5 in j5.DefaultIfEmpty()
                                           where o.HandlingBatchId == premiumBatch.Id
                                           orderby s3.CampaignId, s2.RegistrationId
                                           select new
                                           {
                                              CampaignId = s3.CampaignId,
                                              CampaignName = s4.Name,
                                              RegistrationId = s2.RegistrationId,
                                              RegistrationName = !String.IsNullOrWhiteSpace(s3.FirstName) || !String.IsNullOrWhiteSpace(s3.LastName) ? $"{s3.FirstName} {s3.LastName}" : s3.CompanyName,
                                              RegistrationStreet = $"{s3.Street} {s3.HouseNr}",
                                              RegistrationPostal = s3.PostalCode,
                                              RegistrationCity = s3.City,
                                              RegistrationCountry = s5.Description,
                                              RegistrationEmail = s3.EmailAddress,
                                              RegistrationStatus = s1.StatusDescription,
                                              OrderId = o.ExternalOrderId,
                                              CustomerCode = o.CustomerCode,
                                              Quantity = o.Quantity.Value
                                           }).ToList();

            var premiumBatchRegistrationsForView = new List<PremiumBatchRegistrationForView>();
            var premiumBatchRegistrationIds = premiumBatchDataForView.Select(x => x.RegistrationId).Distinct();

            foreach (var premiumBatchRegistrationId in premiumBatchRegistrationIds)
            {
                var _premiumBatchDataForView = premiumBatchDataForView.Where(x => x.RegistrationId == premiumBatchRegistrationId);
                var premiumBatchRegistrationForView = new PremiumBatchRegistrationForView()
                {
                    CampaignName = _premiumBatchDataForView.First().CampaignName,
                    Id = _premiumBatchDataForView.First().RegistrationId,
                    Name = _premiumBatchDataForView.First().RegistrationName,
                    Street = _premiumBatchDataForView.First().RegistrationStreet,
                    Postal = _premiumBatchDataForView.First().RegistrationPostal,
                    City = _premiumBatchDataForView.First().RegistrationCity,
                    Country = _premiumBatchDataForView.First().RegistrationCountry,
                    Email = _premiumBatchDataForView.First().RegistrationEmail,
                    StatusDescription = _premiumBatchDataForView.First().RegistrationStatus,
                    PremiumDescription = String.Join(", ", _premiumBatchDataForView.Select(x => $"{x.Quantity } of {x.CustomerCode }")), 
                    OrderId = _premiumBatchDataForView.First().OrderId
                };

                if (!String.IsNullOrWhiteSpace(premiumBatchRegistrationForView.OrderId))
                {
                    var wmsOrderReference = $"RMS2 {premiumBatchRegistrationForView.Id}";
                    var wmsOrderStatus = _wmsOrderClient.GetOrderStatus(wmsOrderReference, input.WarehouseId, input.OrderUserId, input.Password).Result;

                    if (wmsOrderStatus != null && wmsOrderStatus.MainOrder != null)
                    {
                        switch (wmsOrderStatus.MainOrder.StatusId)
                        {
                            case 205:
                                premiumBatchRegistrationForView.OrderStatus = "Order is shipped";
                                break;
                            case 206:
                                premiumBatchRegistrationForView.OrderStatus = "Order is deleted";
                                break;
                            default:
                                premiumBatchRegistrationForView.OrderStatus = "Order in progress";
                                break;
                        }
                    }
                    else
                    {
                        premiumBatchRegistrationForView.OrderStatus = "Order status unknown";
                    }
                }

                premiumBatchRegistrationsForView.Add(premiumBatchRegistrationForView);
            }

            return _handlingBatchesExcelExporter.ExportToFilePM(premiumBatch.Id, premiumBatchRegistrationsForView);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<GetCashRefundBatchForView> GetCashRefundBatchForView(long id)
        {
            var cashRefundBatch = await _handlingBatchRepository.FirstOrDefaultAsync(id);
            if (cashRefundBatch == null)
            {
                return null;
            }

            var cashRefundBatchStatus = await _handlingBatchStatusAppService.GetById(cashRefundBatch.HandlingBatchStatusId);
            var cashRefundBatchForView = new GetCashRefundBatchForView()
            {
                Id = cashRefundBatch.Id,
                StatusCode = cashRefundBatchStatus.HandlingBatchStatus.StatusCode,
                StatusDescription = cashRefundBatchStatus.HandlingBatchStatus.StatusDescription,
                CreatedOn = cashRefundBatch.DateCreated,
                Remarks = cashRefundBatch.Remarks
            };

            var cashRefundBatchCampaignsForView = new List<CashRefundBatchCampaignForView>();
            var cashRefundBatchDataForView = (from o in _handlingBatchLineRepository.GetAll()
                                              join o1 in _handlingBatchLineStatusRepository.GetAll() on o.HandlingBatchLineStatusId equals o1.Id into j1
                                              from s1 in j1.DefaultIfEmpty()
                                              join o2 in _lookup_purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o2.Id into j2
                                              from s2 in j2.DefaultIfEmpty()
                                              where o.HandlingBatchId == cashRefundBatch.Id
                                              select new
                                              {
                                                 RegistrationId = s2.RegistrationId,
                                                 RefundAmount = o.Amount.Value
                                              }).ToList();

            cashRefundBatchForView.RegistrationsCount = cashRefundBatchDataForView.Select(x => x.RegistrationId).Distinct().Count();
            cashRefundBatchForView.TotalRefundAmount = Math.Round(cashRefundBatchDataForView.Sum(x => x.RefundAmount), 2);

            return cashRefundBatchForView;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<FileDto> GetCashRefundBatchToExcel(long id)
        {
            var cashRefundBatch = await _handlingBatchRepository.FirstOrDefaultAsync(id);
            if (cashRefundBatch == null)
            {
                return null;
            }

            var cashRefundBatchDataForView = (from o in _handlingBatchLineRepository.GetAll()
                                              join o1 in _handlingBatchLineStatusRepository.GetAll() on o.HandlingBatchLineStatusId equals o1.Id into j1
                                              from s1 in j1.DefaultIfEmpty()
                                              join o2 in _lookup_purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o2.Id into j2
                                              from s2 in j2.DefaultIfEmpty()
                                              join o3 in _lookup_registrationRepository.GetAll() on s2.RegistrationId equals o3.Id into j3
                                              from s3 in j3.DefaultIfEmpty()
                                              join o4 in _lookup_campaignRepository.GetAll() on s3.CampaignId equals o4.Id into j4
                                              from s4 in j4.DefaultIfEmpty()
                                              join o5 in _lookup_countryRepository.GetAll() on s3.CountryId equals o5.Id into j5
                                              from s5 in j5.DefaultIfEmpty()
                                              where o.HandlingBatchId == cashRefundBatch.Id
                                              orderby s3.CampaignId, s2.RegistrationId
                                              select new
                                              {
                                                 CampaignId = s3.CampaignId,
                                                 CampaignName = s4.Name,
                                                 RegistrationId = s2.RegistrationId,
                                                 RegistrationName = !String.IsNullOrWhiteSpace(s3.FirstName) || !String.IsNullOrWhiteSpace(s3.LastName) ? $"{s3.FirstName} {s3.LastName}" : s3.CompanyName,
                                                 RegistrationStreet = $"{s3.Street} {s3.HouseNr}",
                                                 RegistrationPostal = s3.PostalCode,
                                                 RegistrationCity = s3.City,
                                                 RegistrationCountry = s5.Description,
                                                 RegistrationEmail = s3.EmailAddress,
                                                 RegistrationStatus = s1.StatusDescription,
                                                 RefundAmount = o.Amount.Value,
                                                 Bic = s3.Bic,
                                                 Iban = s3.Iban
                                              }).ToList();

            var cashRefundBatchRegistrationsForView = new List<CashRefundBatchRegistrationForView>();
            var cashRefundBatchRegistrationIds = cashRefundBatchDataForView.Select(x => x.RegistrationId).Distinct();

            foreach (var cashRefundBatchRegistrationId in cashRefundBatchRegistrationIds)
            {
                var _cashRefundBatchDataForView = cashRefundBatchDataForView.Where(x => x.RegistrationId == cashRefundBatchRegistrationId);
                var cashRefundBatchRegistrationForView = new CashRefundBatchRegistrationForView()
                {
                    CampaignName = _cashRefundBatchDataForView.First().CampaignName,
                    Id = _cashRefundBatchDataForView.First().RegistrationId,
                    Name = _cashRefundBatchDataForView.First().RegistrationName,
                    Street = _cashRefundBatchDataForView.First().RegistrationStreet,
                    Postal = _cashRefundBatchDataForView.First().RegistrationPostal,
                    City = _cashRefundBatchDataForView.First().RegistrationCity,
                    Country = _cashRefundBatchDataForView.First().RegistrationCountry,
                    Email = _cashRefundBatchDataForView.First().RegistrationEmail,
                    StatusDescription = _cashRefundBatchDataForView.First().RegistrationStatus,
                    RefundAmount = Math.Round(_cashRefundBatchDataForView.Sum(x => x.RefundAmount), 2),
                    Bic = _cashRefundBatchDataForView.First().Bic,
                    Iban = _cashRefundBatchDataForView.First().Iban
                };

                cashRefundBatchRegistrationsForView.Add(cashRefundBatchRegistrationForView);
            }

            return _handlingBatchesExcelExporter.ExportToFileCR(cashRefundBatch.Id, cashRefundBatchRegistrationsForView);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<GetActivationCodeBatchForView> GetActivationCodeBatchForView(long id)
        {
            var activationCodeBatch = await _handlingBatchRepository.FirstOrDefaultAsync(id);
            if (activationCodeBatch == null)
            {
                return null;
            }

            var activationCodeBatchStatus = await _handlingBatchStatusAppService.GetById(activationCodeBatch.HandlingBatchStatusId);
            var activationCodeBatchForView = new GetActivationCodeBatchForView()
            {
                Id = activationCodeBatch.Id,
                StatusCode = activationCodeBatchStatus.HandlingBatchStatus.StatusCode,
                StatusDescription = activationCodeBatchStatus.HandlingBatchStatus.StatusDescription,
                CreatedOn = activationCodeBatch.DateCreated,
                Remarks = activationCodeBatch.Remarks
            };

            var activationCodeBatchCampaignsForView = new List<ActivationCodeBatchCampaignForView>();
            var activationCodeBatchDataForView = (from o in _handlingBatchLineRepository.GetAll()
                                                  join o1 in _handlingBatchLineStatusRepository.GetAll() on o.HandlingBatchLineStatusId equals o1.Id into j1
                                                  from s1 in j1.DefaultIfEmpty()
                                                  join o2 in _lookup_purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o2.Id into j2
                                                  from s2 in j2.DefaultIfEmpty()
                                                  join o3 in _lookup_activationCodeRepository.GetAll() on o.ActivationCodeId equals o3.Id into j3
                                                  from s3 in j3.DefaultIfEmpty()
                                                  where o.HandlingBatchId == activationCodeBatch.Id
                                                  select new
                                                  {
                                                     RegistrationId = s2.RegistrationId,
                                                     ActivationCode = s3.Code
                                                  }).ToList();

            activationCodeBatchForView.RegistrationsCount = activationCodeBatchDataForView.Select(x => x.RegistrationId).Distinct().Count();
            activationCodeBatchForView.ActivationCodeCount = activationCodeBatchDataForView.Select(x => x.ActivationCode).Distinct().Count();

            return activationCodeBatchForView;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<FileDto> GetActivationCodeBatchToExcel(long id)
        {
            var activationCodeBatch = await _handlingBatchRepository.FirstOrDefaultAsync(id);
            if (activationCodeBatch == null)
            {
                return null;
            }

            var activationCodeBatchCampaignsForView = new List<ActivationCodeBatchCampaignForView>();
            var activationCodeBatchDataForView = (from o in _handlingBatchLineRepository.GetAll()
                                                  join o1 in _handlingBatchLineStatusRepository.GetAll() on o.HandlingBatchLineStatusId equals o1.Id into j1
                                                  from s1 in j1.DefaultIfEmpty()
                                                  join o2 in _lookup_purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o2.Id into j2
                                                  from s2 in j2.DefaultIfEmpty()
                                                  join o3 in _lookup_registrationRepository.GetAll() on s2.RegistrationId equals o3.Id into j3
                                                  from s3 in j3.DefaultIfEmpty()
                                                  join o4 in _lookup_campaignRepository.GetAll() on s3.CampaignId equals o4.Id into j4
                                                  from s4 in j4.DefaultIfEmpty()
                                                  join o5 in _lookup_countryRepository.GetAll() on s3.CountryId equals o5.Id into j5
                                                  from s5 in j5.DefaultIfEmpty()
                                                  join o6 in _lookup_activationCodeRepository.GetAll() on o.ActivationCodeId equals o6.Id into j6
                                                  from s6 in j6.DefaultIfEmpty()
                                                  where o.HandlingBatchId == activationCodeBatch.Id
                                                  orderby s3.CampaignId, s2.RegistrationId
                                                  select new
                                                  {
                                                     CampaignId = s3.CampaignId,
                                                     CampaignName = s4.Name,
                                                     RegistrationId = s2.RegistrationId,
                                                     RegistrationName = !String.IsNullOrWhiteSpace(s3.FirstName) || !String.IsNullOrWhiteSpace(s3.LastName) ? $"{s3.FirstName} {s3.LastName}" : s3.CompanyName,
                                                     RegistrationStreet = $"{s3.Street} {s3.HouseNr}",
                                                     RegistrationPostal = s3.PostalCode,
                                                     RegistrationCity = s3.City,
                                                     RegistrationCountry = s5.Description,
                                                     RegistrationEmail = s3.EmailAddress,
                                                     RegistrationStatus = s1.StatusDescription,
                                                     ActivationCode = s6.Code
                                                  }).ToList();

            var activationCodeBatchRegistrationsForView = new List<ActivationCodeBatchRegistrationForView>();
            var activationCodeBatchRegistrationIds = activationCodeBatchDataForView.Select(x => x.RegistrationId).Distinct();

            foreach (var activationCodeBatchRegistrationId in activationCodeBatchRegistrationIds)
            {
                var _activationCodeBatchDataForView = activationCodeBatchDataForView.Where(x => x.RegistrationId == activationCodeBatchRegistrationId);
                var activationCodeBatchRegistrationForView = new ActivationCodeBatchRegistrationForView()
                {
                    CampaignName = _activationCodeBatchDataForView.First().CampaignName,
                    Id = _activationCodeBatchDataForView.First().RegistrationId,
                    Name = _activationCodeBatchDataForView.First().RegistrationName,
                    Street = _activationCodeBatchDataForView.First().RegistrationStreet,
                    Postal = _activationCodeBatchDataForView.First().RegistrationPostal,
                    City = _activationCodeBatchDataForView.First().RegistrationCity,
                    Country = _activationCodeBatchDataForView.First().RegistrationCountry,
                    Email = _activationCodeBatchDataForView.First().RegistrationEmail,
                    StatusDescription = _activationCodeBatchDataForView.First().RegistrationStatus,
                    ActivationCode = String.Join(", ", _activationCodeBatchDataForView.Select(x => x.ActivationCode))
                };

                //SendGrid status (or actually Press status... what's in a name?)
                var activationCodeMessageHistory = _lookup_messageHistoryRepository.GetAll().Where(h => h.RegistrationId == activationCodeBatchRegistrationId && h.MessageName.ToLower().Trim() == "activationcode").FirstOrDefault();

                if (activationCodeMessageHistory != null)
                {
                    var activationCodeMessageStatusList = _messagingAppService.getMessageStatusList(new List<long>() { activationCodeMessageHistory.MessageId });
                    var activationCodeMessageStatus = activationCodeMessageStatusList.Any(s => s.MessageId == activationCodeMessageHistory.MessageId) ? activationCodeMessageStatusList.Where(s => s.MessageId == activationCodeMessageHistory.MessageId).First().StatusId : MessageStatusHelper.Unknown;

                    switch (activationCodeMessageStatus)
                    {
                        case MessageStatusHelper.Sent:
                            activationCodeBatchRegistrationForView.MessageStatus = "Mail has been sent";
                            break;
                        case MessageStatusHelper.Awaiting:
                            activationCodeBatchRegistrationForView.MessageStatus = "Mail is waiting to be sent";
                            break;
                        case MessageStatusHelper.Failed:
                            activationCodeBatchRegistrationForView.MessageStatus = "Mail could not be sent (error)";
                            break;
                        default:
                            activationCodeBatchRegistrationForView.MessageStatus = "Mail status is unknown";
                            break;
                    }
                }

                activationCodeBatchRegistrationsForView.Add(activationCodeBatchRegistrationForView);
            }

            return _handlingBatchesExcelExporter.ExportToFileAC(activationCodeBatch.Id, activationCodeBatchRegistrationsForView);
        }

        public async Task<FileDto> GetHandlingBatchesToExcel(GetAllHandlingBatchesForExcelInput input)
        {
            var filteredHandlingBatches = _handlingBatchRepository.GetAll()
                        .Include(e => e.CampaignTypeFk)
                        .Include(e => e.HandlingBatchStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remarks.Contains(input.Filter))
                        .WhereIf(input.MinDateCreatedFilter != null, e => e.DateCreated >= input.MinDateCreatedFilter)
                        .WhereIf(input.MaxDateCreatedFilter != null, e => e.DateCreated <= input.MaxDateCreatedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeNameFilter), e => e.CampaignTypeFk != null && e.CampaignTypeFk.Name == input.CampaignTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HandlingBatchStatusStatusDescriptionFilter), e => e.HandlingBatchStatusFk != null && e.HandlingBatchStatusFk.StatusDescription == input.HandlingBatchStatusStatusDescriptionFilter);

            var query = (from o in filteredHandlingBatches
                         join o1 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_handlingBatchStatusRepository.GetAll() on o.HandlingBatchStatusId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHandlingBatchForViewDto()
                         {
                             HandlingBatch = new HandlingBatchDto
                             {
                                 DateCreated = o.DateCreated,
                                 Remarks = o.Remarks,
                                 Id = o.Id
                             },
                             CampaignTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             HandlingBatchStatusStatusDescription = s2 == null || s2.StatusDescription == null ? "" : s2.StatusDescription.ToString()
                         });

            var handlingBatchListDtos = await query.ToListAsync();

            return _handlingBatchesExcelExporter.ExportToFile(handlingBatchListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<PagedResultDto<HandlingBatchCampaignTypeLookupTableDto>> GetAllCampaignTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_campaignTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var campaignTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HandlingBatchCampaignTypeLookupTableDto>();
            foreach (var campaignType in campaignTypeList)
            {
                lookupTableDtoList.Add(new HandlingBatchCampaignTypeLookupTableDto
                {
                    Id = campaignType.Id,
                    DisplayName = campaignType.Name?.ToString()
                });
            }

            return new PagedResultDto<HandlingBatchCampaignTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatches, AppPermissions.Pages_HandlingBatches_ViewMenu)]
        public async Task<PagedResultDto<HandlingBatchHandlingBatchStatusLookupTableDto>> GetAllHandlingBatchStatusForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_handlingBatchStatusRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.StatusDescription != null && e.StatusDescription.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var handlingBatchStatusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HandlingBatchHandlingBatchStatusLookupTableDto>();
            foreach (var handlingBatchStatus in handlingBatchStatusList)
            {
                lookupTableDtoList.Add(new HandlingBatchHandlingBatchStatusLookupTableDto
                {
                    Id = handlingBatchStatus.Id,
                    DisplayName = handlingBatchStatus.StatusDescription?.ToString()
                });
            }

            return new PagedResultDto<HandlingBatchHandlingBatchStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}