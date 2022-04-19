using RMS.SBJ.CodeTypeTables;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.PromoPlanner.Exporting;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.PromoPlanner
{
    [AbpAuthorize(AppPermissions.Pages_PromoCountries)]
    public class PromoCountriesAppService : RMSAppServiceBase, IPromoCountriesAppService
    {
        private readonly IRepository<PromoCountry, long> _promoCountryRepository;
        private readonly IPromoCountriesExcelExporter _promoCountriesExcelExporter;
        private readonly IRepository<Promo, long> _lookup_promoRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;


        public PromoCountriesAppService(IRepository<PromoCountry, long> promoCountryRepository, IPromoCountriesExcelExporter promoCountriesExcelExporter, IRepository<Promo, long> lookup_promoRepository, IRepository<Country, long> lookup_countryRepository)
        {
            _promoCountryRepository = promoCountryRepository;
            _promoCountriesExcelExporter = promoCountriesExcelExporter;
            _lookup_promoRepository = lookup_promoRepository;
            _lookup_countryRepository = lookup_countryRepository;

        }

        public async Task<PagedResultDto<GetPromoCountryForViewDto>> GetAll(GetAllPromoCountriesInput input)
        {

            var filteredPromoCountries = _promoCountryRepository.GetAll()
                        .Include(e => e.PromoFk)
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoPromocodeFilter), e => e.PromoFk != null && e.PromoFk.Promocode == input.PromoPromocodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCountryCodeFilter), e => e.CountryFk != null && e.CountryFk.CountryCode == input.CountryCountryCodeFilter);

            var pagedAndFilteredPromoCountries = filteredPromoCountries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var promoCountries = from o in pagedAndFilteredPromoCountries
                                 join o1 in _lookup_promoRepository.GetAll() on o.PromoId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 select new GetPromoCountryForViewDto()
                                 {
                                     PromoCountry = new PromoCountryDto
                                     {
                                         Id = o.Id
                                     },
                                     PromoPromocode = s1 == null ? "" : s1.Promocode.ToString(),
                                     CountryCountryCode = s2 == null ? "" : s2.CountryCode.ToString()
                                 };

            var totalCount = await filteredPromoCountries.CountAsync();

            return new PagedResultDto<GetPromoCountryForViewDto>(
                totalCount,
                await promoCountries.ToListAsync()
            );
        }

        public async Task<PagedResultDto<CustomPromoCountryForView>> GetAllCountriesForPromo(GetAllCountriesForPromoInput input)
        {
            var filteredPromoCountries =
                _promoCountryRepository.GetAll()
                .Include(e => e.PromoFk)
                .Include(e => e.CountryFk)
                .Where(country => country.PromoId == input.PromoId)
                .OrderBy("id asc")
                .PageBy(input);

            var listing = from o in filteredPromoCountries
                          select new CustomPromoCountryForView()
                          {
                              Id = o.Id,
                              CountryId = o.CountryId,
                              CountryCode = o.CountryFk.CountryCode,
                              CountryName = o.CountryFk.Description
                          };

            var totalCount = await listing.CountAsync();

            return new PagedResultDto<CustomPromoCountryForView>(
                totalCount,
                await listing.OrderBy(x => x.CountryName).ToListAsync());
        }

        public async Task<GetPromoCountryForViewDto> GetPromoCountryForView(long id)
        {
            var promoCountry = await _promoCountryRepository.GetAsync(id);

            var output = new GetPromoCountryForViewDto { PromoCountry = ObjectMapper.Map<PromoCountryDto>(promoCountry) };

            //if (output.PromoCountry.PromoId != null)
            //{
                var _lookupPromo = await _lookup_promoRepository.FirstOrDefaultAsync((long)output.PromoCountry.PromoId);
                output.PromoPromocode = _lookupPromo.Promocode.ToString();
            //}

            //if (output.PromoCountry.CountryId != null)
            //{
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.PromoCountry.CountryId);
                output.CountryCountryCode = _lookupCountry.CountryCode.ToString();
            //}

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PromoCountries_Edit)]
        public async Task<GetPromoCountryForEditOutput> GetPromoCountryForEdit(EntityDto<long> input)
        {
            var promoCountry = await _promoCountryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPromoCountryForEditOutput { PromoCountry = ObjectMapper.Map<CreateOrEditPromoCountryDto>(promoCountry) };

            //if (output.PromoCountry.PromoId != null)
            //{
                var _lookupPromo = await _lookup_promoRepository.FirstOrDefaultAsync((long)output.PromoCountry.PromoId);
                output.PromoPromocode = _lookupPromo.Promocode.ToString();
            //}

            //if (output.PromoCountry.CountryId != null)
            //{
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.PromoCountry.CountryId);
                output.CountryCountryCode = _lookupCountry.CountryCode.ToString();
            //}

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPromoCountryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PromoCountries_Create)]
        protected virtual async Task Create(CreateOrEditPromoCountryDto input)
        {
            var promoCountry = ObjectMapper.Map<PromoCountry>(input);


            if (AbpSession.TenantId != null)
            {
                promoCountry.TenantId = (int?)AbpSession.TenantId;
            }


            await _promoCountryRepository.InsertAsync(promoCountry);
        }

        [AbpAuthorize(AppPermissions.Pages_PromoCountries_Edit)]
        protected virtual async Task Update(CreateOrEditPromoCountryDto input)
        {
            var promoCountry = await _promoCountryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, promoCountry);
        }

        [AbpAuthorize(AppPermissions.Pages_PromoCountries_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _promoCountryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPromoCountriesToExcel(GetAllPromoCountriesForExcelInput input)
        {

            var filteredPromoCountries = _promoCountryRepository.GetAll()
                        .Include(e => e.PromoFk)
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoPromocodeFilter), e => e.PromoFk != null && e.PromoFk.Promocode == input.PromoPromocodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCountryCodeFilter), e => e.CountryFk != null && e.CountryFk.CountryCode == input.CountryCountryCodeFilter);

            var query = (from o in filteredPromoCountries
                         join o1 in _lookup_promoRepository.GetAll() on o.PromoId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetPromoCountryForViewDto()
                         {
                             PromoCountry = new PromoCountryDto
                             {
                                 Id = o.Id
                             },
                             PromoPromocode = s1 == null ? "" : s1.Promocode.ToString(),
                             CountryCountryCode = s2 == null ? "" : s2.CountryCode.ToString()
                         });


            var promoCountryListDtos = await query.ToListAsync();

            return _promoCountriesExcelExporter.ExportToFile(promoCountryListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_PromoCountries)]
        public async Task<PagedResultDto<PromoCountryPromoLookupTableDto>> GetAllPromoForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_promoRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Promocode != null && e.Promocode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var promoList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoCountryPromoLookupTableDto>();
            foreach (var promo in promoList)
            {
                lookupTableDtoList.Add(new PromoCountryPromoLookupTableDto
                {
                    Id = promo.Id,
                    DisplayName = promo.Promocode?.ToString()
                });
            }

            return new PagedResultDto<PromoCountryPromoLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PromoCountries)]
        public async Task<PagedResultDto<PromoCountryCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_countryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CountryCode != null && e.CountryCode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var countryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoCountryCountryLookupTableDto>();
            foreach (var country in countryList)
            {
                lookupTableDtoList.Add(new PromoCountryCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country.CountryCode?.ToString()
                });
            }

            return new PagedResultDto<PromoCountryCountryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PromoCountries)]
        public async Task<PagedResultDto<PromoCountryCountryLookupTableDto>> GetAvailableCountryForLookupTable(GetAvailableForLookupTableInput input)
        {
            //var occupiedCountriesById = _promoCountryRepository.GetAll().Where(e => e.PromoId == input.FilterId.Value).Select(e => e.CountryId);
            var occupiedCountriesByEx = !String.IsNullOrEmpty(input.FilterEx) && !String.IsNullOrWhiteSpace(input.FilterEx) ? input.FilterEx.Split("|") : new string[] { };
            //var occupiedCountriesByExCode = new List<string>();
            var occupiedCountriesByExId = new List<long>();

            foreach (var country in occupiedCountriesByEx)
            {
                //var countryParts = country.Split('(');
                //var countryCode = countryParts.Last().Substring(0, countryParts.Last().Length - 1);

                //occupiedCountriesByExCode.Add(countryCode);
                occupiedCountriesByExId.Add(Convert.ToInt64(country));
            }

            //var query = _lookup_countryRepository.GetAll().WhereIf(
            //       !string.IsNullOrWhiteSpace(input.Filter),
            //      e => e.CountryCode != null && e.CountryCode.Contains(input.Filter)
            //   ).WhereIf(occupiedCountriesByExCode.Count > 0,
            //      e => e.CountryCode != null && !occupiedCountriesByExCode.Contains(e.CountryCode)
            //   ).OrderBy(e => e.Description);    //.Where(e => !occupiedCountriesById.Contains(e.Id));

            var query = _lookup_countryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CountryCode != null && e.CountryCode.Contains(input.Filter)
               ).WhereIf(occupiedCountriesByExId.Count > 0,
                  e => !occupiedCountriesByExId.Contains(e.Id)
               ).OrderBy(e => e.Description);    //.Where(e => !occupiedCountriesById.Contains(e.Id));

            var totalCount = await query.CountAsync();

            var countryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoCountryCountryLookupTableDto>();

            foreach (var country in countryList)
            {
                lookupTableDtoList.Add(new PromoCountryCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = String.Format("{0} ({1})", country.Description?.ToString(), country.CountryCode?.ToString())
                });
            }

            return new PagedResultDto<PromoCountryCountryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}