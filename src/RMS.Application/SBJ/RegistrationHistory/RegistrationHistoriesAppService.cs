using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.Registrations;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.RegistrationHistory.Exporting;
using RMS.SBJ.RegistrationHistory.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.RegistrationHistory
{
    [AbpAuthorize(AppPermissions.Pages_RegistrationHistories)]
    public class RegistrationHistoriesAppService : RMSAppServiceBase, IRegistrationHistoriesAppService
    {
        private readonly IRepository<RegistrationHistory, long> _registrationHistoryRepository;
        private readonly IRegistrationHistoriesExcelExporter _registrationHistoriesExcelExporter;
        private readonly IRepository<RegistrationStatus, long> _lookup_registrationStatusRepository;
        private readonly IRepository<Registration, long> _lookup_registrationRepository;

        public RegistrationHistoriesAppService(IRepository<RegistrationHistory, long> registrationHistoryRepository, IRegistrationHistoriesExcelExporter registrationHistoriesExcelExporter, IRepository<RegistrationStatus, long> lookup_registrationStatusRepository, IRepository<Registration, long> lookup_registrationRepository)
        {
            _registrationHistoryRepository = registrationHistoryRepository;
            _registrationHistoriesExcelExporter = registrationHistoriesExcelExporter;
            _lookup_registrationStatusRepository = lookup_registrationStatusRepository;
            _lookup_registrationRepository = lookup_registrationRepository;

        }

        public async Task<PagedResultDto<GetRegistrationHistoryForViewDto>> GetAll(GetAllRegistrationHistoriesInput input)
        {

            var filteredRegistrationHistories = _registrationHistoryRepository.GetAll()
                        .Include(e => e.RegistrationStatusFk)
                        .Include(e => e.RegistrationFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remarks.Contains(input.Filter))
                        .WhereIf(input.MinDateCreatedFilter != null, e => e.DateCreated >= input.MinDateCreatedFilter)
                        .WhereIf(input.MaxDateCreatedFilter != null, e => e.DateCreated <= input.MaxDateCreatedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter)
                        .WhereIf(input.MinAbpUserIdFilter != null, e => e.AbpUserId >= input.MinAbpUserIdFilter)
                        .WhereIf(input.MaxAbpUserIdFilter != null, e => e.AbpUserId <= input.MaxAbpUserIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationStatusStatusCodeFilter), e => e.RegistrationStatusFk != null && e.RegistrationStatusFk.StatusCode == input.RegistrationStatusStatusCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationFirstNameFilter), e => e.RegistrationFk != null && e.RegistrationFk.FirstName == input.RegistrationFirstNameFilter);

            var pagedAndFilteredRegistrationHistories = filteredRegistrationHistories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var registrationHistories = from o in pagedAndFilteredRegistrationHistories
                                        join o1 in _lookup_registrationStatusRepository.GetAll() on o.RegistrationStatusId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()

                                        join o2 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o2.Id into j2
                                        from s2 in j2.DefaultIfEmpty()

                                        select new GetRegistrationHistoryForViewDto()
                                        {
                                            RegistrationHistory = new RegistrationHistoryDto
                                            {
                                                DateCreated = o.DateCreated,
                                                Remarks = o.Remarks,
                                                AbpUserId = o.AbpUserId,
                                                Id = o.Id
                                            },
                                            RegistrationStatusStatusCode = s1 == null || s1.StatusCode == null ? "" : s1.StatusCode.ToString(),
                                            RegistrationFirstName = s2 == null || s2.FirstName == null ? "" : s2.FirstName.ToString()
                                        };

            var totalCount = await filteredRegistrationHistories.CountAsync();

            return new PagedResultDto<GetRegistrationHistoryForViewDto>(
                totalCount,
                await registrationHistories.ToListAsync()
            );
        }

        public async Task<GetRegistrationHistoryForViewDto> GetRegistrationHistoryForView(long id)
        {
            var registrationHistory = await _registrationHistoryRepository.GetAsync(id);

            var output = new GetRegistrationHistoryForViewDto { RegistrationHistory = ObjectMapper.Map<RegistrationHistoryDto>(registrationHistory) };

            if (output.RegistrationHistory.RegistrationStatusId != null)
            {
                var _lookupRegistrationStatus = await _lookup_registrationStatusRepository.FirstOrDefaultAsync((long)output.RegistrationHistory.RegistrationStatusId);
                output.RegistrationStatusStatusCode = _lookupRegistrationStatus?.StatusCode?.ToString();
            }

            if (output.RegistrationHistory.RegistrationId != null)
            {
                var _lookupRegistration = await _lookup_registrationRepository.FirstOrDefaultAsync((long)output.RegistrationHistory.RegistrationId);
                output.RegistrationFirstName = _lookupRegistration?.FirstName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationHistories_Edit)]
        public async Task<GetRegistrationHistoryForEditOutput> GetRegistrationHistoryForEdit(EntityDto<long> input)
        {
            var registrationHistory = await _registrationHistoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRegistrationHistoryForEditOutput { RegistrationHistory = ObjectMapper.Map<CreateOrEditRegistrationHistoryDto>(registrationHistory) };

            if (output.RegistrationHistory.RegistrationStatusId != null)
            {
                var _lookupRegistrationStatus = await _lookup_registrationStatusRepository.FirstOrDefaultAsync((long)output.RegistrationHistory.RegistrationStatusId);
                output.RegistrationStatusStatusCode = _lookupRegistrationStatus?.StatusCode?.ToString();
            }

            if (output.RegistrationHistory.RegistrationId != null)
            {
                var _lookupRegistration = await _lookup_registrationRepository.FirstOrDefaultAsync((long)output.RegistrationHistory.RegistrationId);
                output.RegistrationFirstName = _lookupRegistration?.FirstName?.ToString();
            }

            return output;
        }

        public async Task CreateNew(CreateOrEditRegistrationHistoryDto input)
        {
            await Create(input);
        }

        public async Task CreateOrEdit(CreateOrEditRegistrationHistoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_RegistrationHistories_Create)]
        protected virtual async Task Create(CreateOrEditRegistrationHistoryDto input)
        {
            var registrationHistory = ObjectMapper.Map<RegistrationHistory>(input);

            if (AbpSession.TenantId != null)
            {
                registrationHistory.TenantId = (int?)AbpSession.TenantId;
            }

            await _registrationHistoryRepository.InsertAsync(registrationHistory);
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationHistories_Edit)]
        protected virtual async Task Update(CreateOrEditRegistrationHistoryDto input)
        {
            var registrationHistory = await _registrationHistoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, registrationHistory);
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationHistories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _registrationHistoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRegistrationHistoriesToExcel(GetAllRegistrationHistoriesForExcelInput input)
        {

            var filteredRegistrationHistories = _registrationHistoryRepository.GetAll()
                        .Include(e => e.RegistrationStatusFk)
                        .Include(e => e.RegistrationFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remarks.Contains(input.Filter))
                        .WhereIf(input.MinDateCreatedFilter != null, e => e.DateCreated >= input.MinDateCreatedFilter)
                        .WhereIf(input.MaxDateCreatedFilter != null, e => e.DateCreated <= input.MaxDateCreatedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter)
                        .WhereIf(input.MinAbpUserIdFilter != null, e => e.AbpUserId >= input.MinAbpUserIdFilter)
                        .WhereIf(input.MaxAbpUserIdFilter != null, e => e.AbpUserId <= input.MaxAbpUserIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationStatusStatusCodeFilter), e => e.RegistrationStatusFk != null && e.RegistrationStatusFk.StatusCode == input.RegistrationStatusStatusCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationFirstNameFilter), e => e.RegistrationFk != null && e.RegistrationFk.FirstName == input.RegistrationFirstNameFilter);

            var query = (from o in filteredRegistrationHistories
                         join o1 in _lookup_registrationStatusRepository.GetAll() on o.RegistrationStatusId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetRegistrationHistoryForViewDto()
                         {
                             RegistrationHistory = new RegistrationHistoryDto
                             {
                                 DateCreated = o.DateCreated,
                                 Remarks = o.Remarks,
                                 AbpUserId = o.AbpUserId,
                                 Id = o.Id
                             },
                             RegistrationStatusStatusCode = s1 == null || s1.StatusCode == null ? "" : s1.StatusCode.ToString(),
                             RegistrationFirstName = s2 == null || s2.FirstName == null ? "" : s2.FirstName.ToString()
                         });

            var registrationHistoryListDtos = await query.ToListAsync();

            return _registrationHistoriesExcelExporter.ExportToFile(registrationHistoryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationHistories)]
        public async Task<PagedResultDto<RegistrationHistoryRegistrationStatusLookupTableDto>> GetAllRegistrationStatusForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_registrationStatusRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.StatusCode != null && e.StatusCode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var registrationStatusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RegistrationHistoryRegistrationStatusLookupTableDto>();
            foreach (var registrationStatus in registrationStatusList)
            {
                lookupTableDtoList.Add(new RegistrationHistoryRegistrationStatusLookupTableDto
                {
                    Id = registrationStatus.Id,
                    DisplayName = registrationStatus.StatusCode?.ToString()
                });
            }

            return new PagedResultDto<RegistrationHistoryRegistrationStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationHistories)]
        public async Task<PagedResultDto<RegistrationHistoryRegistrationLookupTableDto>> GetAllRegistrationForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_registrationRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FirstName != null && e.FirstName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var registrationList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RegistrationHistoryRegistrationLookupTableDto>();
            foreach (var registration in registrationList)
            {
                lookupTableDtoList.Add(new RegistrationHistoryRegistrationLookupTableDto
                {
                    Id = registration.Id,
                    DisplayName = registration.FirstName?.ToString()
                });
            }

            return new PagedResultDto<RegistrationHistoryRegistrationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}