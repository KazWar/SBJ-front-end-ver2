using RMS.SBJ.HandlingBatch;
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
    [AbpAuthorize(AppPermissions.Pages_HandlingBatchHistories)]
    public class HandlingBatchHistoriesAppService : RMSAppServiceBase, IHandlingBatchHistoriesAppService
    {
        private readonly IRepository<HandlingBatchHistory, long> _handlingBatchHistoryRepository;
        private readonly IRepository<HandlingBatch, long> _lookup_handlingBatchRepository;
        private readonly IRepository<HandlingBatchStatus, long> _lookup_handlingBatchStatusRepository;

        public HandlingBatchHistoriesAppService(IRepository<HandlingBatchHistory, long> handlingBatchHistoryRepository, IRepository<HandlingBatch, long> lookup_handlingBatchRepository, IRepository<HandlingBatchStatus, long> lookup_handlingBatchStatusRepository)
        {
            _handlingBatchHistoryRepository = handlingBatchHistoryRepository;
            _lookup_handlingBatchRepository = lookup_handlingBatchRepository;
            _lookup_handlingBatchStatusRepository = lookup_handlingBatchStatusRepository;

        }

        public async Task<PagedResultDto<GetHandlingBatchHistoryForViewDto>> GetAll(GetAllHandlingBatchHistoriesInput input)
        {

            var filteredHandlingBatchHistories = _handlingBatchHistoryRepository.GetAll()
                        .Include(e => e.HandlingBatchFk)
                        .Include(e => e.HandlingBatchStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remarks.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HandlingBatchRemarksFilter), e => e.HandlingBatchFk != null && e.HandlingBatchFk.Remarks == input.HandlingBatchRemarksFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HandlingBatchStatusStatusDescriptionFilter), e => e.HandlingBatchStatusFk != null && e.HandlingBatchStatusFk.StatusDescription == input.HandlingBatchStatusStatusDescriptionFilter);

            var pagedAndFilteredHandlingBatchHistories = filteredHandlingBatchHistories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var handlingBatchHistories = from o in pagedAndFilteredHandlingBatchHistories
                                         join o1 in _lookup_handlingBatchRepository.GetAll() on o.HandlingBatchId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _lookup_handlingBatchStatusRepository.GetAll() on o.HandlingBatchStatusId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         select new GetHandlingBatchHistoryForViewDto()
                                         {
                                             HandlingBatchHistory = new HandlingBatchHistoryDto
                                             {
                                                 Id = o.Id
                                             },
                                             HandlingBatchRemarks = s1 == null || s1.Remarks == null ? "" : s1.Remarks.ToString(),
                                             HandlingBatchStatusStatusDescription = s2 == null || s2.StatusDescription == null ? "" : s2.StatusDescription.ToString()
                                         };

            var totalCount = await filteredHandlingBatchHistories.CountAsync();

            return new PagedResultDto<GetHandlingBatchHistoryForViewDto>(
                totalCount,
                await handlingBatchHistories.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchHistories_Edit)]
        public async Task<GetHandlingBatchHistoryForEditOutput> GetHandlingBatchHistoryForEdit(EntityDto<long> input)
        {
            var handlingBatchHistory = await _handlingBatchHistoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHandlingBatchHistoryForEditOutput { HandlingBatchHistory = ObjectMapper.Map<CreateOrEditHandlingBatchHistoryDto>(handlingBatchHistory) };

            if (output.HandlingBatchHistory.HandlingBatchId != null)
            {
                var _lookupHandlingBatch = await _lookup_handlingBatchRepository.FirstOrDefaultAsync((long)output.HandlingBatchHistory.HandlingBatchId);
                output.HandlingBatchRemarks = _lookupHandlingBatch?.Remarks?.ToString();
            }

            if (output.HandlingBatchHistory.HandlingBatchStatusId != null)
            {
                var _lookupHandlingBatchStatus = await _lookup_handlingBatchStatusRepository.FirstOrDefaultAsync((long)output.HandlingBatchHistory.HandlingBatchStatusId);
                output.HandlingBatchStatusStatusDescription = _lookupHandlingBatchStatus?.StatusDescription?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHandlingBatchHistoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchHistories_Create)]
        protected virtual async Task Create(CreateOrEditHandlingBatchHistoryDto input)
        {
            var handlingBatchHistory = ObjectMapper.Map<HandlingBatchHistory>(input);

            if (AbpSession.TenantId != null)
            {
                handlingBatchHistory.TenantId = (int?)AbpSession.TenantId;
            }

            await _handlingBatchHistoryRepository.InsertAsync(handlingBatchHistory);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchHistories_Edit)]
        protected virtual async Task Update(CreateOrEditHandlingBatchHistoryDto input)
        {
            var handlingBatchHistory = await _handlingBatchHistoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, handlingBatchHistory);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchHistories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _handlingBatchHistoryRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchHistories)]
        public async Task<PagedResultDto<HandlingBatchHistoryHandlingBatchLookupTableDto>> GetAllHandlingBatchForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_handlingBatchRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Remarks != null && e.Remarks.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var handlingBatchList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HandlingBatchHistoryHandlingBatchLookupTableDto>();
            foreach (var handlingBatch in handlingBatchList)
            {
                lookupTableDtoList.Add(new HandlingBatchHistoryHandlingBatchLookupTableDto
                {
                    Id = handlingBatch.Id,
                    DisplayName = handlingBatch.Remarks?.ToString()
                });
            }

            return new PagedResultDto<HandlingBatchHistoryHandlingBatchLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchHistories)]
        public async Task<PagedResultDto<HandlingBatchHistoryHandlingBatchStatusLookupTableDto>> GetAllHandlingBatchStatusForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_handlingBatchStatusRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.StatusDescription != null && e.StatusDescription.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var handlingBatchStatusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HandlingBatchHistoryHandlingBatchStatusLookupTableDto>();
            foreach (var handlingBatchStatus in handlingBatchStatusList)
            {
                lookupTableDtoList.Add(new HandlingBatchHistoryHandlingBatchStatusLookupTableDto
                {
                    Id = handlingBatchStatus.Id,
                    DisplayName = handlingBatchStatus.StatusDescription?.ToString()
                });
            }

            return new PagedResultDto<HandlingBatchHistoryHandlingBatchStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}