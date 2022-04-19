using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables;


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
	[AbpAuthorize(AppPermissions.Pages_CampaignCategoryTranslations)]
    public class CampaignCategoryTranslationsAppService : RMSAppServiceBase, ICampaignCategoryTranslationsAppService
    {
		 private readonly IRepository<CampaignCategoryTranslation, long> _campaignCategoryTranslationRepository;
		 private readonly ICampaignCategoryTranslationsExcelExporter _campaignCategoryTranslationsExcelExporter;
		 private readonly IRepository<Locale,long> _lookup_localeRepository;
		 private readonly IRepository<CampaignCategory,long> _lookup_campaignCategoryRepository;
		 

		  public CampaignCategoryTranslationsAppService(IRepository<CampaignCategoryTranslation, long> campaignCategoryTranslationRepository, ICampaignCategoryTranslationsExcelExporter campaignCategoryTranslationsExcelExporter , IRepository<Locale, long> lookup_localeRepository, IRepository<CampaignCategory, long> lookup_campaignCategoryRepository) 
		  {
			_campaignCategoryTranslationRepository = campaignCategoryTranslationRepository;
			_campaignCategoryTranslationsExcelExporter = campaignCategoryTranslationsExcelExporter;
			_lookup_localeRepository = lookup_localeRepository;
		_lookup_campaignCategoryRepository = lookup_campaignCategoryRepository;
		
		  }

		 public async Task<PagedResultDto<GetCampaignCategoryTranslationForViewDto>> GetAll(GetAllCampaignCategoryTranslationsInput input)
         {
			
			var filteredCampaignCategoryTranslations = _campaignCategoryTranslationRepository.GetAll()
						.Include( e => e.LocaleFk)
						.Include( e => e.CampaignCategoryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocaleDescriptionFilter), e => e.LocaleFk != null && e.LocaleFk.Description == input.LocaleDescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CampaignCategoryNameFilter), e => e.CampaignCategoryFk != null && e.CampaignCategoryFk.Name == input.CampaignCategoryNameFilter);

			var pagedAndFilteredCampaignCategoryTranslations = filteredCampaignCategoryTranslations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var campaignCategoryTranslations = from o in pagedAndFilteredCampaignCategoryTranslations
                         join o1 in _lookup_localeRepository.GetAll() on o.LocaleId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_campaignCategoryRepository.GetAll() on o.CampaignCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetCampaignCategoryTranslationForViewDto() {
							CampaignCategoryTranslation = new CampaignCategoryTranslationDto
							{
                                Name = o.Name,
                                Id = o.Id
							},
                         	LocaleDescription = s1 == null ? "" : s1.Description.ToString(),
                         	CampaignCategoryName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredCampaignCategoryTranslations.CountAsync();

            return new PagedResultDto<GetCampaignCategoryTranslationForViewDto>(
                totalCount,
                await campaignCategoryTranslations.ToListAsync()
            );
         }
		 
		 public async Task<GetCampaignCategoryTranslationForViewDto> GetCampaignCategoryTranslationForView(long id)
         {
            var campaignCategoryTranslation = await _campaignCategoryTranslationRepository.GetAsync(id);

            var output = new GetCampaignCategoryTranslationForViewDto { CampaignCategoryTranslation = ObjectMapper.Map<CampaignCategoryTranslationDto>(campaignCategoryTranslation) };

		    if (output.CampaignCategoryTranslation.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.CampaignCategoryTranslation.LocaleId);
                output.LocaleDescription = _lookupLocale.Description.ToString();
            }

		    if (output.CampaignCategoryTranslation.CampaignCategoryId != null)
            {
                var _lookupCampaignCategory = await _lookup_campaignCategoryRepository.FirstOrDefaultAsync((long)output.CampaignCategoryTranslation.CampaignCategoryId);
                output.CampaignCategoryName = _lookupCampaignCategory.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_CampaignCategoryTranslations_Edit)]
		 public async Task<GetCampaignCategoryTranslationForEditOutput> GetCampaignCategoryTranslationForEdit(EntityDto<long> input)
         {
            var campaignCategoryTranslation = await _campaignCategoryTranslationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCampaignCategoryTranslationForEditOutput {CampaignCategoryTranslation = ObjectMapper.Map<CreateOrEditCampaignCategoryTranslationDto>(campaignCategoryTranslation)};

		    if (output.CampaignCategoryTranslation.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.CampaignCategoryTranslation.LocaleId);
                output.LocaleDescription = _lookupLocale.Description.ToString();
            }

		    if (output.CampaignCategoryTranslation.CampaignCategoryId != null)
            {
                var _lookupCampaignCategory = await _lookup_campaignCategoryRepository.FirstOrDefaultAsync((long)output.CampaignCategoryTranslation.CampaignCategoryId);
                output.CampaignCategoryName = _lookupCampaignCategory.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCampaignCategoryTranslationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignCategoryTranslations_Create)]
		 protected virtual async Task Create(CreateOrEditCampaignCategoryTranslationDto input)
         {
            var campaignCategoryTranslation = ObjectMapper.Map<CampaignCategoryTranslation>(input);

			
			if (AbpSession.TenantId != null)
			{
				campaignCategoryTranslation.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _campaignCategoryTranslationRepository.InsertAsync(campaignCategoryTranslation);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignCategoryTranslations_Edit)]
		 protected virtual async Task Update(CreateOrEditCampaignCategoryTranslationDto input)
         {
            var campaignCategoryTranslation = await _campaignCategoryTranslationRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, campaignCategoryTranslation);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignCategoryTranslations_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _campaignCategoryTranslationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCampaignCategoryTranslationsToExcel(GetAllCampaignCategoryTranslationsForExcelInput input)
         {
			
			var filteredCampaignCategoryTranslations = _campaignCategoryTranslationRepository.GetAll()
						.Include( e => e.LocaleFk)
						.Include( e => e.CampaignCategoryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocaleDescriptionFilter), e => e.LocaleFk != null && e.LocaleFk.Description == input.LocaleDescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CampaignCategoryNameFilter), e => e.CampaignCategoryFk != null && e.CampaignCategoryFk.Name == input.CampaignCategoryNameFilter);

			var query = (from o in filteredCampaignCategoryTranslations
                         join o1 in _lookup_localeRepository.GetAll() on o.LocaleId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_campaignCategoryRepository.GetAll() on o.CampaignCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetCampaignCategoryTranslationForViewDto() { 
							CampaignCategoryTranslation = new CampaignCategoryTranslationDto
							{
                                Name = o.Name,
                                Id = o.Id
							},
                         	LocaleDescription = s1 == null ? "" : s1.Description.ToString(),
                         	CampaignCategoryName = s2 == null ? "" : s2.Name.ToString()
						 });


            var campaignCategoryTranslationListDtos = await query.ToListAsync();

            return _campaignCategoryTranslationsExcelExporter.ExportToFile(campaignCategoryTranslationListDtos);
         }


    }
}