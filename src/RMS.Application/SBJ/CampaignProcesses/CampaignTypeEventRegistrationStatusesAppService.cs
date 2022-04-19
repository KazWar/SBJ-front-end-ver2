using RMS.SBJ.CodeTypeTables;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CampaignProcesses.Exporting;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.CampaignProcesses
{
    [AbpAuthorize(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses)]
    public class CampaignTypeEventRegistrationStatusesAppService : RMSAppServiceBase, ICampaignTypeEventRegistrationStatusesAppService
    {
        private readonly IRepository<CampaignTypeEventRegistrationStatus, long> _campaignTypeEventRegistrationStatusRepository;
        private readonly ICampaignTypeEventRegistrationStatusesExcelExporter _campaignTypeEventRegistrationStatusesExcelExporter;
        private readonly IRepository<CampaignTypeEvent, long> _lookup_campaignTypeEventRepository;
        private readonly IRepository<RegistrationStatus, long> _lookup_registrationStatusRepository;


        public CampaignTypeEventRegistrationStatusesAppService(IRepository<CampaignTypeEventRegistrationStatus, long> campaignTypeEventRegistrationStatusRepository, ICampaignTypeEventRegistrationStatusesExcelExporter campaignTypeEventRegistrationStatusesExcelExporter, IRepository<CampaignTypeEvent, long> lookup_campaignTypeEventRepository, IRepository<RegistrationStatus, long> lookup_registrationStatusRepository)
        {
            _campaignTypeEventRegistrationStatusRepository = campaignTypeEventRegistrationStatusRepository;
            _campaignTypeEventRegistrationStatusesExcelExporter = campaignTypeEventRegistrationStatusesExcelExporter;
            _lookup_campaignTypeEventRepository = lookup_campaignTypeEventRepository;
            _lookup_registrationStatusRepository = lookup_registrationStatusRepository;

        }

        public async Task<PagedResultDto<GetCampaignTypeEventRegistrationStatusForViewDto>> GetAll(GetAllCampaignTypeEventRegistrationStatusesInput input)
        {
            var filteredCampaignTypeEventRegistrationStatuses = _campaignTypeEventRegistrationStatusRepository.GetAll()
                        .Include(e => e.CampaignTypeEventFk)
                        .Include(e => e.RegistrationStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
                        .WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeEventSortOrderFilter), e => e.CampaignTypeEventFk != null && e.CampaignTypeEventFk.SortOrder.ToString().ToLower() == input.CampaignTypeEventSortOrderFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationStatusDescriptionFilter), e => e.RegistrationStatusFk != null && e.RegistrationStatusFk.Description.ToLower() == input.RegistrationStatusDescriptionFilter.ToLower().Trim());

            var pagedAndFilteredCampaignTypeEventRegistrationStatuses = filteredCampaignTypeEventRegistrationStatuses.OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var campaignTypeEventRegistrationStatuses = from o in pagedAndFilteredCampaignTypeEventRegistrationStatuses
                                                        join o1 in _lookup_campaignTypeEventRepository.GetAll() on o.CampaignTypeEventId equals o1.Id into j1
                                                        from s1 in j1.DefaultIfEmpty()

                                                        join o2 in _lookup_registrationStatusRepository.GetAll() on o.RegistrationStatusId equals o2.Id into j2
                                                        from s2 in j2.DefaultIfEmpty()

                                                        select new GetCampaignTypeEventRegistrationStatusForViewDto()
                                                        {
                                                            CampaignTypeEventRegistrationStatus = new CampaignTypeEventRegistrationStatusDto
                                                            {
                                                                Id = o.Id,
                                                                CampaignTypeEventId = o.CampaignTypeEventId,
                                                                RegistrationStatusId = o.RegistrationStatusId,
                                                                SortOrder = o.SortOrder
                                                            },
                                                            CampaignTypeEventSortOrder = s1 == null ? "" : s1.SortOrder.ToString(),
                                                            RegistrationStatusDescription = s2 == null ? "" : s2.Description.ToString()
                                                        };

            var totalCount = await filteredCampaignTypeEventRegistrationStatuses.CountAsync();

            return new PagedResultDto<GetCampaignTypeEventRegistrationStatusForViewDto>(
                totalCount,
                await campaignTypeEventRegistrationStatuses.ToListAsync()
            );
        }

        public async Task<GetCampaignTypeEventRegistrationStatusForViewDto> GetCampaignTypeEventRegistrationStatusForView(long id)
        {
            var campaignTypeEventRegistrationStatus = await _campaignTypeEventRegistrationStatusRepository.GetAsync(id);

            var output = new GetCampaignTypeEventRegistrationStatusForViewDto { CampaignTypeEventRegistrationStatus = ObjectMapper.Map<CampaignTypeEventRegistrationStatusDto>(campaignTypeEventRegistrationStatus) };

            var _lookupCampaignTypeEvent = await _lookup_campaignTypeEventRepository.FirstOrDefaultAsync((long)output.CampaignTypeEventRegistrationStatus.CampaignTypeEventId);
            output.CampaignTypeEventSortOrder = _lookupCampaignTypeEvent.SortOrder.ToString();

            var _lookupRegistrationStatus = await _lookup_registrationStatusRepository.FirstOrDefaultAsync((long)output.CampaignTypeEventRegistrationStatus.RegistrationStatusId);
            output.RegistrationStatusDescription = _lookupRegistrationStatus.Description.ToString();

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Edit)]
        public async Task<GetCampaignTypeEventRegistrationStatusForEditOutput> GetCampaignTypeEventRegistrationStatusForEdit(EntityDto<long> input)
        {
            var campaignTypeEventRegistrationStatus = await _campaignTypeEventRegistrationStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCampaignTypeEventRegistrationStatusForEditOutput { CampaignTypeEventRegistrationStatus = ObjectMapper.Map<CreateOrEditCampaignTypeEventRegistrationStatusDto>(campaignTypeEventRegistrationStatus) };

            var _lookupCampaignTypeEvent = await _lookup_campaignTypeEventRepository.FirstOrDefaultAsync((long)output.CampaignTypeEventRegistrationStatus.CampaignTypeEventId);
            output.CampaignTypeEventSortOrder = _lookupCampaignTypeEvent.SortOrder.ToString();

            var _lookupRegistrationStatus = await _lookup_registrationStatusRepository.FirstOrDefaultAsync((long)output.CampaignTypeEventRegistrationStatus.RegistrationStatusId);
            output.RegistrationStatusDescription = _lookupRegistrationStatus.Description.ToString();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCampaignTypeEventRegistrationStatusDto input)
        {
            if ((input.Id == null || input.Id == 0) && !input.IsUpdate)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Create)]
        protected virtual async Task Create(CreateOrEditCampaignTypeEventRegistrationStatusDto input)
        {
            var campaignTypeEventRegistrationStatus = ObjectMapper.Map<CampaignTypeEventRegistrationStatus>(input);


            if (AbpSession.TenantId != null)
            {
                campaignTypeEventRegistrationStatus.TenantId = (int?)AbpSession.TenantId;
            }


            await _campaignTypeEventRegistrationStatusRepository.InsertAsync(campaignTypeEventRegistrationStatus);
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditCampaignTypeEventRegistrationStatusDto input)
        {
            var campaignTypeEventRegistrationStatus = await _campaignTypeEventRegistrationStatusRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, campaignTypeEventRegistrationStatus);
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _campaignTypeEventRegistrationStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCampaignTypeEventRegistrationStatusesToExcel(GetAllCampaignTypeEventRegistrationStatusesForExcelInput input)
        {

            var filteredCampaignTypeEventRegistrationStatuses = _campaignTypeEventRegistrationStatusRepository.GetAll()
                        .Include(e => e.CampaignTypeEventFk)
                        .Include(e => e.RegistrationStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
                        .WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeEventSortOrderFilter), e => e.CampaignTypeEventFk != null && e.CampaignTypeEventFk.SortOrder.ToString().ToLower() == input.CampaignTypeEventSortOrderFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationStatusDescriptionFilter), e => e.RegistrationStatusFk != null && e.RegistrationStatusFk.Description.ToLower() == input.RegistrationStatusDescriptionFilter.ToLower().Trim());

            var query = (from o in filteredCampaignTypeEventRegistrationStatuses
                         join o1 in _lookup_campaignTypeEventRepository.GetAll() on o.CampaignTypeEventId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_registrationStatusRepository.GetAll() on o.RegistrationStatusId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetCampaignTypeEventRegistrationStatusForViewDto()
                         {
                             CampaignTypeEventRegistrationStatus = new CampaignTypeEventRegistrationStatusDto
                             {
                                 SortOrder = o.SortOrder,
                                 Id = o.Id
                             },
                             CampaignTypeEventSortOrder = s1 == null ? "" : s1.SortOrder.ToString(),
                             RegistrationStatusDescription = s2 == null ? "" : s2.Description.ToString()
                         });


            var campaignTypeEventRegistrationStatusListDtos = await query.ToListAsync();

            return _campaignTypeEventRegistrationStatusesExcelExporter.ExportToFile(campaignTypeEventRegistrationStatusListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses)]
        public async Task<PagedResultDto<CampaignTypeEventRegistrationStatusCampaignTypeEventLookupTableDto>> GetAllCampaignTypeEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_campaignTypeEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.SortOrder.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var campaignTypeEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CampaignTypeEventRegistrationStatusCampaignTypeEventLookupTableDto>();
            foreach (var campaignTypeEvent in campaignTypeEventList)
            {
                lookupTableDtoList.Add(new CampaignTypeEventRegistrationStatusCampaignTypeEventLookupTableDto
                {
                    Id = campaignTypeEvent.Id,
                    DisplayName = campaignTypeEvent.SortOrder.ToString()
                });
            }

            return new PagedResultDto<CampaignTypeEventRegistrationStatusCampaignTypeEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEventRegistrationStatuses)]
        public async Task<PagedResultDto<CampaignTypeEventRegistrationStatusRegistrationStatusLookupTableDto>> GetAllRegistrationStatusForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_registrationStatusRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var registrationStatusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CampaignTypeEventRegistrationStatusRegistrationStatusLookupTableDto>();
            foreach (var registrationStatus in registrationStatusList)
            {
                lookupTableDtoList.Add(new CampaignTypeEventRegistrationStatusRegistrationStatusLookupTableDto
                {
                    Id = registrationStatus.Id,
                    DisplayName = registrationStatus.Description?.ToString()
                });
            }

            return new PagedResultDto<CampaignTypeEventRegistrationStatusRegistrationStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<PagedResultDto<GetCampaignTypeEventRegistrationStatusForViewDto>> GetAllCampaignTypeEventRegistrationStatus()
        {
            var filteredCampaignTypeEventRegistrationStatuses = _campaignTypeEventRegistrationStatusRepository.GetAll()
                        .Include(e => e.CampaignTypeEventFk)
                        .Include(e => e.RegistrationStatusFk);

            var campaignTypeEventRegistrationStatuses = from o in filteredCampaignTypeEventRegistrationStatuses
                                                        join o1 in _lookup_campaignTypeEventRepository.GetAll() on o.CampaignTypeEventId equals o1.Id into j1
                                                        from s1 in j1.DefaultIfEmpty()

                                                        join o2 in _lookup_registrationStatusRepository.GetAll() on o.RegistrationStatusId equals o2.Id into j2
                                                        from s2 in j2.DefaultIfEmpty()

                                                        select new GetCampaignTypeEventRegistrationStatusForViewDto()
                                                        {
                                                            CampaignTypeEventRegistrationStatus = new CampaignTypeEventRegistrationStatusDto
                                                            {
                                                                SortOrder = o.SortOrder,
                                                                Id = o.Id,
                                                                CampaignTypeEventId = o.CampaignTypeEventId,
                                                                RegistrationStatusId = o.RegistrationStatusId
                                                            },
                                                            CampaignTypeEventSortOrder = s1 == null ? "" : s1.SortOrder.ToString(),
                                                            RegistrationStatusDescription = s2 == null ? "" : s2.Description.ToString()
                                                        };

            var totalCount = await filteredCampaignTypeEventRegistrationStatuses.CountAsync();

            return new PagedResultDto<GetCampaignTypeEventRegistrationStatusForViewDto>(
                totalCount,
                await campaignTypeEventRegistrationStatuses.ToListAsync()
            );
        }
    }
}