

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CodeTypeTables.Exporting;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using RMS.PromoPlanner.Dtos;

namespace RMS.SBJ.CodeTypeTables
{
    [AbpAuthorize(AppPermissions.Pages_Countries)]
    public class CountriesAppService : RMSAppServiceBase, ICountriesAppService
    {
        private readonly IRepository<Country, long> _countryRepository;
        private readonly ICountriesExcelExporter _countriesExcelExporter;


        public CountriesAppService(IRepository<Country, long> countryRepository, ICountriesExcelExporter countriesExcelExporter)
        {
            _countryRepository = countryRepository;
            _countriesExcelExporter = countriesExcelExporter;

        }

        public async Task<IEnumerable<GetCountryForViewDto>> GetAll()
        {
            var countries = await _countryRepository.GetAll().Select(x => new GetCountryForViewDto
            {
                Country = new CountryDto { Id = x.Id, CountryCode = x.CountryCode, Description = x.Description }
            }).ToListAsync();

            return countries;
        }

        public async Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input)
        {

            var filteredCountries = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CountryCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCodeFilter), e => e.CountryCode == input.CountryCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredCountries = filteredCountries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var countries = from o in pagedAndFilteredCountries
                            select new GetCountryForViewDto()
                            {
                                Country = new CountryDto
                                {
                                    CountryCode = o.CountryCode,
                                    Description = o.Description,
                                    Id = o.Id
                                }
                            };

            var totalCount = await filteredCountries.CountAsync();

            return new PagedResultDto<GetCountryForViewDto>(
                totalCount,
                await countries.ToListAsync()
            );
        }

        public async Task<IEnumerable<CustomPromoCountryForView>> GetAllWithoutPaging()
        {
            var countries = _countryRepository.GetAll().OrderBy(x => x.Description)
                .Select(x => new CustomPromoCountryForView
                {
                    CountryId = x.Id,
                    CountryCode = x.CountryCode,
                    CountryName = x.Description
                });

            return await countries.ToListAsync();
        }

        public async Task<GetCountryForViewDto> GetCountryForView(long id)
        {
            var country = await _countryRepository.GetAsync(id);

            var output = new GetCountryForViewDto { Country = ObjectMapper.Map<CountryDto>(country) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
        public async Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto<long> input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCountryForEditOutput { Country = ObjectMapper.Map<CreateOrEditCountryDto>(country) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCountryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Countries_Create)]
        protected virtual async Task Create(CreateOrEditCountryDto input)
        {
            var country = ObjectMapper.Map<Country>(input);


            if (AbpSession.TenantId != null)
            {
                country.TenantId = (int?)AbpSession.TenantId;
            }


            await _countryRepository.InsertAsync(country);
        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
        protected virtual async Task Update(CreateOrEditCountryDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, country);
        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _countryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input)
        {

            var filteredCountries = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CountryCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCodeFilter), e => e.CountryCode == input.CountryCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredCountries
                         select new GetCountryForViewDto()
                         {
                             Country = new CountryDto
                             {
                                 CountryCode = o.CountryCode,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });


            var countryListDtos = await query.ToListAsync();

            return _countriesExcelExporter.ExportToFile(countryListDtos);
        }


    }
}