using RMS.SBJ.HandlingLines;
using RMS.SBJ.Retailers;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.HandlingLineRetailers.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.HandlingLineRetailers
{
	[AbpAuthorize(AppPermissions.Pages_HandlingLineRetailers)]
    public class HandlingLineRetailersAppService : RMSAppServiceBase, IHandlingLineRetailersAppService
    {
		 private readonly IRepository<HandlingLineRetailer, long> _handlingLineRetailerRepository;
		 private readonly IRepository<HandlingLine,long> _lookup_handlingLineRepository;
		 private readonly IRepository<Retailer,long> _lookup_retailerRepository;
		 

		  public HandlingLineRetailersAppService(IRepository<HandlingLineRetailer, long> handlingLineRetailerRepository , IRepository<HandlingLine, long> lookup_handlingLineRepository, IRepository<Retailer, long> lookup_retailerRepository) 
		  {
			_handlingLineRetailerRepository = handlingLineRetailerRepository;
			_lookup_handlingLineRepository = lookup_handlingLineRepository;
		_lookup_retailerRepository = lookup_retailerRepository;
		
		  }

		 public async Task<PagedResultDto<GetHandlingLineRetailerForViewDto>> GetAll(GetAllHandlingLineRetailersInput input)
         {
			
			var filteredHandlingLineRetailers = _handlingLineRetailerRepository.GetAll()
						.Include( e => e.HandlingLineFk)
						.Include( e => e.RetailerFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.HandlingLineCustomerCodeFilter), e => e.HandlingLineFk != null && e.HandlingLineFk.CustomerCode == input.HandlingLineCustomerCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RetailerNameFilter), e => e.RetailerFk != null && e.RetailerFk.Name == input.RetailerNameFilter);

			var pagedAndFilteredHandlingLineRetailers = filteredHandlingLineRetailers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var handlingLineRetailers = from o in pagedAndFilteredHandlingLineRetailers
                         join o1 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_retailerRepository.GetAll() on o.RetailerId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetHandlingLineRetailerForViewDto() {
							HandlingLineRetailer = new HandlingLineRetailerDto
							{
                                Id = o.Id
							},
                         	HandlingLineCustomerCode = s1 == null || s1.CustomerCode == null ? "" : s1.CustomerCode.ToString(),
                         	RetailerName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredHandlingLineRetailers.CountAsync();

            return new PagedResultDto<GetHandlingLineRetailerForViewDto>(
                totalCount,
                await handlingLineRetailers.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_HandlingLineRetailers_Edit)]
		 public async Task<GetHandlingLineRetailerForEditOutput> GetHandlingLineRetailerForEdit(EntityDto<long> input)
         {
            var handlingLineRetailer = await _handlingLineRetailerRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetHandlingLineRetailerForEditOutput {HandlingLineRetailer = ObjectMapper.Map<CreateOrEditHandlingLineRetailerDto>(handlingLineRetailer)};

		    if (output.HandlingLineRetailer.HandlingLineId != null)
            {
                var _lookupHandlingLine = await _lookup_handlingLineRepository.FirstOrDefaultAsync((long)output.HandlingLineRetailer.HandlingLineId);
                output.HandlingLineCustomerCode = _lookupHandlingLine?.CustomerCode?.ToString();
            }

		    if (output.HandlingLineRetailer.RetailerId != null)
            {
                var _lookupRetailer = await _lookup_retailerRepository.FirstOrDefaultAsync((long)output.HandlingLineRetailer.RetailerId);
                output.RetailerName = _lookupRetailer?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditHandlingLineRetailerDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineRetailers_Create)]
		 protected virtual async Task Create(CreateOrEditHandlingLineRetailerDto input)
         {
            var handlingLineRetailer = ObjectMapper.Map<HandlingLineRetailer>(input);

			
			if (AbpSession.TenantId != null)
			{
				handlingLineRetailer.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _handlingLineRetailerRepository.InsertAsync(handlingLineRetailer);
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineRetailers_Edit)]
		 protected virtual async Task Update(CreateOrEditHandlingLineRetailerDto input)
         {
            var handlingLineRetailer = await _handlingLineRetailerRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, handlingLineRetailer);
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineRetailers_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _handlingLineRetailerRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_HandlingLineRetailers)]
         public async Task<PagedResultDto<HandlingLineRetailerHandlingLineLookupTableDto>> GetAllHandlingLineForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_handlingLineRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.CustomerCode != null && e.CustomerCode.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var handlingLineList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<HandlingLineRetailerHandlingLineLookupTableDto>();
			foreach(var handlingLine in handlingLineList){
				lookupTableDtoList.Add(new HandlingLineRetailerHandlingLineLookupTableDto
				{
					Id = handlingLine.Id,
					DisplayName = handlingLine.CustomerCode?.ToString()
				});
			}

            return new PagedResultDto<HandlingLineRetailerHandlingLineLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_HandlingLineRetailers)]
         public async Task<PagedResultDto<HandlingLineRetailerRetailerLookupTableDto>> GetAllRetailerForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_retailerRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var retailerList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<HandlingLineRetailerRetailerLookupTableDto>();
			foreach(var retailer in retailerList){
				lookupTableDtoList.Add(new HandlingLineRetailerRetailerLookupTableDto
				{
					Id = retailer.Id,
					DisplayName = retailer.Name?.ToString()
				});
			}

            return new PagedResultDto<HandlingLineRetailerRetailerLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}