using RMS.SBJ.CampaignProcesses;
					using System.Collections.Generic;
using RMS.SBJ.CodeTypeTables;
					using System.Collections.Generic;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CampaignProcesses.Exporting;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.CampaignProcesses
{
	[AbpAuthorize(AppPermissions.Pages_CampaignCampaignTypes)]
    public class CampaignCampaignTypesAppService : RMSAppServiceBase, ICampaignCampaignTypesAppService
    {
		 private readonly IRepository<CampaignCampaignType, long> _campaignCampaignTypeRepository;
		 private readonly ICampaignCampaignTypesExcelExporter _campaignCampaignTypesExcelExporter;
		 private readonly IRepository<Campaign,long> _lookup_campaignRepository;
		 private readonly IRepository<CampaignType,long> _lookup_campaignTypeRepository;
		 

		  public CampaignCampaignTypesAppService(IRepository<CampaignCampaignType, long> campaignCampaignTypeRepository, ICampaignCampaignTypesExcelExporter campaignCampaignTypesExcelExporter , IRepository<Campaign, long> lookup_campaignRepository, IRepository<CampaignType, long> lookup_campaignTypeRepository) 
		  {
			_campaignCampaignTypeRepository = campaignCampaignTypeRepository;
			_campaignCampaignTypesExcelExporter = campaignCampaignTypesExcelExporter;
			_lookup_campaignRepository = lookup_campaignRepository;
		_lookup_campaignTypeRepository = lookup_campaignTypeRepository;
		
		  }

		 public async Task<PagedResultDto<GetCampaignCampaignTypeForViewDto>> GetAll(GetAllCampaignCampaignTypesInput input)
         {
			
			var filteredCampaignCampaignTypes = _campaignCampaignTypeRepository.GetAll()
						.Include( e => e.CampaignFk)
						.Include( e => e.CampaignTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CampaignDescriptionFilter), e => e.CampaignFk != null && e.CampaignFk.Description == input.CampaignDescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeNameFilter), e => e.CampaignTypeFk != null && e.CampaignTypeFk.Name == input.CampaignTypeNameFilter);

			var pagedAndFilteredCampaignCampaignTypes = filteredCampaignCampaignTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var campaignCampaignTypes = from o in pagedAndFilteredCampaignCampaignTypes
                         join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetCampaignCampaignTypeForViewDto() {
							CampaignCampaignType = new CampaignCampaignTypeDto
							{
                                Description = o.Description,
                                Id = o.Id,
								CampaignId = o.CampaignId,
								CampaignTypeId = o.CampaignTypeId
							},
                         	CampaignDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                         	CampaignTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredCampaignCampaignTypes.CountAsync();

            return new PagedResultDto<GetCampaignCampaignTypeForViewDto>(
                totalCount,
                await campaignCampaignTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetCampaignCampaignTypeForViewDto> GetCampaignCampaignTypeForView(long id)
         {
            var campaignCampaignType = await _campaignCampaignTypeRepository.GetAsync(id);

            var output = new GetCampaignCampaignTypeForViewDto { CampaignCampaignType = ObjectMapper.Map<CampaignCampaignTypeDto>(campaignCampaignType) };

		    if (output.CampaignCampaignType.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.CampaignCampaignType.CampaignId);
                output.CampaignDescription = _lookupCampaign?.Description?.ToString();
            }

		    if (output.CampaignCampaignType.CampaignTypeId != null)
            {
                var _lookupCampaignType = await _lookup_campaignTypeRepository.FirstOrDefaultAsync((long)output.CampaignCampaignType.CampaignTypeId);
                output.CampaignTypeName = _lookupCampaignType?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_CampaignCampaignTypes_Edit)]
		 public async Task<GetCampaignCampaignTypeForEditOutput> GetCampaignCampaignTypeForEdit(EntityDto<long> input)
         {
            var campaignCampaignType = await _campaignCampaignTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCampaignCampaignTypeForEditOutput {CampaignCampaignType = ObjectMapper.Map<CreateOrEditCampaignCampaignTypeDto>(campaignCampaignType)};

		    if (output.CampaignCampaignType.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.CampaignCampaignType.CampaignId);
                output.CampaignDescription = _lookupCampaign?.Description?.ToString();
            }

		    if (output.CampaignCampaignType.CampaignTypeId != null)
            {
                var _lookupCampaignType = await _lookup_campaignTypeRepository.FirstOrDefaultAsync((long)output.CampaignCampaignType.CampaignTypeId);
                output.CampaignTypeName = _lookupCampaignType?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCampaignCampaignTypeDto input)
         {
            if(input.Id == null || input.Id == 0){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignCampaignTypes_Create)]
		 protected virtual async Task Create(CreateOrEditCampaignCampaignTypeDto input)
         {
            var campaignCampaignType = ObjectMapper.Map<CampaignCampaignType>(input);

			
			if (AbpSession.TenantId != null)
			{
				campaignCampaignType.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _campaignCampaignTypeRepository.InsertAsync(campaignCampaignType);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignCampaignTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditCampaignCampaignTypeDto input)
         {
            var campaignCampaignType = await _campaignCampaignTypeRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, campaignCampaignType);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignCampaignTypes_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _campaignCampaignTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCampaignCampaignTypesToExcel(GetAllCampaignCampaignTypesForExcelInput input)
         {
			
			var filteredCampaignCampaignTypes = _campaignCampaignTypeRepository.GetAll()
						.Include( e => e.CampaignFk)
						.Include( e => e.CampaignTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CampaignDescriptionFilter), e => e.CampaignFk != null && e.CampaignFk.Description == input.CampaignDescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeNameFilter), e => e.CampaignTypeFk != null && e.CampaignTypeFk.Name == input.CampaignTypeNameFilter);

			var query = (from o in filteredCampaignCampaignTypes
                         join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetCampaignCampaignTypeForViewDto() { 
							CampaignCampaignType = new CampaignCampaignTypeDto
							{
                                Description = o.Description,
                                Id = o.Id
							},
                         	CampaignDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                         	CampaignTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						 });


            var campaignCampaignTypeListDtos = await query.ToListAsync();

            return _campaignCampaignTypesExcelExporter.ExportToFile(campaignCampaignTypeListDtos);
         }


			[AbpAuthorize(AppPermissions.Pages_CampaignCampaignTypes)]
			public async Task<List<CampaignCampaignTypeCampaignLookupTableDto>> GetAllCampaignForTableDropdown()
			{
				return await _lookup_campaignRepository.GetAll()
					.Select(campaign => new CampaignCampaignTypeCampaignLookupTableDto
					{
						Id = campaign.Id,
						DisplayName = campaign == null || campaign.Description == null ? "" : campaign.Description.ToString()
					}).ToListAsync();
			}
							
			[AbpAuthorize(AppPermissions.Pages_CampaignCampaignTypes)]
			public async Task<List<CampaignCampaignTypeCampaignTypeLookupTableDto>> GetAllCampaignTypeForTableDropdown()
			{
				return await _lookup_campaignTypeRepository.GetAll()
					.Select(campaignType => new CampaignCampaignTypeCampaignTypeLookupTableDto
					{
						Id = campaignType.Id,
						DisplayName = campaignType == null || campaignType.Name == null ? "" : campaignType.Name.ToString()
					}).ToListAsync();
			}

		public async Task<List<GetCampaignCampaignTypeForViewDto>> GetAllCampaignCampaignTypes()
		{

			var allCampaignCampaignTypes = _campaignCampaignTypeRepository.GetAll()
						.Include(e => e.CampaignFk)
						.Include(e => e.CampaignTypeFk);

			var campaignCampaignTypes = from o in allCampaignCampaignTypes
										join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
										from s1 in j1.DefaultIfEmpty()

										join o2 in _lookup_campaignTypeRepository.GetAll() on o.CampaignTypeId equals o2.Id into j2
										from s2 in j2.DefaultIfEmpty()

										select new GetCampaignCampaignTypeForViewDto()
										{
											CampaignCampaignType = new CampaignCampaignTypeDto
											{
												Description = o.Description,
												Id = o.Id,
												CampaignId = o.CampaignId,
												CampaignTypeId = o.CampaignTypeId
											},
											CampaignDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
											CampaignTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
										};

			var totalCount = await allCampaignCampaignTypes.CountAsync();

			return await campaignCampaignTypes.ToListAsync();

			//return new List<GetCampaignCampaignTypeForViewDto>(
			//	totalCount,
			//	await campaignCampaignTypes.ToListAsync()
			//);
		}

	}
}