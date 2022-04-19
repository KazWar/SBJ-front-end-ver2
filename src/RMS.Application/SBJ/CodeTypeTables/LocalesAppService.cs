using RMS.SBJ.CodeTypeTables;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
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
using RMS.SBJ.Forms;

namespace RMS.SBJ.CodeTypeTables
{
    [AbpAuthorize(AppPermissions.Pages_Locales)]
    public class LocalesAppService : RMSAppServiceBase, ILocalesAppService
    {
        private readonly IRepository<Locale, long> _localeRepository;
        private readonly ILocalesExcelExporter _localesExcelExporter;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<Form, long> _formRepository;
        private readonly IRepository<FormLocale, long> _formLocaleRepository;

        public LocalesAppService(IRepository<Locale, long> localeRepository,
                                 ILocalesExcelExporter localesExcelExporter,
                                 IRepository<Country, long> lookup_countryRepository,
                                 IRepository<Form, long> formRepository,
                                 IRepository<FormLocale, long> formLocaleRepository)
        {
            _localeRepository = localeRepository;
            _localesExcelExporter = localesExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;
            _formRepository = formRepository;
            _formLocaleRepository = formLocaleRepository;
        }

        public async Task<PagedResultDto<GetLocaleForViewDto>> GetAll(GetAllLocalesInput input)
        {

            var filteredLocales = _localeRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LanguageCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LanguageCodeFilter), e => e.LanguageCode == input.LanguageCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCountryCodeFilter), e => e.CountryFk != null && e.CountryFk.CountryCode == input.CountryCountryCodeFilter);

            var pagedAndFilteredLocales = filteredLocales
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var locales = from o in pagedAndFilteredLocales
                          join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty()

                          select new GetLocaleForViewDto()
                          {
                              Locale = new LocaleDto
                              {
                                  LanguageCode = o.LanguageCode,
                                  Description = o.Description,
                                  IsActive = o.IsActive,
                                  Id = o.Id
                              },
                              CountryCountryCode = s1 == null || s1.CountryCode == null ? "" : s1.CountryCode.ToString()
                          };

            var totalCount = await filteredLocales.CountAsync();

            return new PagedResultDto<GetLocaleForViewDto>(
                totalCount,
                await locales.ToListAsync()
            );
        }

        public async Task<GetLocaleForViewDto> GetLocaleForView(long id)
        {
            var locale = await _localeRepository.GetAsync(id);

            var output = new GetLocaleForViewDto { Locale = ObjectMapper.Map<LocaleDto>(locale) };

            if (output.Locale.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Locale.CountryId);
                output.CountryCountryCode = _lookupCountry?.CountryCode?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Locales_Edit)]
        public async Task<GetLocaleForEditOutput> GetLocaleForEdit(EntityDto<long> input)
        {
            var locale = await _localeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLocaleForEditOutput { Locale = ObjectMapper.Map<CreateOrEditLocaleDto>(locale) };

            if (output.Locale.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Locale.CountryId);
                output.CountryCountryCode = _lookupCountry?.CountryCode?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLocaleDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Locales_Create)]
        protected virtual async Task Create(CreateOrEditLocaleDto input)
        {
            var locale = ObjectMapper.Map<Locale>(input);


            if (AbpSession.TenantId != null)
            {
                locale.TenantId = (int?)AbpSession.TenantId;
            }


            await _localeRepository.InsertAsync(locale);
        }

        [AbpAuthorize(AppPermissions.Pages_Locales_Edit)]
        protected virtual async Task Update(CreateOrEditLocaleDto input)
        {
            var locale = await _localeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, locale);
        }

        [AbpAuthorize(AppPermissions.Pages_Locales_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _localeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLocalesToExcel(GetAllLocalesForExcelInput input)
        {

            var filteredLocales = _localeRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LanguageCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LanguageCodeFilter), e => e.LanguageCode == input.LanguageCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCountryCodeFilter), e => e.CountryFk != null && e.CountryFk.CountryCode == input.CountryCountryCodeFilter);

            var query = (from o in filteredLocales
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetLocaleForViewDto()
                         {
                             Locale = new LocaleDto
                             {
                                 LanguageCode = o.LanguageCode,
                                 Description = o.Description,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             },
                             CountryCountryCode = s1 == null || s1.CountryCode == null ? "" : s1.CountryCode.ToString()
                         });


            var localeListDtos = await query.ToListAsync();

            return _localesExcelExporter.ExportToFile(localeListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_Locales)]
        public async Task<List<LocaleCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new LocaleCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country == null || country.CountryCode == null ? "" : country.CountryCode.ToString()
                }).ToListAsync();
        }

        public async Task<List<GetLocaleForViewDto>> GetAllLocales()
        {
            var allLocales = _localeRepository.GetAll().Include(e => e.CountryFk);

            var locales = from o in allLocales
                          join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty()
                          orderby o.Id
                          select new GetLocaleForViewDto()
                          {
                              Locale = new LocaleDto
                              {
                                  LanguageCode = o.LanguageCode,
                                  Description = o.Description,
                                  IsActive = o.IsActive,
                                  Id = o.Id
                              },
                              CountryCountryCode = s1 == null || s1.CountryCode == null ? "" : s1.CountryCode.ToString()
                          };

            return await locales.ToListAsync();
        }

        public async Task<List<GetLocaleForViewDto>> GetAllLocalesOnCompanyLevel()
        {
            var localeIds = (from o in _formLocaleRepository.GetAll()
                             join o1 in _formRepository.GetAll() on o.FormId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()
                             where s1.SystemLevelId == 1
                             select o.LocaleId).Distinct().ToList();

            var allLocales = _localeRepository.GetAll().Include(e => e.CountryFk);

            var locales = from o in allLocales
                          join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty()
                          where localeIds.Contains(o.Id)
                          orderby o.Id
                          select new GetLocaleForViewDto()
                          {
                              Locale = new LocaleDto
                              {
                                  LanguageCode = o.LanguageCode,
                                  Description = o.Description,
                                  IsActive = o.IsActive,
                                  Id = o.Id
                              },
                              CountryCountryCode = s1 == null || s1.CountryCode == null ? "" : s1.CountryCode.ToString()
                          };

            return await locales.ToListAsync();
        }
    }
}