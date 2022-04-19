

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CodeTypeTables.Exporting;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.CodeTypeTables
{
	[AbpAuthorize(AppPermissions.Pages_CampaignCategories)]
    public class CampaignCategoriesAppService : RMSAppServiceBase, ICampaignCategoriesAppService
    {
		 private readonly IRepository<CampaignCategory, long> _campaignCategoryRepository;
		 private readonly ICampaignCategoriesExcelExporter _campaignCategoriesExcelExporter;
		 

		  public CampaignCategoriesAppService(IRepository<CampaignCategory, long> campaignCategoryRepository, ICampaignCategoriesExcelExporter campaignCategoriesExcelExporter ) 
		  {
			_campaignCategoryRepository = campaignCategoryRepository;
			_campaignCategoriesExcelExporter = campaignCategoriesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetCampaignCategoryForViewDto>> GetAll(GetAllCampaignCategoriesInput input)
         {
			
			var filteredCampaignCategories = _campaignCategoryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
						.WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter);

			var pagedAndFilteredCampaignCategories = filteredCampaignCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var campaignCategories = from o in pagedAndFilteredCampaignCategories
                         select new GetCampaignCategoryForViewDto() {
							CampaignCategory = new CampaignCategoryDto
							{
                                Name = o.Name,
                                IsActive = o.IsActive,
                                SortOrder = o.SortOrder,
                                Id = o.Id
							}
						};

            var totalCount = await filteredCampaignCategories.CountAsync();

            return new PagedResultDto<GetCampaignCategoryForViewDto>(
                totalCount,
                await campaignCategories.ToListAsync()
            );
         }
		 
		 public async Task<GetCampaignCategoryForViewDto> GetCampaignCategoryForView(long id)
         {
            var campaignCategory = await _campaignCategoryRepository.GetAsync(id);

            var output = new GetCampaignCategoryForViewDto { CampaignCategory = ObjectMapper.Map<CampaignCategoryDto>(campaignCategory) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_CampaignCategories_Edit)]
		 public async Task<GetCampaignCategoryForEditOutput> GetCampaignCategoryForEdit(EntityDto<long> input)
         {
            var campaignCategory = await _campaignCategoryRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCampaignCategoryForEditOutput {CampaignCategory = ObjectMapper.Map<CreateOrEditCampaignCategoryDto>(campaignCategory)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCampaignCategoryDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignCategories_Create)]
		 protected virtual async Task Create(CreateOrEditCampaignCategoryDto input)
         {
            var campaignCategory = ObjectMapper.Map<CampaignCategory>(input);

			
			if (AbpSession.TenantId != null)
			{
				campaignCategory.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _campaignCategoryRepository.InsertAsync(campaignCategory);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignCategories_Edit)]
		 protected virtual async Task Update(CreateOrEditCampaignCategoryDto input)
         {
            var campaignCategory = await _campaignCategoryRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, campaignCategory);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignCategories_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _campaignCategoryRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCampaignCategoriesToExcel(GetAllCampaignCategoriesForExcelInput input)
         {
			
			var filteredCampaignCategories = _campaignCategoryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
						.WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter);

			var query = (from o in filteredCampaignCategories
                         select new GetCampaignCategoryForViewDto() { 
							CampaignCategory = new CampaignCategoryDto
							{
                                Name = o.Name,
                                IsActive = o.IsActive,
                                SortOrder = o.SortOrder,
                                Id = o.Id
							}
						 });


            var campaignCategoryListDtos = await query.ToListAsync();

            return _campaignCategoriesExcelExporter.ExportToFile(campaignCategoryListDtos);
         }


    }
}