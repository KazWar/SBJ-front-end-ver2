using RMS.SBJ.HandlingBatch;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.HandlingBatch;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.HandlingBatch.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.HandlingBatch
{
    [AbpAuthorize(AppPermissions.Pages_HandlingBatchLines)]
    public class HandlingBatchLinesAppService : RMSAppServiceBase, IHandlingBatchLinesAppService
    {
        private readonly IRepository<HandlingBatchLine, long> _handlingBatchLineRepository;
        private readonly IRepository<HandlingBatch, long> _lookup_handlingBatchRepository;
        private readonly IRepository<PurchaseRegistration, long> _lookup_purchaseRegistrationRepository;
        private readonly IRepository<HandlingBatchLineStatus, long> _lookup_handlingBatchLineStatusRepository;

        public HandlingBatchLinesAppService(IRepository<HandlingBatchLine, long> handlingBatchLineRepository, IRepository<HandlingBatch, long> lookup_handlingBatchRepository, IRepository<PurchaseRegistration, long> lookup_purchaseRegistrationRepository, IRepository<HandlingBatchLineStatus, long> lookup_handlingBatchLineStatusRepository)
        {
            _handlingBatchLineRepository = handlingBatchLineRepository;
            _lookup_handlingBatchRepository = lookup_handlingBatchRepository;
            _lookup_purchaseRegistrationRepository = lookup_purchaseRegistrationRepository;
            _lookup_handlingBatchLineStatusRepository = lookup_handlingBatchLineStatusRepository;

        }

        public async Task<PagedResultDto<GetHandlingBatchLineForViewDto>> GetAll(GetAllHandlingBatchLinesInput input)
        {

            var filteredHandlingBatchLines = _handlingBatchLineRepository.GetAll()
                        .Include(e => e.HandlingBatchFk)
                        .Include(e => e.PurchaseRegistrationFk)
                        .Include(e => e.HandlingBatchLineStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerCode.Contains(input.Filter) || e.ExternalOrderId.Contains(input.Filter))
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HandlingBatchRemarksFilter), e => e.HandlingBatchFk != null && e.HandlingBatchFk.Remarks == input.HandlingBatchRemarksFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PurchaseRegistrationInvoiceImagePathFilter), e => e.PurchaseRegistrationFk != null && e.PurchaseRegistrationFk.InvoiceImagePath == input.PurchaseRegistrationInvoiceImagePathFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HandlingBatchLineStatusStatusDescriptionFilter), e => e.HandlingBatchLineStatusFk != null && e.HandlingBatchLineStatusFk.StatusDescription == input.HandlingBatchLineStatusStatusDescriptionFilter);

            var pagedAndFilteredHandlingBatchLines = filteredHandlingBatchLines
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var handlingBatchLines = from o in pagedAndFilteredHandlingBatchLines
                                     join o1 in _lookup_handlingBatchRepository.GetAll() on o.HandlingBatchId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     join o2 in _lookup_purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()

                                     join o3 in _lookup_handlingBatchLineStatusRepository.GetAll() on o.HandlingBatchLineStatusId equals o3.Id into j3
                                     from s3 in j3.DefaultIfEmpty()

                                     select new GetHandlingBatchLineForViewDto()
                                     {
                                         HandlingBatchLine = new HandlingBatchLineDto
                                         {
                                             Quantity = o.Quantity,
                                             Id = o.Id
                                         },
                                         HandlingBatchRemarks = s1 == null || s1.Remarks == null ? "" : s1.Remarks.ToString(),
                                         PurchaseRegistrationInvoiceImagePath = s2 == null || s2.InvoiceImagePath == null ? "" : s2.InvoiceImagePath.ToString(),
                                         HandlingBatchLineStatusStatusDescription = s3 == null || s3.StatusDescription == null ? "" : s3.StatusDescription.ToString()
                                     };

            var totalCount = await filteredHandlingBatchLines.CountAsync();

            return new PagedResultDto<GetHandlingBatchLineForViewDto>(
                totalCount,
                await handlingBatchLines.ToListAsync()
            );
        }

        public async Task<GetHandlingBatchLineForViewDto> GetHandlingBatchLineForView(long id)
        {
            var handlingBatchLine = await _handlingBatchLineRepository.GetAsync(id);

            var output = new GetHandlingBatchLineForViewDto { HandlingBatchLine = ObjectMapper.Map<HandlingBatchLineDto>(handlingBatchLine) };

            if (output.HandlingBatchLine.HandlingBatchId != null)
            {
                var _lookupHandlingBatch = await _lookup_handlingBatchRepository.FirstOrDefaultAsync((long)output.HandlingBatchLine.HandlingBatchId);
                output.HandlingBatchRemarks = _lookupHandlingBatch?.Remarks?.ToString();
            }

            if (output.HandlingBatchLine.PurchaseRegistrationId != null)
            {
                var _lookupPurchaseRegistration = await _lookup_purchaseRegistrationRepository.FirstOrDefaultAsync((long)output.HandlingBatchLine.PurchaseRegistrationId);
                output.PurchaseRegistrationInvoiceImagePath = _lookupPurchaseRegistration?.InvoiceImagePath?.ToString();
            }

            if (output.HandlingBatchLine.HandlingBatchLineStatusId != null)
            {
                var _lookupHandlingBatchLineStatus = await _lookup_handlingBatchLineStatusRepository.FirstOrDefaultAsync((long)output.HandlingBatchLine.HandlingBatchLineStatusId);
                output.HandlingBatchLineStatusStatusDescription = _lookupHandlingBatchLineStatus?.StatusDescription?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLines_Edit)]
        public async Task<GetHandlingBatchLineForEditOutput> GetHandlingBatchLineForEdit(EntityDto<long> input)
        {
            var handlingBatchLine = await _handlingBatchLineRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHandlingBatchLineForEditOutput { HandlingBatchLine = ObjectMapper.Map<CreateOrEditHandlingBatchLineDto>(handlingBatchLine) };

            if (output.HandlingBatchLine.HandlingBatchId != null)
            {
                var _lookupHandlingBatch = await _lookup_handlingBatchRepository.FirstOrDefaultAsync((long)output.HandlingBatchLine.HandlingBatchId);
                output.HandlingBatchRemarks = _lookupHandlingBatch?.Remarks?.ToString();
            }

            if (output.HandlingBatchLine.PurchaseRegistrationId != null)
            {
                var _lookupPurchaseRegistration = await _lookup_purchaseRegistrationRepository.FirstOrDefaultAsync((long)output.HandlingBatchLine.PurchaseRegistrationId);
                output.PurchaseRegistrationInvoiceImagePath = _lookupPurchaseRegistration?.InvoiceImagePath?.ToString();
            }

            if (output.HandlingBatchLine.HandlingBatchLineStatusId != null)
            {
                var _lookupHandlingBatchLineStatus = await _lookup_handlingBatchLineStatusRepository.FirstOrDefaultAsync((long)output.HandlingBatchLine.HandlingBatchLineStatusId);
                output.HandlingBatchLineStatusStatusDescription = _lookupHandlingBatchLineStatus?.StatusDescription?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHandlingBatchLineDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLines_Create)]
        protected virtual async Task Create(CreateOrEditHandlingBatchLineDto input)
        {
            var handlingBatchLine = ObjectMapper.Map<HandlingBatchLine>(input);

            if (AbpSession.TenantId != null)
            {
                handlingBatchLine.TenantId = (int?)AbpSession.TenantId;
            }

            await _handlingBatchLineRepository.InsertAsync(handlingBatchLine);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLines_Edit)]
        protected virtual async Task Update(CreateOrEditHandlingBatchLineDto input)
        {
            var handlingBatchLine = await _handlingBatchLineRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, handlingBatchLine);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLines_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _handlingBatchLineRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLines)]
        public async Task<PagedResultDto<HandlingBatchLineHandlingBatchLookupTableDto>> GetAllHandlingBatchForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_handlingBatchRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Remarks != null && e.Remarks.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var handlingBatchList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HandlingBatchLineHandlingBatchLookupTableDto>();
            foreach (var handlingBatch in handlingBatchList)
            {
                lookupTableDtoList.Add(new HandlingBatchLineHandlingBatchLookupTableDto
                {
                    Id = handlingBatch.Id,
                    DisplayName = handlingBatch.Remarks?.ToString()
                });
            }

            return new PagedResultDto<HandlingBatchLineHandlingBatchLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLines)]
        public async Task<PagedResultDto<HandlingBatchLinePurchaseRegistrationLookupTableDto>> GetAllPurchaseRegistrationForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_purchaseRegistrationRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.InvoiceImagePath != null && e.InvoiceImagePath.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var purchaseRegistrationList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HandlingBatchLinePurchaseRegistrationLookupTableDto>();
            foreach (var purchaseRegistration in purchaseRegistrationList)
            {
                lookupTableDtoList.Add(new HandlingBatchLinePurchaseRegistrationLookupTableDto
                {
                    Id = purchaseRegistration.Id,
                    DisplayName = purchaseRegistration.InvoiceImagePath?.ToString()
                });
            }

            return new PagedResultDto<HandlingBatchLinePurchaseRegistrationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLines)]
        public async Task<PagedResultDto<HandlingBatchLineHandlingBatchLineStatusLookupTableDto>> GetAllHandlingBatchLineStatusForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_handlingBatchLineStatusRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.StatusDescription != null && e.StatusDescription.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var handlingBatchLineStatusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HandlingBatchLineHandlingBatchLineStatusLookupTableDto>();
            foreach (var handlingBatchLineStatus in handlingBatchLineStatusList)
            {
                lookupTableDtoList.Add(new HandlingBatchLineHandlingBatchLineStatusLookupTableDto
                {
                    Id = handlingBatchLineStatus.Id,
                    DisplayName = handlingBatchLineStatus.StatusDescription?.ToString()
                });
            }

            return new PagedResultDto<HandlingBatchLineHandlingBatchLineStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}