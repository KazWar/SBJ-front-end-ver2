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
    [AbpAuthorize(AppPermissions.Pages_CampaignTypeEvents)]
    public class CampaignTypeEventsAppService : RMSAppServiceBase, ICampaignTypeEventsAppService
    {
        private readonly IRepository<CampaignTypeEvent, long> _campaignTypeEventRepository;
        private readonly ICampaignTypeEventsExcelExporter _campaignTypeEventsExcelExporter;
        private readonly IRepository<CampaignType, long> _lookup_campaignTypeRepository;
        private readonly IRepository<ProcessEvent, long> _lookup_processEventRepository;

        public CampaignTypeEventsAppService(IRepository<CampaignTypeEvent, long> campaignTypeEventRepository, ICampaignTypeEventsExcelExporter campaignTypeEventsExcelExporter, IRepository<CampaignType, long> lookup_campaignTypeRepository, IRepository<ProcessEvent, long> lookup_processEventRepository)
        {
            _campaignTypeEventRepository = campaignTypeEventRepository;
            _campaignTypeEventsExcelExporter = campaignTypeEventsExcelExporter;
            _lookup_campaignTypeRepository = lookup_campaignTypeRepository;
            _lookup_processEventRepository = lookup_processEventRepository;
        }

        public async Task<GetCampaignTypeEventForViewDto> GetByIdAsync(long campaignTypeEventId)
        {
            var campaignTypeEvent = await _campaignTypeEventRepository.GetAll().Where(cte => cte.Id == campaignTypeEventId).FirstOrDefaultAsync();
            if (campaignTypeEvent == null) return null; // because there are a few database entries that are corrupt and it tries to lookup IDs that do not exist

            var output = new GetCampaignTypeEventForViewDto { CampaignTypeEvent = ObjectMapper.Map<CampaignTypeEventDto>(campaignTypeEvent) };
            var lookupCampaignType = await _lookup_campaignTypeRepository.FirstOrDefaultAsync(output.CampaignTypeEvent.CampaignTypeId);
            var lookupProcessEvent = await _lookup_processEventRepository.FirstOrDefaultAsync(output.CampaignTypeEvent.ProcessEventId);

            output.CampaignTypeName = lookupCampaignType.Name.ToString();
            output.ProcessEventName = lookupProcessEvent.Name.ToString();

            return output;
        }

        public async Task<PagedResultDto<GetCampaignTypeEventForViewDto>> GetAll(GetAllCampaignTypeEventsInput input)
        {
            var filteredCampaignTypeEvents = _campaignTypeEventRepository.GetAll()
                .Include(e => e.CampaignTypeFk)
                .Include(e => e.ProcessEventFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                .WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
                .WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeNameFilter), e => e.CampaignTypeFk != null && e.CampaignTypeFk.Name.ToLower() == input.CampaignTypeNameFilter.ToLower().Trim())
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessEventNameFilter), e => e.ProcessEventFk != null && e.ProcessEventFk.Name.ToLower() == input.ProcessEventNameFilter.ToLower().Trim());

            var pagedAndFilteredCampaignTypeEvents = filteredCampaignTypeEvents
                .OrderBy(input.Sorting ?? "id asc");
                // .PageBy(input);

            var campaignTypeEvents = 
                from o in pagedAndFilteredCampaignTypeEvents
                join o1 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()

                join o2 in _lookup_processEventRepository.GetAll() on o.ProcessEventId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()

                select new GetCampaignTypeEventForViewDto()
                {
                    CampaignTypeEvent = new CampaignTypeEventDto
                    {
                        SortOrder = o.SortOrder,
                        Id = o.Id
                    },
                    CampaignTypeName = s1 == null ? "" : s1.Name.ToString(),
                    ProcessEventName = s2 == null ? "" : s2.Name.ToString()
                };

            var totalCount = await filteredCampaignTypeEvents.CountAsync();

            return new PagedResultDto<GetCampaignTypeEventForViewDto>(
                totalCount,
                await campaignTypeEvents.ToListAsync()
            );
        }

        public async Task<GetCampaignTypeEventForViewDto> GetCampaignTypeEventForView(long id)
        {
            var campaignTypeEvent = await _campaignTypeEventRepository.GetAsync(id);

            var output = new GetCampaignTypeEventForViewDto { CampaignTypeEvent = ObjectMapper.Map<CampaignTypeEventDto>(campaignTypeEvent) };

            if (output.CampaignTypeEvent.CampaignTypeId != null)
            {
                var _lookupCampaignType = await _lookup_campaignTypeRepository.FirstOrDefaultAsync((long)output.CampaignTypeEvent.CampaignTypeId);
                output.CampaignTypeName = _lookupCampaignType.Name.ToString();
            }

            if (output.CampaignTypeEvent.ProcessEventId != null)
            {
                var _lookupProcessEvent = await _lookup_processEventRepository.FirstOrDefaultAsync((long)output.CampaignTypeEvent.ProcessEventId);
                output.ProcessEventName = _lookupProcessEvent.Name.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEvents_Edit)]
        public async Task<GetCampaignTypeEventForEditOutput> GetCampaignTypeEventForEdit(EntityDto<long> input)
        {
            var campaignTypeEvent = await _campaignTypeEventRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCampaignTypeEventForEditOutput { CampaignTypeEvent = ObjectMapper.Map<CreateOrEditCampaignTypeEventDto>(campaignTypeEvent) };

            if (output.CampaignTypeEvent.CampaignTypeId != null)
            {
                var _lookupCampaignType = await _lookup_campaignTypeRepository.FirstOrDefaultAsync((long)output.CampaignTypeEvent.CampaignTypeId);
                output.CampaignTypeName = _lookupCampaignType.Name.ToString();
            }

            if (output.CampaignTypeEvent.ProcessEventId != null)
            {
                var _lookupProcessEvent = await _lookup_processEventRepository.FirstOrDefaultAsync((long)output.CampaignTypeEvent.ProcessEventId);
                output.ProcessEventName = _lookupProcessEvent.Name.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCampaignTypeEventDto input)
        {
            if (input.Id == 0 && !input.IsUpdate)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEvents_Create)]
        protected virtual async Task Create(CreateOrEditCampaignTypeEventDto input)
        {
            var campaignTypeEvent = ObjectMapper.Map<CampaignTypeEvent>(input);


            if (AbpSession.TenantId != null)
            {
                campaignTypeEvent.TenantId = (int?)AbpSession.TenantId;
            }


            await _campaignTypeEventRepository.InsertAsync(campaignTypeEvent);
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEvents_Edit)]
        protected virtual async Task Update(CreateOrEditCampaignTypeEventDto input)
        {
            var campaignTypeEvent = await _campaignTypeEventRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, campaignTypeEvent);
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEvents_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _campaignTypeEventRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCampaignTypeEventsToExcel(GetAllCampaignTypeEventsForExcelInput input)
        {

            var filteredCampaignTypeEvents = _campaignTypeEventRepository.GetAll()
                .Include(e => e.CampaignTypeFk)
                .Include(e => e.ProcessEventFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                .WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
                .WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeNameFilter), e => e.CampaignTypeFk != null && e.CampaignTypeFk.Name == input.CampaignTypeNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessEventNameFilter), e => e.ProcessEventFk != null && e.ProcessEventFk.Name == input.ProcessEventNameFilter);

            var query =
                from o in filteredCampaignTypeEvents
                join o1 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()

                join o2 in _lookup_processEventRepository.GetAll() on o.ProcessEventId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()

                select new GetCampaignTypeEventForViewDto()
                {
                    CampaignTypeEvent = new CampaignTypeEventDto
                    {
                        SortOrder = o.SortOrder,
                        Id = o.Id
                    },
                    CampaignTypeName = s1 == null ? "" : s1.Name.ToString(),
                    ProcessEventName = s2 == null ? "" : s2.Name.ToString()
                };

            var campaignTypeEventListDtos = await query.ToListAsync();

            return _campaignTypeEventsExcelExporter.ExportToFile(campaignTypeEventListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEvents)]
        public async Task<PagedResultDto<CampaignTypeEventCampaignTypeLookupTableDto>> GetAllCampaignTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_campaignTypeRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name.ToString().Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var campaignTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CampaignTypeEventCampaignTypeLookupTableDto>();
            foreach (var campaignType in campaignTypeList)
            {
                lookupTableDtoList.Add(new CampaignTypeEventCampaignTypeLookupTableDto
                {
                    Id = campaignType.Id,
                    DisplayName = campaignType.Name?.ToString()
                });
            }

            return new PagedResultDto<CampaignTypeEventCampaignTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypeEvents)]
        public async Task<PagedResultDto<CampaignTypeEventProcessEventLookupTableDto>> GetAllProcessEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_processEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var processEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CampaignTypeEventProcessEventLookupTableDto>();
            foreach (var processEvent in processEventList)
            {
                lookupTableDtoList.Add(new CampaignTypeEventProcessEventLookupTableDto
                {
                    Id = processEvent.Id,
                    DisplayName = processEvent.Name?.ToString()
                });
            }

            return new PagedResultDto<CampaignTypeEventProcessEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<PagedResultDto<GetCampaignTypeEventForViewDto>> GetAll()
        {
            var filteredCampaignTypeEvents = _campaignTypeEventRepository.GetAll();

            var campaignTypeEvents = 
                from o in filteredCampaignTypeEvents
                join o1 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()

                join o2 in _lookup_processEventRepository.GetAll() on o.ProcessEventId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()

                select new GetCampaignTypeEventForViewDto()
                {
                    CampaignTypeEvent = new CampaignTypeEventDto
                    {
                        SortOrder = o.SortOrder,
                        Id = o.Id,
                        CampaignTypeId = o.CampaignTypeId,
                        ProcessEventId = o.ProcessEventId
                    },
                    CampaignTypeName = s1 == null ? "" : s1.Name.ToString(),
                    ProcessEventName = s2 == null ? "" : s2.Name.ToString()
                };

            var totalCount = await campaignTypeEvents.CountAsync();

            return new PagedResultDto<GetCampaignTypeEventForViewDto>(
                totalCount,
                await campaignTypeEvents.ToListAsync()
            );
        }
    }
}