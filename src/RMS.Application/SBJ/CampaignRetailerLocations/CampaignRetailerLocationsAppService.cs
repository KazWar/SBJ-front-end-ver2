using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.RetailerLocations;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CampaignRetailerLocations.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.CampaignRetailerLocations
{
	[AbpAuthorize(AppPermissions.Pages_CampaignRetailerLocations)]
    public class CampaignRetailerLocationsAppService : RMSAppServiceBase, ICampaignRetailerLocationsAppService
    {
		 private readonly IRepository<CampaignRetailerLocation, long> _campaignRetailerLocationRepository;
		 private readonly IRepository<Campaign,long> _lookup_campaignRepository;
		 private readonly IRepository<RetailerLocation,long> _lookup_retailerLocationRepository;
		 

		  public CampaignRetailerLocationsAppService(IRepository<CampaignRetailerLocation, long> campaignRetailerLocationRepository , IRepository<Campaign, long> lookup_campaignRepository, IRepository<RetailerLocation, long> lookup_retailerLocationRepository) 
		  {
			_campaignRetailerLocationRepository = campaignRetailerLocationRepository;
			_lookup_campaignRepository = lookup_campaignRepository;
		_lookup_retailerLocationRepository = lookup_retailerLocationRepository;
		
		  }

		 public async Task<PagedResultDto<GetCampaignRetailerLocationForViewDto>> GetAll(GetAllCampaignRetailerLocationsInput input)
         {
			
			var filteredCampaignRetailerLocations = _campaignRetailerLocationRepository.GetAll()
						.Include( e => e.CampaignFk)
						.Include( e => e.RetailerLocationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RetailerLocationNameFilter), e => e.RetailerLocationFk != null && e.RetailerLocationFk.Name == input.RetailerLocationNameFilter);

			var pagedAndFilteredCampaignRetailerLocations = filteredCampaignRetailerLocations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var campaignRetailerLocations = from o in pagedAndFilteredCampaignRetailerLocations
                         join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_retailerLocationRepository.GetAll() on o.RetailerLocationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetCampaignRetailerLocationForViewDto() {
							CampaignRetailerLocation = new CampaignRetailerLocationDto
							{
                                Id = o.Id
							},
                         	CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	RetailerLocationName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredCampaignRetailerLocations.CountAsync();

            return new PagedResultDto<GetCampaignRetailerLocationForViewDto>(
                totalCount,
                await campaignRetailerLocations.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_CampaignRetailerLocations_Edit)]
		 public async Task<GetCampaignRetailerLocationForEditOutput> GetCampaignRetailerLocationForEdit(EntityDto<long> input)
         {
            var campaignRetailerLocation = await _campaignRetailerLocationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCampaignRetailerLocationForEditOutput {CampaignRetailerLocation = ObjectMapper.Map<CreateOrEditCampaignRetailerLocationDto>(campaignRetailerLocation)};

		    if (output.CampaignRetailerLocation.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.CampaignRetailerLocation.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

		    if (output.CampaignRetailerLocation.RetailerLocationId != null)
            {
                var _lookupRetailerLocation = await _lookup_retailerLocationRepository.FirstOrDefaultAsync((long)output.CampaignRetailerLocation.RetailerLocationId);
                output.RetailerLocationName = _lookupRetailerLocation?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCampaignRetailerLocationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignRetailerLocations_Create)]
		 protected virtual async Task Create(CreateOrEditCampaignRetailerLocationDto input)
         {
            var campaignRetailerLocation = ObjectMapper.Map<CampaignRetailerLocation>(input);

			
			if (AbpSession.TenantId != null)
			{
				campaignRetailerLocation.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _campaignRetailerLocationRepository.InsertAsync(campaignRetailerLocation);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignRetailerLocations_Edit)]
		 protected virtual async Task Update(CreateOrEditCampaignRetailerLocationDto input)
         {
            var campaignRetailerLocation = await _campaignRetailerLocationRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, campaignRetailerLocation);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignRetailerLocations_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _campaignRetailerLocationRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_CampaignRetailerLocations)]
         public async Task<PagedResultDto<CampaignRetailerLocationCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_campaignRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var campaignList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<CampaignRetailerLocationCampaignLookupTableDto>();
			foreach(var campaign in campaignList){
				lookupTableDtoList.Add(new CampaignRetailerLocationCampaignLookupTableDto
				{
					Id = campaign.Id,
					DisplayName = campaign.Name?.ToString()
				});
			}

            return new PagedResultDto<CampaignRetailerLocationCampaignLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_CampaignRetailerLocations)]
         public async Task<PagedResultDto<CampaignRetailerLocationRetailerLocationLookupTableDto>> GetAllRetailerLocationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_retailerLocationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var retailerLocationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<CampaignRetailerLocationRetailerLocationLookupTableDto>();
			foreach(var retailerLocation in retailerLocationList){
				lookupTableDtoList.Add(new CampaignRetailerLocationRetailerLocationLookupTableDto
				{
					Id = retailerLocation.Id,
					DisplayName = retailerLocation.Name?.ToString()
				});
			}

            return new PagedResultDto<CampaignRetailerLocationRetailerLocationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}