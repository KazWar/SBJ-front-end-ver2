using RMS.SBJ.Company;


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
	[AbpAuthorize(AppPermissions.Pages_Companies)]
    public class CompaniesAppService : RMSAppServiceBase, ICompaniesAppService
    {
		 private readonly IRepository<Company, long> _companyRepository;
		 private readonly ICompaniesExcelExporter _companiesExcelExporter;
		 private readonly IRepository<Address,long> _lookup_addressRepository;
		 

		  public CompaniesAppService(IRepository<Company, long> companyRepository, ICompaniesExcelExporter companiesExcelExporter , IRepository<Address, long> lookup_addressRepository) 
		  {
			_companyRepository = companyRepository;
			_companiesExcelExporter = companiesExcelExporter;
			_lookup_addressRepository = lookup_addressRepository;
		
		  }

		 public async Task<PagedResultDto<GetCompanyForViewDto>> GetAll(GetAllCompaniesInput input)
         {
			
			var filteredCompanies = _companyRepository.GetAll()
						.Include( e => e.AddressFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter) || e.EmailAddress.Contains(input.Filter) || e.BicCashBack.Contains(input.Filter) || e.IbanCashBack.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name.ToLower() == input.NameFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNumberFilter),  e => e.PhoneNumber.ToLower() == input.PhoneNumberFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailAddressFilter),  e => e.EmailAddress.ToLower() == input.EmailAddressFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.BICCashBackFilter),  e => e.BicCashBack.ToLower() == input.BICCashBackFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.IBANCashBackFilter),  e => e.IbanCashBack.ToLower() == input.IBANCashBackFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.AddressPostalCodeFilter), e => e.AddressFk != null && e.AddressFk.PostalCode.ToLower() == input.AddressPostalCodeFilter.ToLower().Trim());

			var pagedAndFilteredCompanies = filteredCompanies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var companies = from o in pagedAndFilteredCompanies
                         join o1 in _lookup_addressRepository.GetAll() on o.AddressId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetCompanyForViewDto() {
							Company = new CompanyDto
							{
                                Name = o.Name,
                                PhoneNumber = o.PhoneNumber,
                                EmailAddress = o.EmailAddress,
                                BICCashBack = o.BicCashBack,
                                IBANCashBack = o.IbanCashBack,
                                Id = o.Id
							},
                         	AddressPostalCode = s1 == null ? "" : s1.PostalCode.ToString()
						};

            var totalCount = await filteredCompanies.CountAsync();

            return new PagedResultDto<GetCompanyForViewDto>(
                totalCount,
                await companies.ToListAsync()
            );
         }
		 
		 public async Task<GetCompanyForViewDto> GetCompanyForView(long id)
         {
            var company = await _companyRepository.GetAsync(id);

            var output = new GetCompanyForViewDto { Company = ObjectMapper.Map<CompanyDto>(company) };

		    if (output.Company.AddressId != null)
            {
                var _lookupAddress = await _lookup_addressRepository.FirstOrDefaultAsync((long)output.Company.AddressId);
                output.AddressPostalCode = _lookupAddress.PostalCode.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Companies_Edit)]
		 public async Task<GetCompanyForEditOutput> GetCompanyForEdit(EntityDto<long> input)
         {
            var company = await _companyRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCompanyForEditOutput {Company = ObjectMapper.Map<CreateOrEditCompanyDto>(company)};

		    if (output.Company.AddressId != null)
            {
                var _lookupAddress = await _lookup_addressRepository.FirstOrDefaultAsync((long)output.Company.AddressId);
                output.AddressPostalCode = _lookupAddress.PostalCode.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCompanyDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Companies_Create)]
		 protected virtual async Task Create(CreateOrEditCompanyDto input)
         {
            var company = ObjectMapper.Map<Company>(input);

			
			if (AbpSession.TenantId != null)
			{
				company.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _companyRepository.InsertAsync(company);
         }

		 [AbpAuthorize(AppPermissions.Pages_Companies_Edit)]
		 protected virtual async Task Update(CreateOrEditCompanyDto input)
         {
            var company = await _companyRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, company);
         }

		 [AbpAuthorize(AppPermissions.Pages_Companies_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _companyRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCompaniesToExcel(GetAllCompaniesForExcelInput input)
         {
			
			var filteredCompanies = _companyRepository.GetAll()
						.Include( e => e.AddressFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter) || e.EmailAddress.Contains(input.Filter) || e.BicCashBack.Contains(input.Filter) || e.IbanCashBack.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name.ToLower() == input.NameFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNumberFilter),  e => e.PhoneNumber.ToLower() == input.PhoneNumberFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailAddressFilter),  e => e.EmailAddress.ToLower() == input.EmailAddressFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.BICCashBackFilter),  e => e.BicCashBack.ToLower() == input.BICCashBackFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.IBANCashBackFilter),  e => e.IbanCashBack.ToLower() == input.IBANCashBackFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.AddressPostalCodeFilter), e => e.AddressFk != null && e.AddressFk.PostalCode.ToLower() == input.AddressPostalCodeFilter.ToLower().Trim());

			var query = (from o in filteredCompanies
                         join o1 in _lookup_addressRepository.GetAll() on o.AddressId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetCompanyForViewDto() { 
							Company = new CompanyDto
							{
                                Name = o.Name,
                                PhoneNumber = o.PhoneNumber,
                                EmailAddress = o.EmailAddress,
                                BICCashBack = o.BicCashBack,
                                IBANCashBack = o.IbanCashBack,
                                Id = o.Id
							},
                         	AddressPostalCode = s1 == null ? "" : s1.PostalCode.ToString()
						 });


            var companyListDtos = await query.ToListAsync();

            return _companiesExcelExporter.ExportToFile(companyListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Companies)]
         public async Task<PagedResultDto<CompanyAddressLookupTableDto>> GetAllAddressForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_addressRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.PostalCode.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var addressList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<CompanyAddressLookupTableDto>();
			foreach(var address in addressList){
				lookupTableDtoList.Add(new CompanyAddressLookupTableDto
				{
					Id = address.Id,
					DisplayName = address.PostalCode?.ToString()
				});
			}

            return new PagedResultDto<CompanyAddressLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}