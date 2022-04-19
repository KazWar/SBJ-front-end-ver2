using RMS.SBJ.Retailers;
using RMS.SBJ.Company;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.RetailerLocations.Exporting;
using RMS.SBJ.RetailerLocations.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.RetailerLocations
{
    [AbpAuthorize(AppPermissions.Pages_RetailerLocations)]
    public class RetailerLocationsAppService : RMSAppServiceBase, IRetailerLocationsAppService
    {
        private readonly IRepository<RetailerLocation, long> _retailerLocationRepository;
        private readonly IRetailerLocationsExcelExporter _retailerLocationsExcelExporter;
        private readonly IRepository<Retailer, long> _lookup_retailerRepository;
        private readonly IRepository<Address, long> _lookup_addressRepository;


        public RetailerLocationsAppService(IRepository<RetailerLocation, long> retailerLocationRepository, IRetailerLocationsExcelExporter retailerLocationsExcelExporter, IRepository<Retailer, long> lookup_retailerRepository, IRepository<Address, long> lookup_addressRepository)
        {
            _retailerLocationRepository = retailerLocationRepository;
            _retailerLocationsExcelExporter = retailerLocationsExcelExporter;
            _lookup_retailerRepository = lookup_retailerRepository;
            _lookup_addressRepository = lookup_addressRepository;

        }

        public async Task<PagedResultDto<GetRetailerLocationForViewDto>> GetAll(GetAllRetailerLocationsInput input)
        {

            var filteredRetailerLocations = _retailerLocationRepository.GetAll()
                        .Include(e => e.RetailerFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RetailerNameFilter), e => e.RetailerFk != null && e.RetailerFk.Name == input.RetailerNameFilter);

            var pagedAndFilteredRetailerLocations = filteredRetailerLocations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var retailerLocations = from o in pagedAndFilteredRetailerLocations
                                    join o1 in _lookup_retailerRepository.GetAll() on o.RetailerId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    select new GetRetailerLocationForViewDto()
                                    {
                                        RetailerLocation = new RetailerLocationDto
                                        {
                                            Name = o.Name,
                                            Id = o.Id
                                        },
                                        RetailerName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                    };

            var totalCount = await filteredRetailerLocations.CountAsync();

            return new PagedResultDto<GetRetailerLocationForViewDto>(
                totalCount,
                await retailerLocations.ToListAsync()
            );
        }

        public async Task<GetRetailerLocationForViewDto> GetRetailerLocationForView(long id)
        {
            var retailerLocation = await _retailerLocationRepository.GetAsync(id);

            var output = new GetRetailerLocationForViewDto { RetailerLocation = ObjectMapper.Map<RetailerLocationDto>(retailerLocation) };

            if (output.RetailerLocation.RetailerId != null)
            {
                var _lookupRetailer = await _lookup_retailerRepository.FirstOrDefaultAsync((long)output.RetailerLocation.RetailerId);
                output.RetailerName = _lookupRetailer?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_RetailerLocations_Edit)]
        public async Task<GetRetailerLocationForEditOutput> GetRetailerLocationForEdit(EntityDto<long> input)
        {
            var retailerLocation = await _retailerLocationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRetailerLocationForEditOutput { RetailerLocation = ObjectMapper.Map<CreateOrEditRetailerLocationDto>(retailerLocation) };

            if (output.RetailerLocation.RetailerId != null)
            {
                var _lookupRetailer = await _lookup_retailerRepository.FirstOrDefaultAsync((long)output.RetailerLocation.RetailerId);
                output.RetailerName = _lookupRetailer?.Name?.ToString();
            }

            if (output.RetailerLocation.AddressId != null)
            {
                var _lookupAddress = await _lookup_addressRepository.FirstOrDefaultAsync((long)output.RetailerLocation.AddressId);
                output.AddressAddressLine1 = _lookupAddress?.AddressLine1?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRetailerLocationDto input)
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

        [AbpAuthorize(AppPermissions.Pages_RetailerLocations_Create)]
        protected virtual async Task Create(CreateOrEditRetailerLocationDto input)
        {
            var retailerLocation = ObjectMapper.Map<RetailerLocation>(input);


            if (AbpSession.TenantId != null)
            {
                retailerLocation.TenantId = (int?)AbpSession.TenantId;
            }


            await _retailerLocationRepository.InsertAsync(retailerLocation);
        }

        [AbpAuthorize(AppPermissions.Pages_RetailerLocations_Edit)]
        protected virtual async Task Update(CreateOrEditRetailerLocationDto input)
        {
            var retailerLocation = await _retailerLocationRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, retailerLocation);
        }

        [AbpAuthorize(AppPermissions.Pages_RetailerLocations_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _retailerLocationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRetailerLocationsToExcel(GetAllRetailerLocationsForExcelInput input)
        {

            var filteredRetailerLocations = _retailerLocationRepository.GetAll()
                        .Include(e => e.RetailerFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RetailerNameFilter), e => e.RetailerFk != null && e.RetailerFk.Name == input.RetailerNameFilter);

            var query = (from o in filteredRetailerLocations
                         join o1 in _lookup_retailerRepository.GetAll() on o.RetailerId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetRetailerLocationForViewDto()
                         {
                             RetailerLocation = new RetailerLocationDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             },
                             RetailerName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });


            var retailerLocationListDtos = await query.ToListAsync();

            return _retailerLocationsExcelExporter.ExportToFile(retailerLocationListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_RetailerLocations)]
        public async Task<PagedResultDto<RetailerLocationRetailerLookupTableDto>> GetAllRetailerForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_retailerRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var retailerList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RetailerLocationRetailerLookupTableDto>();
            foreach (var retailer in retailerList)
            {
                lookupTableDtoList.Add(new RetailerLocationRetailerLookupTableDto
                {
                    Id = retailer.Id,
                    DisplayName = retailer.Name?.ToString()
                });
            }

            return new PagedResultDto<RetailerLocationRetailerLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_RetailerLocations)]
        public async Task<PagedResultDto<RetailerLocationAddressLookupTableDto>> GetAllAddressForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_addressRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.AddressLine1 != null && e.AddressLine1.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var addressList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RetailerLocationAddressLookupTableDto>();
            foreach (var address in addressList)
            {
                lookupTableDtoList.Add(new RetailerLocationAddressLookupTableDto
                {
                    Id = address.Id,
                    DisplayName = address.AddressLine1?.ToString()
                });
            }

            return new PagedResultDto<RetailerLocationAddressLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}