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
    [AbpAuthorize(AppPermissions.Pages_CampaignCountries)]
    public class CampaignCountriesAppService : RMSAppServiceBase, ICampaignCountriesAppService
    {
        private readonly IRepository<CampaignCountry, long> _campaignCountryRepository;
        private readonly ICampaignCountriesExcelExporter _campaignCountriesExcelExporter;
        private readonly IRepository<Campaign, long> _lookup_campaignRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;

        public CampaignCountriesAppService(IRepository<CampaignCountry, long> campaignCountryRepository, ICampaignCountriesExcelExporter campaignCountriesExcelExporter, IRepository<Campaign, long> lookup_campaignRepository, IRepository<Country, long> lookup_countryRepository)
        {
            _campaignCountryRepository = campaignCountryRepository;
            _campaignCountriesExcelExporter = campaignCountriesExcelExporter;
            _lookup_campaignRepository = lookup_campaignRepository;
            _lookup_countryRepository = lookup_countryRepository;

        }

        public async Task<PagedResultDto<GetCampaignCountryForViewDto>> GetAll(GetAllCampaignCountriesInput input)
        {

            var filteredCampaignCountries = _campaignCountryRepository.GetAll()
                        .Include(e => e.CampaignFk)
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryDescriptionFilter), e => e.CountryFk != null && e.CountryFk.Description == input.CountryDescriptionFilter);

            var pagedAndFilteredCampaignCountries = filteredCampaignCountries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var campaignCountries = from o in pagedAndFilteredCampaignCountries
                                    join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    select new
                                    {

                                        Id = o.Id,
                                        CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                        CountryDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                                    };

            var totalCount = await filteredCampaignCountries.CountAsync();

            var dbList = await campaignCountries.ToListAsync();
            var results = new List<GetCampaignCountryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCampaignCountryForViewDto()
                {
                    CampaignCountry = new CampaignCountryDto
                    {

                        Id = o.Id,
                    },
                    CampaignName = o.CampaignName,
                    CountryDescription = o.CountryDescription
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCampaignCountryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCampaignCountryForViewDto> GetCampaignCountryForView(long id)
        {
            var campaignCountry = await _campaignCountryRepository.GetAsync(id);

            var output = new GetCampaignCountryForViewDto { CampaignCountry = ObjectMapper.Map<CampaignCountryDto>(campaignCountry) };

            if (output.CampaignCountry.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.CampaignCountry.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

            if (output.CampaignCountry.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.CampaignCountry.CountryId);
                output.CountryDescription = _lookupCountry?.Description?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignCountries_Edit)]
        public async Task<GetCampaignCountryForEditOutput> GetCampaignCountryForEdit(EntityDto<long> input)
        {
            var campaignCountry = await _campaignCountryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCampaignCountryForEditOutput { CampaignCountry = ObjectMapper.Map<CreateOrEditCampaignCountryDto>(campaignCountry) };

            if (output.CampaignCountry.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.CampaignCountry.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

            if (output.CampaignCountry.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.CampaignCountry.CountryId);
                output.CountryDescription = _lookupCountry?.Description?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCampaignCountryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CampaignCountries_Create)]
        protected virtual async Task Create(CreateOrEditCampaignCountryDto input)
        {
            var campaignCountry = ObjectMapper.Map<CampaignCountry>(input);

            if (AbpSession.TenantId != null)
            {
                campaignCountry.TenantId = (int?)AbpSession.TenantId;
            }

            await _campaignCountryRepository.InsertAsync(campaignCountry);

        }

        [AbpAuthorize(AppPermissions.Pages_CampaignCountries_Edit)]
        protected virtual async Task Update(CreateOrEditCampaignCountryDto input)
        {
            var campaignCountry = await _campaignCountryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, campaignCountry);

        }

        [AbpAuthorize(AppPermissions.Pages_CampaignCountries_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _campaignCountryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCampaignCountriesToExcel(GetAllCampaignCountriesForExcelInput input)
        {

            var filteredCampaignCountries = _campaignCountryRepository.GetAll()
                        .Include(e => e.CampaignFk)
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryDescriptionFilter), e => e.CountryFk != null && e.CountryFk.Description == input.CountryDescriptionFilter);

            var query = (from o in filteredCampaignCountries
                         join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetCampaignCountryForViewDto()
                         {
                             CampaignCountry = new CampaignCountryDto
                             {
                                 Id = o.Id
                             },
                             CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CountryDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                         });

            var campaignCountryListDtos = await query.ToListAsync();

            return _campaignCountriesExcelExporter.ExportToFile(campaignCountryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignCountries)]
        public async Task<PagedResultDto<CampaignCountryCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_campaignRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var campaignList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CampaignCountryCampaignLookupTableDto>();
            foreach (var campaign in campaignList)
            {
                lookupTableDtoList.Add(new CampaignCountryCampaignLookupTableDto
                {
                    Id = campaign.Id,
                    DisplayName = campaign.Name?.ToString()
                });
            }

            return new PagedResultDto<CampaignCountryCampaignLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignCountries)]
        public async Task<PagedResultDto<CampaignCountryCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_countryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var countryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CampaignCountryCountryLookupTableDto>();
            foreach (var country in countryList)
            {
                lookupTableDtoList.Add(new CampaignCountryCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country.Description?.ToString()
                });
            }

            return new PagedResultDto<CampaignCountryCountryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}