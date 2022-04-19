using RMS.SBJ.Registrations;
using RMS.SBJ.Products;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.RetailerLocations;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.PurchaseRegistrations.Exporting;
using RMS.SBJ.PurchaseRegistrations.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.PurchaseRegistrations
{
	[AbpAuthorize(AppPermissions.Pages_PurchaseRegistrations)]
    public class PurchaseRegistrationsAppService : RMSAppServiceBase, IPurchaseRegistrationsAppService
    {
		 private readonly IRepository<PurchaseRegistration, long> _purchaseRegistrationRepository;
		 private readonly IPurchaseRegistrationsExcelExporter _purchaseRegistrationsExcelExporter;
		 private readonly IRepository<Registration,long> _lookup_registrationRepository;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 private readonly IRepository<HandlingLine,long> _lookup_handlingLineRepository;
		 private readonly IRepository<RetailerLocation,long> _lookup_retailerLocationRepository;
		 

		  public PurchaseRegistrationsAppService(IRepository<PurchaseRegistration, long> purchaseRegistrationRepository, IPurchaseRegistrationsExcelExporter purchaseRegistrationsExcelExporter , IRepository<Registration, long> lookup_registrationRepository, IRepository<Product, long> lookup_productRepository, IRepository<HandlingLine, long> lookup_handlingLineRepository, IRepository<RetailerLocation, long> lookup_retailerLocationRepository) 
		  {
			_purchaseRegistrationRepository = purchaseRegistrationRepository;
			_purchaseRegistrationsExcelExporter = purchaseRegistrationsExcelExporter;
			_lookup_registrationRepository = lookup_registrationRepository;
		_lookup_productRepository = lookup_productRepository;
		_lookup_handlingLineRepository = lookup_handlingLineRepository;
		_lookup_retailerLocationRepository = lookup_retailerLocationRepository;
		
		  }

		 public async Task<PagedResultDto<GetPurchaseRegistrationForViewDto>> GetAll(GetAllPurchaseRegistrationsInput input)
         {
			
			var filteredPurchaseRegistrations = _purchaseRegistrationRepository.GetAll()
						.Include( e => e.RegistrationFk)
						.Include( e => e.ProductFk)
						.Include( e => e.HandlingLineFk)
						.Include( e => e.RetailerLocationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.InvoiceImage.Contains(input.Filter))
						.WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
						.WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
						.WhereIf(input.MinTotalAmountFilter != null, e => e.TotalAmount >= input.MinTotalAmountFilter)
						.WhereIf(input.MaxTotalAmountFilter != null, e => e.TotalAmount <= input.MaxTotalAmountFilter)
						.WhereIf(input.MinPurchaseDateFilter != null, e => e.PurchaseDate >= input.MinPurchaseDateFilter)
						.WhereIf(input.MaxPurchaseDateFilter != null, e => e.PurchaseDate <= input.MaxPurchaseDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceImageFilter),  e => e.InvoiceImage == input.InvoiceImageFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationFirstNameFilter), e => e.RegistrationFk != null && e.RegistrationFk.FirstName == input.RegistrationFirstNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductCtnFilter), e => e.ProductFk != null && e.ProductFk.ProductCode == input.ProductCtnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.HandlingLineCustomerCodeFilter), e => e.HandlingLineFk != null && e.HandlingLineFk.CustomerCode == input.HandlingLineCustomerCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RetailerLocationNameFilter), e => e.RetailerLocationFk != null && e.RetailerLocationFk.Name == input.RetailerLocationNameFilter);

			var pagedAndFilteredPurchaseRegistrations = filteredPurchaseRegistrations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var purchaseRegistrations = from o in pagedAndFilteredPurchaseRegistrations
                         join o1 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         join o4 in _lookup_retailerLocationRepository.GetAll() on o.RetailerLocationId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         
                         select new GetPurchaseRegistrationForViewDto() {
							PurchaseRegistration = new PurchaseRegistrationDto
							{
                                Quantity = o.Quantity,
                                TotalAmount = o.TotalAmount,
                                PurchaseDate = o.PurchaseDate,
                                InvoiceImage = o.InvoiceImage,
                                Id = o.Id
							},
                         	RegistrationFirstName = s1 == null || s1.FirstName == null ? "" : s1.FirstName.ToString(),
                         	ProductCtn = s2 == null || s2.ProductCode == null ? "" : s2.ProductCode.ToString(),
                         	HandlingLineCustomerCode = s3 == null || s3.CustomerCode == null ? "" : s3.CustomerCode.ToString(),
                         	RetailerLocationName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
						};

            var totalCount = await filteredPurchaseRegistrations.CountAsync();

            return new PagedResultDto<GetPurchaseRegistrationForViewDto>(
                totalCount,
                await purchaseRegistrations.ToListAsync()
            );
         }
		 
		 public async Task<GetPurchaseRegistrationForViewDto> GetPurchaseRegistrationForView(long id)
         {
            var purchaseRegistration = await _purchaseRegistrationRepository.GetAsync(id);

            var output = new GetPurchaseRegistrationForViewDto { PurchaseRegistration = ObjectMapper.Map<PurchaseRegistrationDto>(purchaseRegistration) };

		    if (output.PurchaseRegistration.RegistrationId != null)
            {
                var _lookupRegistration = await _lookup_registrationRepository.FirstOrDefaultAsync((long)output.PurchaseRegistration.RegistrationId);
                output.RegistrationFirstName = _lookupRegistration?.FirstName?.ToString();
            }

		    if (output.PurchaseRegistration.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.PurchaseRegistration.ProductId);
                output.ProductCtn = _lookupProduct?.ProductCode?.ToString();
            }

		    if (output.PurchaseRegistration.HandlingLineId != null)
            {
                var _lookupHandlingLine = await _lookup_handlingLineRepository.FirstOrDefaultAsync((long)output.PurchaseRegistration.HandlingLineId);
                output.HandlingLineCustomerCode = _lookupHandlingLine?.CustomerCode?.ToString();
            }

		    if (output.PurchaseRegistration.RetailerLocationId != null)
            {
                var _lookupRetailerLocation = await _lookup_retailerLocationRepository.FirstOrDefaultAsync((long)output.PurchaseRegistration.RetailerLocationId);
                output.RetailerLocationName = _lookupRetailerLocation?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrations_Edit)]
		 public async Task<GetPurchaseRegistrationForEditOutput> GetPurchaseRegistrationForEdit(EntityDto<long> input)
         {
            var purchaseRegistration = await _purchaseRegistrationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPurchaseRegistrationForEditOutput {PurchaseRegistration = ObjectMapper.Map<CreateOrEditPurchaseRegistrationDto>(purchaseRegistration)};

		    if (output.PurchaseRegistration.RegistrationId != null)
            {
                var _lookupRegistration = await _lookup_registrationRepository.FirstOrDefaultAsync((long)output.PurchaseRegistration.RegistrationId);
                output.RegistrationFirstName = _lookupRegistration?.FirstName?.ToString();
            }

		    if (output.PurchaseRegistration.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.PurchaseRegistration.ProductId);
                output.ProductCtn = _lookupProduct?.ProductCode?.ToString();
            }

		    if (output.PurchaseRegistration.HandlingLineId != null)
            {
                var _lookupHandlingLine = await _lookup_handlingLineRepository.FirstOrDefaultAsync((long)output.PurchaseRegistration.HandlingLineId);
                output.HandlingLineCustomerCode = _lookupHandlingLine?.CustomerCode?.ToString();
            }

		    if (output.PurchaseRegistration.RetailerLocationId != null)
            {
                var _lookupRetailerLocation = await _lookup_retailerLocationRepository.FirstOrDefaultAsync((long)output.PurchaseRegistration.RetailerLocationId);
                output.RetailerLocationName = _lookupRetailerLocation?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPurchaseRegistrationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrations_Create)]
		 protected virtual async Task Create(CreateOrEditPurchaseRegistrationDto input)
         {
            var purchaseRegistration = ObjectMapper.Map<PurchaseRegistration>(input);

			
			if (AbpSession.TenantId != null)
			{
				purchaseRegistration.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _purchaseRegistrationRepository.InsertAsync(purchaseRegistration);
         }

		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrations_Edit)]
		 protected virtual async Task Update(CreateOrEditPurchaseRegistrationDto input)
         {
            var purchaseRegistration = await _purchaseRegistrationRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, purchaseRegistration);
         }

		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrations_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _purchaseRegistrationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPurchaseRegistrationsToExcel(GetAllPurchaseRegistrationsForExcelInput input)
         {
			
			var filteredPurchaseRegistrations = _purchaseRegistrationRepository.GetAll()
						.Include( e => e.RegistrationFk)
						.Include( e => e.ProductFk)
						.Include( e => e.HandlingLineFk)
						.Include( e => e.RetailerLocationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.InvoiceImage.Contains(input.Filter))
						.WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
						.WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
						.WhereIf(input.MinTotalAmountFilter != null, e => e.TotalAmount >= input.MinTotalAmountFilter)
						.WhereIf(input.MaxTotalAmountFilter != null, e => e.TotalAmount <= input.MaxTotalAmountFilter)
						.WhereIf(input.MinPurchaseDateFilter != null, e => e.PurchaseDate >= input.MinPurchaseDateFilter)
						.WhereIf(input.MaxPurchaseDateFilter != null, e => e.PurchaseDate <= input.MaxPurchaseDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceImageFilter),  e => e.InvoiceImage == input.InvoiceImageFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationFirstNameFilter), e => e.RegistrationFk != null && e.RegistrationFk.FirstName == input.RegistrationFirstNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductCtnFilter), e => e.ProductFk != null && e.ProductFk.ProductCode == input.ProductCtnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.HandlingLineCustomerCodeFilter), e => e.HandlingLineFk != null && e.HandlingLineFk.CustomerCode == input.HandlingLineCustomerCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RetailerLocationNameFilter), e => e.RetailerLocationFk != null && e.RetailerLocationFk.Name == input.RetailerLocationNameFilter);

			var query = (from o in filteredPurchaseRegistrations
                         join o1 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         join o4 in _lookup_retailerLocationRepository.GetAll() on o.RetailerLocationId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         
                         select new GetPurchaseRegistrationForViewDto() { 
							PurchaseRegistration = new PurchaseRegistrationDto
							{
                                Quantity = o.Quantity,
                                TotalAmount = o.TotalAmount,
                                PurchaseDate = o.PurchaseDate,
                                InvoiceImage = o.InvoiceImage,
                                Id = o.Id
							},
                         	RegistrationFirstName = s1 == null || s1.FirstName == null ? "" : s1.FirstName.ToString(),
                         	ProductCtn = s2 == null || s2.ProductCode == null ? "" : s2.ProductCode.ToString(),
                         	HandlingLineCustomerCode = s3 == null || s3.CustomerCode == null ? "" : s3.CustomerCode.ToString(),
                         	RetailerLocationName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
						 });


            var purchaseRegistrationListDtos = await query.ToListAsync();

            return _purchaseRegistrationsExcelExporter.ExportToFile(purchaseRegistrationListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_PurchaseRegistrations)]
         public async Task<PagedResultDto<PurchaseRegistrationRegistrationLookupTableDto>> GetAllRegistrationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_registrationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.FirstName != null && e.FirstName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var registrationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PurchaseRegistrationRegistrationLookupTableDto>();
			foreach(var registration in registrationList){
				lookupTableDtoList.Add(new PurchaseRegistrationRegistrationLookupTableDto
				{
					Id = registration.Id,
					DisplayName = registration.FirstName?.ToString()
				});
			}

            return new PagedResultDto<PurchaseRegistrationRegistrationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_PurchaseRegistrations)]
         public async Task<PagedResultDto<PurchaseRegistrationProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.ProductCode != null && e.ProductCode.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PurchaseRegistrationProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new PurchaseRegistrationProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.ProductCode?.ToString()
				});
			}

            return new PagedResultDto<PurchaseRegistrationProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_PurchaseRegistrations)]
         public async Task<PagedResultDto<PurchaseRegistrationHandlingLineLookupTableDto>> GetAllHandlingLineForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_handlingLineRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.CustomerCode != null && e.CustomerCode.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var handlingLineList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PurchaseRegistrationHandlingLineLookupTableDto>();
			foreach(var handlingLine in handlingLineList){
				lookupTableDtoList.Add(new PurchaseRegistrationHandlingLineLookupTableDto
				{
					Id = handlingLine.Id,
					DisplayName = handlingLine.CustomerCode?.ToString()
				});
			}

            return new PagedResultDto<PurchaseRegistrationHandlingLineLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_PurchaseRegistrations)]
         public async Task<PagedResultDto<PurchaseRegistrationRetailerLocationLookupTableDto>> GetAllRetailerLocationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_retailerLocationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var retailerLocationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PurchaseRegistrationRetailerLocationLookupTableDto>();
			foreach(var retailerLocation in retailerLocationList){
				lookupTableDtoList.Add(new PurchaseRegistrationRetailerLocationLookupTableDto
				{
					Id = retailerLocation.Id,
					DisplayName = retailerLocation.Name?.ToString()
				});
			}

            return new PagedResultDto<PurchaseRegistrationRetailerLocationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}