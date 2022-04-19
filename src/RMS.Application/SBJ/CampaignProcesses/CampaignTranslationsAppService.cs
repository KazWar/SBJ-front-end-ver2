using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CodeTypeTables;

using System;
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
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses
{
    [AbpAuthorize(AppPermissions.Pages_CampaignTranslations)]
    public class CampaignTranslationsAppService : RMSAppServiceBase, ICampaignTranslationsAppService
    {
        private readonly IRepository<CampaignTranslation, long> _campaignTranslationRepository;
        private readonly ICampaignTranslationsExcelExporter _campaignTranslationsExcelExporter;
        private readonly IRepository<Campaign, long> _lookup_campaignRepository;
        private readonly IRepository<Locale, long> _lookup_localeRepository;

        public CampaignTranslationsAppService(IRepository<CampaignTranslation, long> campaignTranslationRepository, ICampaignTranslationsExcelExporter campaignTranslationsExcelExporter, IRepository<Campaign, long> lookup_campaignRepository, IRepository<Locale, long> lookup_localeRepository)
        {
            _campaignTranslationRepository = campaignTranslationRepository;
            _campaignTranslationsExcelExporter = campaignTranslationsExcelExporter;
            _lookup_campaignRepository = lookup_campaignRepository;
            _lookup_localeRepository = lookup_localeRepository;

        }

        public async Task<PagedResultDto<GetCampaignTranslationForViewDto>> GetAll(GetAllCampaignTranslationsInput input)
        {

            var filteredCampaignTranslations = _campaignTranslationRepository.GetAll()
                        .Include(e => e.CampaignFk)
                        .Include(e => e.LocaleFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocaleDescriptionFilter), e => e.LocaleFk != null && e.LocaleFk.Description == input.LocaleDescriptionFilter);

            var pagedAndFilteredCampaignTranslations = filteredCampaignTranslations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var campaignTranslations = from o in pagedAndFilteredCampaignTranslations
                                       join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       join o2 in _lookup_localeRepository.GetAll() on o.LocaleId equals o2.Id into j2
                                       from s2 in j2.DefaultIfEmpty()

                                       select new
                                       {

                                           o.Name,
                                           o.Description,
                                           Id = o.Id,
                                           CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                           LocaleDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                                       };

            var totalCount = await filteredCampaignTranslations.CountAsync();

            var dbList = await campaignTranslations.ToListAsync();
            var results = new List<GetCampaignTranslationForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCampaignTranslationForViewDto()
                {
                    CampaignTranslation = new CampaignTranslationDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,
                    },
                    CampaignName = o.CampaignName,
                    LocaleDescription = o.LocaleDescription
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCampaignTranslationForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCampaignTranslationForViewDto> GetCampaignTranslationForView(long id)
        {
            var campaignTranslation = await _campaignTranslationRepository.GetAsync(id);

            var output = new GetCampaignTranslationForViewDto { CampaignTranslation = ObjectMapper.Map<CampaignTranslationDto>(campaignTranslation) };

            if (output.CampaignTranslation.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.CampaignTranslation.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

            if (output.CampaignTranslation.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.CampaignTranslation.LocaleId);
                output.LocaleDescription = _lookupLocale?.Description?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTranslations_Edit)]
        public async Task<GetCampaignTranslationForEditOutput> GetCampaignTranslationForEdit(EntityDto<long> input)
        {
            var campaignTranslation = await _campaignTranslationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCampaignTranslationForEditOutput { CampaignTranslation = ObjectMapper.Map<CreateOrEditCampaignTranslationDto>(campaignTranslation) };

            if (output.CampaignTranslation.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.CampaignTranslation.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

            if (output.CampaignTranslation.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.CampaignTranslation.LocaleId);
                output.LocaleDescription = _lookupLocale?.Description?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCampaignTranslationDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CampaignTranslations_Create)]
        protected virtual async Task Create(CreateOrEditCampaignTranslationDto input)
        {
            var campaignTranslation = ObjectMapper.Map<CampaignTranslation>(input);

            if (AbpSession.TenantId != null)
            {
                campaignTranslation.TenantId = (int?)AbpSession.TenantId;
            }

            await _campaignTranslationRepository.InsertAsync(campaignTranslation);

        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTranslations_Edit)]
        protected virtual async Task Update(CreateOrEditCampaignTranslationDto input)
        {
            var campaignTranslation = await _campaignTranslationRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, campaignTranslation);

        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTranslations_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _campaignTranslationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCampaignTranslationsToExcel(GetAllCampaignTranslationsForExcelInput input)
        {

            var filteredCampaignTranslations = _campaignTranslationRepository.GetAll()
                        .Include(e => e.CampaignFk)
                        .Include(e => e.LocaleFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocaleDescriptionFilter), e => e.LocaleFk != null && e.LocaleFk.Description == input.LocaleDescriptionFilter);

            var query = (from o in filteredCampaignTranslations
                         join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_localeRepository.GetAll() on o.LocaleId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetCampaignTranslationForViewDto()
                         {
                             CampaignTranslation = new CampaignTranslationDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Id = o.Id
                             },
                             CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             LocaleDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                         });

            var campaignTranslationListDtos = await query.ToListAsync();

            return _campaignTranslationsExcelExporter.ExportToFile(campaignTranslationListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTranslations)]
        public async Task<PagedResultDto<CampaignTranslationCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_campaignRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var campaignList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CampaignTranslationCampaignLookupTableDto>();
            foreach (var campaign in campaignList)
            {
                lookupTableDtoList.Add(new CampaignTranslationCampaignLookupTableDto
                {
                    Id = campaign.Id,
                    DisplayName = campaign.Name?.ToString()
                });
            }

            return new PagedResultDto<CampaignTranslationCampaignLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTranslations)]
        public async Task<PagedResultDto<CampaignTranslationLocaleLookupTableDto>> GetAllLocaleForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_localeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var localeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CampaignTranslationLocaleLookupTableDto>();
            foreach (var locale in localeList)
            {
                lookupTableDtoList.Add(new CampaignTranslationLocaleLookupTableDto
                {
                    Id = locale.Id,
                    DisplayName = locale.Description?.ToString()
                });
            }

            return new PagedResultDto<CampaignTranslationLocaleLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}