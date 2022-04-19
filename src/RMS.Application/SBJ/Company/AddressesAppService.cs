using RMS.SBJ.CodeTypeTables;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.Company.Exporting;
using RMS.SBJ.Company.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.Company
{
	[AbpAuthorize(AppPermissions.Pages_Addresses)]
    public class AddressesAppService : RMSAppServiceBase, IAddressesAppService
    {
		 private readonly IRepository<Address, long> _addressRepository;
		 private readonly IAddressesExcelExporter _addressesExcelExporter;
		 private readonly IRepository<Country,long> _lookup_countryRepository;
		 

		  public AddressesAppService(IRepository<Address, long> addressRepository, IAddressesExcelExporter addressesExcelExporter , IRepository<Country, long> lookup_countryRepository) 
		  {
			_addressRepository = addressRepository;
			_addressesExcelExporter = addressesExcelExporter;
			_lookup_countryRepository = lookup_countryRepository;
		
		  }

		 public async Task<PagedResultDto<GetAddressForViewDto>> GetAll(GetAllAddressesInput input)
         {
			
			var filteredAddresses = _addressRepository.GetAll()
						.Include( e => e.CountryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AddressLine1.Contains(input.Filter) || e.AddressLine2.Contains(input.Filter) || e.PostalCode.Contains(input.Filter) || e.City.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.AddressLine1Filter),  e => e.AddressLine1 == input.AddressLine1Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AddressLine2Filter),  e => e.AddressLine2 == input.AddressLine2Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PostalCodeFilter),  e => e.PostalCode == input.PostalCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter),  e => e.City == input.CityFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CountryCountryCodeFilter), e => e.CountryFk != null && e.CountryFk.CountryCode == input.CountryCountryCodeFilter);

			var pagedAndFilteredAddresses = filteredAddresses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var addresses = from o in pagedAndFilteredAddresses
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetAddressForViewDto() {
							Address = new AddressDto
							{
                                AddressLine1 = o.AddressLine1,
                                AddressLine2 = o.AddressLine2,
                                PostalCode = o.PostalCode,
                                City = o.City,
                                Id = o.Id
							},
                         	CountryCountryCode = s1 == null ? "" : s1.CountryCode.ToString()
						};

            var totalCount = await filteredAddresses.CountAsync();

            return new PagedResultDto<GetAddressForViewDto>(
                totalCount,
                await addresses.ToListAsync()
            );
         }
		 
		 public async Task<GetAddressForViewDto> GetAddressForView(long id)
         {
            var address = await _addressRepository.GetAsync(id);

            var output = new GetAddressForViewDto { Address = ObjectMapper.Map<AddressDto>(address) };

		    if (output.Address.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Address.CountryId);
                output.CountryCountryCode = _lookupCountry.CountryCode.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Addresses_Edit)]
		 public async Task<GetAddressForEditOutput> GetAddressForEdit(EntityDto<long> input)
         {
            var address = await _addressRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetAddressForEditOutput {Address = ObjectMapper.Map<CreateOrEditAddressDto>(address)};

		    if (output.Address.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Address.CountryId);
                output.CountryCountryCode = _lookupCountry.CountryCode.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAddressDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Addresses_Create)]
		 protected virtual async Task Create(CreateOrEditAddressDto input)
         {
            var address = ObjectMapper.Map<Address>(input);

			
			if (AbpSession.TenantId != null)
			{
				address.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _addressRepository.InsertAsync(address);
         }

		 [AbpAuthorize(AppPermissions.Pages_Addresses_Edit)]
		 protected virtual async Task Update(CreateOrEditAddressDto input)
         {
            var address = await _addressRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, address);
         }

		 [AbpAuthorize(AppPermissions.Pages_Addresses_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _addressRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetAddressesToExcel(GetAllAddressesForExcelInput input)
         {
			
			var filteredAddresses = _addressRepository.GetAll()
						.Include( e => e.CountryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AddressLine1.Contains(input.Filter) || e.AddressLine2.Contains(input.Filter) || e.PostalCode.Contains(input.Filter) || e.City.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.AddressLine1Filter),  e => e.AddressLine1 == input.AddressLine1Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AddressLine2Filter),  e => e.AddressLine2 == input.AddressLine2Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PostalCodeFilter),  e => e.PostalCode == input.PostalCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter),  e => e.City == input.CityFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CountryCountryCodeFilter), e => e.CountryFk != null && e.CountryFk.CountryCode == input.CountryCountryCodeFilter);

			var query = (from o in filteredAddresses
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetAddressForViewDto() { 
							Address = new AddressDto
							{
                                AddressLine1 = o.AddressLine1,
                                AddressLine2 = o.AddressLine2,
                                PostalCode = o.PostalCode,
                                City = o.City,
                                Id = o.Id
							},
                         	CountryCountryCode = s1 == null ? "" : s1.CountryCode.ToString()
						 });


            var addressListDtos = await query.ToListAsync();

            return _addressesExcelExporter.ExportToFile(addressListDtos);
         }


    }
}