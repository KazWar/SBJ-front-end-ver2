using RMS.SBJ.HandlingLines;
using RMS.SBJ.Products;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.HandlingLineProducts.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.HandlingLineProducts
{
	[AbpAuthorize(AppPermissions.Pages_HandlingLineProducts)]
    public class HandlingLineProductsAppService : RMSAppServiceBase, IHandlingLineProductsAppService
    {
		 private readonly IRepository<HandlingLineProduct, long> _handlingLineProductRepository;
		 private readonly IRepository<HandlingLine,long> _lookup_handlingLineRepository;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public HandlingLineProductsAppService(IRepository<HandlingLineProduct, long> handlingLineProductRepository , IRepository<HandlingLine, long> lookup_handlingLineRepository, IRepository<Product, long> lookup_productRepository) 
		  {
			_handlingLineProductRepository = handlingLineProductRepository;
			_lookup_handlingLineRepository = lookup_handlingLineRepository;
		_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetHandlingLineProductForViewDto>> GetAll(GetAllHandlingLineProductsInput input)
         {
			
			var filteredHandlingLineProducts = _handlingLineProductRepository.GetAll()
						.Include( e => e.HandlingLineFk)
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.HandlingLineCustomerCodeFilter), e => e.HandlingLineFk != null && e.HandlingLineFk.CustomerCode == input.HandlingLineCustomerCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductDescriptionFilter), e => e.ProductFk != null && e.ProductFk.Description == input.ProductDescriptionFilter);

			var pagedAndFilteredHandlingLineProducts = filteredHandlingLineProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var handlingLineProducts = from o in pagedAndFilteredHandlingLineProducts
                         join o1 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetHandlingLineProductForViewDto() {
							HandlingLineProduct = new HandlingLineProductDto
							{
                                Id = o.Id
							},
                         	HandlingLineCustomerCode = s1 == null || s1.CustomerCode == null ? "" : s1.CustomerCode.ToString(),
                         	ProductDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
						};

            var totalCount = await filteredHandlingLineProducts.CountAsync();

            return new PagedResultDto<GetHandlingLineProductForViewDto>(
                totalCount,
                await handlingLineProducts.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_HandlingLineProducts_Edit)]
		 public async Task<GetHandlingLineProductForEditOutput> GetHandlingLineProductForEdit(EntityDto<long> input)
         {
            var handlingLineProduct = await _handlingLineProductRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetHandlingLineProductForEditOutput {HandlingLineProduct = ObjectMapper.Map<CreateOrEditHandlingLineProductDto>(handlingLineProduct)};

		    if (output.HandlingLineProduct.HandlingLineId != null)
            {
                var _lookupHandlingLine = await _lookup_handlingLineRepository.FirstOrDefaultAsync((long)output.HandlingLineProduct.HandlingLineId);
                output.HandlingLineCustomerCode = _lookupHandlingLine?.CustomerCode?.ToString();
            }

		    if (output.HandlingLineProduct.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.HandlingLineProduct.ProductId);
                output.ProductDescription = _lookupProduct?.Description?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditHandlingLineProductDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineProducts_Create)]
		 protected virtual async Task Create(CreateOrEditHandlingLineProductDto input)
         {
            var handlingLineProduct = ObjectMapper.Map<HandlingLineProduct>(input);

			
			if (AbpSession.TenantId != null)
			{
				handlingLineProduct.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _handlingLineProductRepository.InsertAsync(handlingLineProduct);
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineProducts_Edit)]
		 protected virtual async Task Update(CreateOrEditHandlingLineProductDto input)
         {
            var handlingLineProduct = await _handlingLineProductRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, handlingLineProduct);
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineProducts_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _handlingLineProductRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_HandlingLineProducts)]
         public async Task<PagedResultDto<HandlingLineProductHandlingLineLookupTableDto>> GetAllHandlingLineForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_handlingLineRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.CustomerCode != null && e.CustomerCode.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var handlingLineList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<HandlingLineProductHandlingLineLookupTableDto>();
			foreach(var handlingLine in handlingLineList){
				lookupTableDtoList.Add(new HandlingLineProductHandlingLineLookupTableDto
				{
					Id = handlingLine.Id,
					DisplayName = handlingLine.CustomerCode?.ToString()
				});
			}

            return new PagedResultDto<HandlingLineProductHandlingLineLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_HandlingLineProducts)]
         public async Task<PagedResultDto<HandlingLineProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<HandlingLineProductProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new HandlingLineProductProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Description?.ToString()
				});
			}

            return new PagedResultDto<HandlingLineProductProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}