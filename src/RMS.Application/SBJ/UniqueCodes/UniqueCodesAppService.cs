using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.UniqueCodes.Dtos;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.CampaignProcesses;
using Microsoft.AspNetCore.Mvc;

namespace RMS.SBJ.UniqueCodes
{
    [AbpAuthorize(AppPermissions.Pages_UniqueCodes)]
    public class UniqueCodesAppService : RMSAppServiceBase, IUniqueCodesAppService
    {
        private readonly IRepository<UniqueCode, string> _uniqueCodeRepository;
        private readonly IRepository<UniqueCodeByCampaign, string> _uniqueCodeByCampaignRepository;
        private readonly IRepository<Campaign, long> _lookup_campaignRepository;

        public UniqueCodesAppService(IRepository<UniqueCode, string> uniqueCodeRepository, IRepository<UniqueCodeByCampaign, string> uniqueCodeByCampaignRepository, IRepository<Campaign, long> lookup_campaignRepository)
        {
            _uniqueCodeByCampaignRepository = uniqueCodeByCampaignRepository;
            _uniqueCodeRepository = uniqueCodeRepository;
            _lookup_campaignRepository = lookup_campaignRepository;
        }

        public async Task<PagedResultDto<GetUniqueCodeForViewDto>> GetAll(GetAllUniqueCodesInput input)
        {

            var filteredUniqueCodes = _uniqueCodeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.UsedFilter.HasValue && input.UsedFilter > -1, e => (input.UsedFilter == 1 && e.Used) || (input.UsedFilter == 0 && !e.Used));

            var pagedAndFilteredUniqueCodes = filteredUniqueCodes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var uniqueCodes = from o in pagedAndFilteredUniqueCodes
                              select new
                              {

                                  o.Used,
                                  Id = o.Id
                              };

            var totalCount = await filteredUniqueCodes.CountAsync();

            var dbList = await uniqueCodes.ToListAsync();
            var results = new List<GetUniqueCodeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetUniqueCodeForViewDto()
                {
                    UniqueCode = new UniqueCodeDto
                    {

                        Used = o.Used,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetUniqueCodeForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_UniqueCodes_Edit)]
        public async Task<GetUniqueCodeForEditOutput> GetUniqueCodeForEdit(EntityDto<string> input)
        {
            var uniqueCode = await _uniqueCodeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetUniqueCodeForEditOutput { UniqueCode = ObjectMapper.Map<CreateOrEditUniqueCodeDto>(uniqueCode) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditUniqueCodeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_UniqueCodes_Create)]
        protected virtual async Task Create(CreateOrEditUniqueCodeDto input)
        {
            var uniqueCode = ObjectMapper.Map<UniqueCode>(input);

            if (uniqueCode.Id.IsNullOrWhiteSpace())
            {
                uniqueCode.Id = Guid.NewGuid().ToString();
            }

            await _uniqueCodeRepository.InsertAsync(uniqueCode);
        }

        [AbpAuthorize(AppPermissions.Pages_UniqueCodes_Edit)]
        protected virtual async Task Update(CreateOrEditUniqueCodeDto input)
        {
            var uniqueCode = await _uniqueCodeRepository.FirstOrDefaultAsync((string)input.Id);
            ObjectMapper.Map(input, uniqueCode);
        }

        [AbpAuthorize(AppPermissions.Pages_UniqueCodes_Delete)]
        public async Task Delete(EntityDto<string> input)
        {
            await _uniqueCodeRepository.DeleteAsync(input.Id);
        }

        // Custom Methods for API calls
        public async Task<bool> IsCodeValid(string code)
        {
            var uniqueCode = await _uniqueCodeRepository.FirstOrDefaultAsync(code.Trim());

            if (uniqueCode == null) 
            {
                return false;
            }

            if (uniqueCode.Used == true)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsCodeValidByCampaign(string code, string? campaignCode)
        {
            
            var campiagnLookup = await _lookup_campaignRepository.GetAll().Where(cc => cc.CampaignCode == Convert.ToInt32(campaignCode.Trim())).FirstOrDefaultAsync();
            var uniqueCodeByCampaign = await _uniqueCodeByCampaignRepository.GetAll().Where(uc => uc.Id.Trim() == code.Trim() && uc.CampaignId == campiagnLookup.Id).FirstOrDefaultAsync();

            if (uniqueCodeByCampaign == null)
            {
                return false;
            }

            if (uniqueCodeByCampaign.Used == true)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SetCodeUsed(string code)
        {
            var uniqueCode = await _uniqueCodeRepository.FirstOrDefaultAsync(code.Trim());

            if (uniqueCode == null)
            {
                return false;
            }

            uniqueCode.Used = true;

            return true;
        }

        public async Task<bool> SetCodeUsedByCampaign(string code, string campaignCode)
        {
            var campiagnLookup = await _lookup_campaignRepository.GetAll().Where(cc => cc.CampaignCode == Convert.ToInt32(campaignCode.Trim())).FirstOrDefaultAsync();
            var uniqueCodeByCampaign = await _uniqueCodeByCampaignRepository.GetAll().Where(uc => uc.Id.Trim() == code.Trim() && uc.CampaignId == campiagnLookup.Id).FirstOrDefaultAsync();

            if (uniqueCodeByCampaign == null)
            {
                return false;
            }

            uniqueCodeByCampaign.Used = true;

            return true;
        }

    }
}