using RMS.SBJ.CampaignProcesses;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.UniqueCodes.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using RMS.Storage;

namespace RMS.SBJ.UniqueCodes
{
    [AbpAuthorize(AppPermissions.Pages_UniqueCodeByCampaigns)]
    public class UniqueCodeByCampaignsAppService : RMSAppServiceBase, IUniqueCodeByCampaignsAppService
    {
        private readonly IRepository<UniqueCodeByCampaign, string> _uniqueCodeByCampaignRepository;
        private readonly IRepository<Campaign, long> _lookup_campaignRepository;

        public UniqueCodeByCampaignsAppService(IRepository<UniqueCodeByCampaign, string> uniqueCodeByCampaignRepository, IRepository<Campaign, long> lookup_campaignRepository)
        {
            _uniqueCodeByCampaignRepository = uniqueCodeByCampaignRepository;
            _lookup_campaignRepository = lookup_campaignRepository;

        }

        public async Task<PagedResultDto<GetUniqueCodeByCampaignForViewDto>> GetAll(GetAllUniqueCodeByCampaignsInput input)
        {

            var filteredUniqueCodeByCampaigns = _uniqueCodeByCampaignRepository.GetAll()
                        .Include(e => e.CampaignFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.UsedFilter.HasValue && input.UsedFilter > -1, e => (input.UsedFilter == 1 && e.Used) || (input.UsedFilter == 0 && !e.Used))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter);

            var pagedAndFilteredUniqueCodeByCampaigns = filteredUniqueCodeByCampaigns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var uniqueCodeByCampaigns = from o in pagedAndFilteredUniqueCodeByCampaigns
                                        join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()

                                        select new
                                        {

                                            o.Used,
                                            Id = o.Id,
                                            CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                        };

            var totalCount = await filteredUniqueCodeByCampaigns.CountAsync();

            var dbList = await uniqueCodeByCampaigns.ToListAsync();
            var results = new List<GetUniqueCodeByCampaignForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetUniqueCodeByCampaignForViewDto()
                {
                    UniqueCodeByCampaign = new UniqueCodeByCampaignDto
                    {

                        Used = o.Used,
                        Id = o.Id,
                    },
                    CampaignName = o.CampaignName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetUniqueCodeByCampaignForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetUniqueCodeByCampaignForViewDto> GetUniqueCodeByCampaignForView(string id)
        {
            var uniqueCodeByCampaign = await _uniqueCodeByCampaignRepository.GetAsync(id);

            var output = new GetUniqueCodeByCampaignForViewDto { UniqueCodeByCampaign = ObjectMapper.Map<UniqueCodeByCampaignDto>(uniqueCodeByCampaign) };

            if (output.UniqueCodeByCampaign.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.UniqueCodeByCampaign.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_UniqueCodeByCampaigns_Edit)]
        public async Task<GetUniqueCodeByCampaignForEditOutput> GetUniqueCodeByCampaignForEdit(EntityDto<string> input)
        {
            var uniqueCodeByCampaign = await _uniqueCodeByCampaignRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetUniqueCodeByCampaignForEditOutput { UniqueCodeByCampaign = ObjectMapper.Map<CreateOrEditUniqueCodeByCampaignDto>(uniqueCodeByCampaign) };

            if (output.UniqueCodeByCampaign.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.UniqueCodeByCampaign.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditUniqueCodeByCampaignDto input)
        {
            if (input.Id.IsNullOrWhiteSpace())
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_UniqueCodeByCampaigns_Create)]
        protected virtual async Task Create(CreateOrEditUniqueCodeByCampaignDto input)
        {
            var uniqueCodeByCampaign = ObjectMapper.Map<UniqueCodeByCampaign>(input);

            if (uniqueCodeByCampaign.Id.IsNullOrWhiteSpace())
            {
                uniqueCodeByCampaign.Id = Guid.NewGuid().ToString();
            }

            await _uniqueCodeByCampaignRepository.InsertAsync(uniqueCodeByCampaign);

        }

        [AbpAuthorize(AppPermissions.Pages_UniqueCodeByCampaigns_Edit)]
        protected virtual async Task Update(CreateOrEditUniqueCodeByCampaignDto input)
        {
            var uniqueCodeByCampaign = await _uniqueCodeByCampaignRepository.FirstOrDefaultAsync((string)input.Id);
            ObjectMapper.Map(input, uniqueCodeByCampaign);

        }

        [AbpAuthorize(AppPermissions.Pages_UniqueCodeByCampaigns_Delete)]
        public async Task Delete(EntityDto<string> input)
        {
            await _uniqueCodeByCampaignRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_UniqueCodeByCampaigns)]
        public async Task<PagedResultDto<UniqueCodeByCampaignCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_campaignRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var campaignList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<UniqueCodeByCampaignCampaignLookupTableDto>();
            foreach (var campaign in campaignList)
            {
                lookupTableDtoList.Add(new UniqueCodeByCampaignCampaignLookupTableDto
                {
                    Id = campaign.Id,
                    DisplayName = campaign.Name?.ToString()
                });
            }

            return new PagedResultDto<UniqueCodeByCampaignCampaignLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}