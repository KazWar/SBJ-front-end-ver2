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
    [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineHistories)]
    public class HandlingBatchLineHistoriesAppService : RMSAppServiceBase, IHandlingBatchLineHistoriesAppService
    {
        private readonly IRepository<HandlingBatchLineHistory, long> _handlingBatchLineHistoryRepository;
        private readonly IRepository<HandlingBatchLine, long> _lookup_handlingBatchLineRepository;
        private readonly IRepository<HandlingBatchLineStatus, long> _lookup_handlingBatchLineStatusRepository;

        public HandlingBatchLineHistoriesAppService(IRepository<HandlingBatchLineHistory, long> handlingBatchLineHistoryRepository, IRepository<HandlingBatchLine, long> lookup_handlingBatchLineRepository, IRepository<HandlingBatchLineStatus, long> lookup_handlingBatchLineStatusRepository)
        {
            _handlingBatchLineHistoryRepository = handlingBatchLineHistoryRepository;
            _lookup_handlingBatchLineRepository = lookup_handlingBatchLineRepository;
            _lookup_handlingBatchLineStatusRepository = lookup_handlingBatchLineStatusRepository;

        }

        public async Task<PagedResultDto<GetHandlingBatchLineHistoryForViewDto>> GetAll(GetAllHandlingBatchLineHistoriesInput input)
        {

            var filteredHandlingBatchLineHistories = _handlingBatchLineHistoryRepository.GetAll()
                        .Include(e => e.HandlingBatchLineFk)
                        .Include(e => e.HandlingBatchLineStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remarks.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HandlingBatchLineCustomerCodeFilter), e => e.HandlingBatchLineFk != null && e.HandlingBatchLineFk.CustomerCode == input.HandlingBatchLineCustomerCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HandlingBatchLineStatusStatusDescriptionFilter), e => e.HandlingBatchLineStatusFk != null && e.HandlingBatchLineStatusFk.StatusDescription == input.HandlingBatchLineStatusStatusDescriptionFilter);

            var pagedAndFilteredHandlingBatchLineHistories = filteredHandlingBatchLineHistories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var handlingBatchLineHistories = from o in pagedAndFilteredHandlingBatchLineHistories
                                             join o1 in _lookup_handlingBatchLineRepository.GetAll() on o.HandlingBatchLineId equals o1.Id into j1
                                             from s1 in j1.DefaultIfEmpty()

                                             join o2 in _lookup_handlingBatchLineStatusRepository.GetAll() on o.HandlingBatchLineStatusId equals o2.Id into j2
                                             from s2 in j2.DefaultIfEmpty()

                                             select new GetHandlingBatchLineHistoryForViewDto()
                                             {
                                                 HandlingBatchLineHistory = new HandlingBatchLineHistoryDto
                                                 {
                                                     Id = o.Id
                                                 },
                                                 HandlingBatchLineCustomerCode = s1 == null || s1.CustomerCode == null ? "" : s1.CustomerCode.ToString(),
                                                 HandlingBatchLineStatusStatusDescription = s2 == null || s2.StatusDescription == null ? "" : s2.StatusDescription.ToString()
                                             };

            var totalCount = await filteredHandlingBatchLineHistories.CountAsync();

            return new PagedResultDto<GetHandlingBatchLineHistoryForViewDto>(
                totalCount,
                await handlingBatchLineHistories.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineHistories_Edit)]
        public async Task<GetHandlingBatchLineHistoryForEditOutput> GetHandlingBatchLineHistoryForEdit(EntityDto<long> input)
        {
            var handlingBatchLineHistory = await _handlingBatchLineHistoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHandlingBatchLineHistoryForEditOutput { HandlingBatchLineHistory = ObjectMapper.Map<CreateOrEditHandlingBatchLineHistoryDto>(handlingBatchLineHistory) };

            if (output.HandlingBatchLineHistory.HandlingBatchLineId != null)
            {
                var _lookupHandlingBatchLine = await _lookup_handlingBatchLineRepository.FirstOrDefaultAsync((long)output.HandlingBatchLineHistory.HandlingBatchLineId);
                output.HandlingBatchLineCustomerCode = _lookupHandlingBatchLine?.CustomerCode?.ToString();
            }

            if (output.HandlingBatchLineHistory.HandlingBatchLineStatusId != null)
            {
                var _lookupHandlingBatchLineStatus = await _lookup_handlingBatchLineStatusRepository.FirstOrDefaultAsync((long)output.HandlingBatchLineHistory.HandlingBatchLineStatusId);
                output.HandlingBatchLineStatusStatusDescription = _lookupHandlingBatchLineStatus?.StatusDescription?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHandlingBatchLineHistoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineHistories_Create)]
        protected virtual async Task Create(CreateOrEditHandlingBatchLineHistoryDto input)
        {
            var handlingBatchLineHistory = ObjectMapper.Map<HandlingBatchLineHistory>(input);

            if (AbpSession.TenantId != null)
            {
                handlingBatchLineHistory.TenantId = (int?)AbpSession.TenantId;
            }

            await _handlingBatchLineHistoryRepository.InsertAsync(handlingBatchLineHistory);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineHistories_Edit)]
        protected virtual async Task Update(CreateOrEditHandlingBatchLineHistoryDto input)
        {
            var handlingBatchLineHistory = await _handlingBatchLineHistoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, handlingBatchLineHistory);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineHistories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _handlingBatchLineHistoryRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineHistories)]
        public async Task<PagedResultDto<HandlingBatchLineHistoryHandlingBatchLineLookupTableDto>> GetAllHandlingBatchLineForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_handlingBatchLineRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CustomerCode != null && e.CustomerCode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var handlingBatchLineList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HandlingBatchLineHistoryHandlingBatchLineLookupTableDto>();
            foreach (var handlingBatchLine in handlingBatchLineList)
            {
                lookupTableDtoList.Add(new HandlingBatchLineHistoryHandlingBatchLineLookupTableDto
                {
                    Id = handlingBatchLine.Id,
                    DisplayName = handlingBatchLine.CustomerCode?.ToString()
                });
            }

            return new PagedResultDto<HandlingBatchLineHistoryHandlingBatchLineLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineHistories)]
        public async Task<PagedResultDto<HandlingBatchLineHistoryHandlingBatchLineStatusLookupTableDto>> GetAllHandlingBatchLineStatusForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_handlingBatchLineStatusRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.StatusDescription != null && e.StatusDescription.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var handlingBatchLineStatusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HandlingBatchLineHistoryHandlingBatchLineStatusLookupTableDto>();
            foreach (var handlingBatchLineStatus in handlingBatchLineStatusList)
            {
                lookupTableDtoList.Add(new HandlingBatchLineHistoryHandlingBatchLineStatusLookupTableDto
                {
                    Id = handlingBatchLineStatus.Id,
                    DisplayName = handlingBatchLineStatus.StatusDescription?.ToString()
                });
            }

            return new PagedResultDto<HandlingBatchLineHistoryHandlingBatchLineStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}