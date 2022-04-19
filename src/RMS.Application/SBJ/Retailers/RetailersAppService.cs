using RMS.SBJ.CodeTypeTables;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.Retailers.Exporting;
using RMS.SBJ.Retailers.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.CampaignRetailerLocations;
using RMS.SBJ.RetailerLocations;
using RMS.SBJ.RetailerLocations.Dtos;

namespace RMS.SBJ.Retailers
{
    [AbpAuthorize(AppPermissions.Pages_Retailers)]
    public class RetailersAppService : RMSAppServiceBase, IRetailersAppService
    {
        private readonly IRetailersExcelExporter _retailersExcelExporter;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<CampaignRetailerLocation, long> _lookup_campaignRetailerLocationRepository;
        private readonly IRepository<RetailerLocation, long> _lookup_retailerLocationRepository;
        private readonly IRepository<Retailer, long> _retailerRepository;

        public RetailersAppService(
            IRetailersExcelExporter retailersExcelExporter,
            IRepository<Country, long> lookup_countryRepository,
            IRepository<CampaignRetailerLocation, long> lookup_campaignRetailerLocationRepository,
            IRepository<RetailerLocation, long> lookup_retailerLocationRepository,
            IRepository<Retailer, long> retailerRepository)
        {
            _retailersExcelExporter = retailersExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_campaignRetailerLocationRepository = lookup_campaignRetailerLocationRepository;
            _lookup_retailerLocationRepository = lookup_retailerLocationRepository;
            _retailerRepository = retailerRepository;
        }

        public async Task<PagedResultDto<GetRetailerForViewDto>> GetAll(GetAllRetailersInput input)
        {
            var filteredRetailers = _retailerRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCountryCodeFilter), e => e.CountryFk != null && e.CountryFk.CountryCode == input.CountryCountryCodeFilter);

            var pagedAndFilteredRetailers = filteredRetailers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var retailers = from o in pagedAndFilteredRetailers
                            join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            select new GetRetailerForViewDto()
                            {
                                Retailer = new RetailerDto
                                {
                                    Name = o.Name,
                                    Code = o.Code,
                                    Id = o.Id
                                },
                                CountryCountryCode = s1 == null || s1.CountryCode == null ? "" : s1.CountryCode.ToString()
                            };

            var totalCount = await filteredRetailers.CountAsync();

            return new PagedResultDto<GetRetailerForViewDto>(
                totalCount,
                await retailers.ToListAsync()
            );
        }

        public async Task<IEnumerable<GetRetailerForViewDto>> GetAllWithoutPaging()
        {
            var retailers = _retailerRepository.GetAll().OrderBy(x => x.Name)
                        .Select(x => new GetRetailerForViewDto
                        {
                            Retailer = new RetailerDto { Id = x.Id, Name = x.Name }
                        });

            return await retailers.ToListAsync();
        }

        public async Task<PagedResultDto<GetRetailerForCampaignViewDto>> GetAllRetailersForCampaign(long campaignId)
        {
            var campaignRetailerLocations = await _lookup_campaignRetailerLocationRepository.GetAll()
                .Where(x => x.CampaignId == campaignId)
                .Include(x => x.RetailerLocationFk)
                .ThenInclude(x => x.RetailerFk)
                .ToListAsync();

            if (!campaignRetailerLocations.Any())
            {
                Logger.Error($"Error in {nameof(GetAllRetailersForCampaign)}: could not find CampaignRetailerLocation records with Campaign ID {campaignId}.");

                return null;
            }

            var totalCount = campaignRetailerLocations.Count();
            var items = campaignRetailerLocations.Select(q => new GetRetailerForCampaignViewDto
            {
                Retailer = ObjectMapper.Map<RetailerDto>(q.RetailerLocationFk.RetailerFk),
                RetailerLocation = ObjectMapper.Map<RetailerLocationDto>(q.RetailerLocationFk)
            }).ToList();

            return new PagedResultDto<GetRetailerForCampaignViewDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<GetRetailerForViewDto> GetRetailerForView(long id)
        {
            var retailer = await _retailerRepository.GetAsync(id);

            var output = new GetRetailerForViewDto { Retailer = ObjectMapper.Map<RetailerDto>(retailer) };

            if (output.Retailer.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Retailer.CountryId);
                output.CountryCountryCode = _lookupCountry?.CountryCode?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Retailers_Edit)]
        public async Task<GetRetailerForEditOutput> GetRetailerForEdit(EntityDto<long> input)
        {
            var retailer = await _retailerRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRetailerForEditOutput { Retailer = ObjectMapper.Map<CreateOrEditRetailerDto>(retailer) };

            if (output.Retailer.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Retailer.CountryId);
                output.CountryCountryCode = _lookupCountry?.CountryCode?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRetailerDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Retailers_Create)]
        protected virtual async Task Create(CreateOrEditRetailerDto input)
        {
            var retailer = ObjectMapper.Map<Retailer>(input);

            if (AbpSession.TenantId != null)
            {
                retailer.TenantId = AbpSession.TenantId;
            }

            await _retailerRepository.InsertAsync(retailer);
        }

        [AbpAuthorize(AppPermissions.Pages_Retailers_Edit)]
        protected virtual async Task Update(CreateOrEditRetailerDto input)
        {
            var retailer = await _retailerRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, retailer);
        }

        [AbpAuthorize(AppPermissions.Pages_Retailers_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _retailerRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRetailersToExcel(GetAllRetailersForExcelInput input)
        {

            var filteredRetailers = _retailerRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCountryCodeFilter), e => e.CountryFk != null && e.CountryFk.CountryCode == input.CountryCountryCodeFilter);

            var query = (from o in filteredRetailers
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetRetailerForViewDto()
                         {
                             Retailer = new RetailerDto
                             {
                                 Name = o.Name,
                                 Code = o.Code,
                                 Id = o.Id
                             },
                             CountryCountryCode = s1 == null || s1.CountryCode == null ? "" : s1.CountryCode.ToString()
                         });

            var retailerListDtos = await query.ToListAsync();

            return _retailersExcelExporter.ExportToFile(retailerListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Retailers)]
        public async Task<PagedResultDto<RetailerCountryLookupTableDto>> GetAllCountryForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            var query = _lookup_countryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CountryCode != null && e.CountryCode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var countryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RetailerCountryLookupTableDto>();
            foreach (var country in countryList)
            {
                lookupTableDtoList.Add(new RetailerCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country.CountryCode?.ToString()
                });
            }

            return new PagedResultDto<RetailerCountryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}