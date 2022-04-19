using RMS.SBJ.Forms;
using RMS.SBJ.CodeTypeTables;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.Forms.Exporting;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.Forms
{
	[AbpAuthorize(AppPermissions.Pages_ListValueTranslations)]
    public class ListValueTranslationsAppService : RMSAppServiceBase, IListValueTranslationsAppService
    {
		 private readonly IRepository<ListValueTranslation, long> _listValueTranslationRepository;
		 private readonly IListValueTranslationsExcelExporter _listValueTranslationsExcelExporter;
		 private readonly IRepository<ListValue,long> _lookup_listValueRepository;
		 private readonly IRepository<Locale,long> _lookup_localeRepository;
		 

		  public ListValueTranslationsAppService(IRepository<ListValueTranslation, long> listValueTranslationRepository, IListValueTranslationsExcelExporter listValueTranslationsExcelExporter , IRepository<ListValue, long> lookup_listValueRepository, IRepository<Locale, long> lookup_localeRepository) 
		  {
			_listValueTranslationRepository = listValueTranslationRepository;
			_listValueTranslationsExcelExporter = listValueTranslationsExcelExporter;
			_lookup_listValueRepository = lookup_listValueRepository;
		_lookup_localeRepository = lookup_localeRepository;
		
		  }

		 public async Task<PagedResultDto<GetListValueTranslationForViewDto>> GetAll(GetAllListValueTranslationsInput input)
         {
			
			var filteredListValueTranslations = _listValueTranslationRepository.GetAll()
						.Include( e => e.ListValueFk)
						.Include( e => e.LocaleFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.KeyValue.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeyValueFilter),  e => e.KeyValue == input.KeyValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ListValueKeyValueFilter), e => e.ListValueFk != null && e.ListValueFk.KeyValue == input.ListValueKeyValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocaleLanguageCodeFilter), e => e.LocaleFk != null && e.LocaleFk.LanguageCode == input.LocaleLanguageCodeFilter);

			var pagedAndFilteredListValueTranslations = filteredListValueTranslations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var listValueTranslations = from o in pagedAndFilteredListValueTranslations
                         join o1 in _lookup_listValueRepository.GetAll() on o.ListValueId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_localeRepository.GetAll() on o.LocaleId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetListValueTranslationForViewDto() {
							ListValueTranslation = new ListValueTranslationDto
							{
                                KeyValue = o.KeyValue,
                                Description = o.Description,
                                Id = o.Id
							},
                         	ListValueKeyValue = s1 == null || s1.KeyValue == null ? "" : s1.KeyValue.ToString(),
                         	LocaleLanguageCode = s2 == null || s2.LanguageCode == null ? "" : s2.LanguageCode.ToString()
						};

            var totalCount = await filteredListValueTranslations.CountAsync();

            return new PagedResultDto<GetListValueTranslationForViewDto>(
                totalCount,
                await listValueTranslations.ToListAsync()
            );
         }
		 
		 public async Task<GetListValueTranslationForViewDto> GetListValueTranslationForView(long id)
         {
            var listValueTranslation = await _listValueTranslationRepository.GetAsync(id);

            var output = new GetListValueTranslationForViewDto { ListValueTranslation = ObjectMapper.Map<ListValueTranslationDto>(listValueTranslation) };

		    if (output.ListValueTranslation.ListValueId != null)
            {
                var _lookupListValue = await _lookup_listValueRepository.FirstOrDefaultAsync((long)output.ListValueTranslation.ListValueId);
                output.ListValueKeyValue = _lookupListValue?.KeyValue?.ToString();
            }

		    if (output.ListValueTranslation.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.ListValueTranslation.LocaleId);
                output.LocaleLanguageCode = _lookupLocale?.LanguageCode?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ListValueTranslations_Edit)]
		 public async Task<GetListValueTranslationForEditOutput> GetListValueTranslationForEdit(EntityDto<long> input)
         {
            var listValueTranslation = await _listValueTranslationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetListValueTranslationForEditOutput {ListValueTranslation = ObjectMapper.Map<CreateOrEditListValueTranslationDto>(listValueTranslation)};

		    if (output.ListValueTranslation.ListValueId != null)
            {
                var _lookupListValue = await _lookup_listValueRepository.FirstOrDefaultAsync((long)output.ListValueTranslation.ListValueId);
                output.ListValueKeyValue = _lookupListValue?.KeyValue?.ToString();
            }

		    if (output.ListValueTranslation.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.ListValueTranslation.LocaleId);
                output.LocaleLanguageCode = _lookupLocale?.LanguageCode?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditListValueTranslationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ListValueTranslations_Create)]
		 protected virtual async Task Create(CreateOrEditListValueTranslationDto input)
         {
            var listValueTranslation = ObjectMapper.Map<ListValueTranslation>(input);

			
			if (AbpSession.TenantId != null)
			{
				listValueTranslation.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _listValueTranslationRepository.InsertAsync(listValueTranslation);
         }

		 [AbpAuthorize(AppPermissions.Pages_ListValueTranslations_Edit)]
		 protected virtual async Task Update(CreateOrEditListValueTranslationDto input)
         {
            var listValueTranslation = await _listValueTranslationRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, listValueTranslation);
         }

		 [AbpAuthorize(AppPermissions.Pages_ListValueTranslations_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _listValueTranslationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetListValueTranslationsToExcel(GetAllListValueTranslationsForExcelInput input)
         {
			
			var filteredListValueTranslations = _listValueTranslationRepository.GetAll()
						.Include( e => e.ListValueFk)
						.Include( e => e.LocaleFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.KeyValue.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeyValueFilter),  e => e.KeyValue == input.KeyValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ListValueKeyValueFilter), e => e.ListValueFk != null && e.ListValueFk.KeyValue == input.ListValueKeyValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocaleLanguageCodeFilter), e => e.LocaleFk != null && e.LocaleFk.LanguageCode == input.LocaleLanguageCodeFilter);

			var query = (from o in filteredListValueTranslations
                         join o1 in _lookup_listValueRepository.GetAll() on o.ListValueId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_localeRepository.GetAll() on o.LocaleId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetListValueTranslationForViewDto() { 
							ListValueTranslation = new ListValueTranslationDto
							{
                                KeyValue = o.KeyValue,
                                Description = o.Description,
                                Id = o.Id
							},
                         	ListValueKeyValue = s1 == null || s1.KeyValue == null ? "" : s1.KeyValue.ToString(),
                         	LocaleLanguageCode = s2 == null || s2.LanguageCode == null ? "" : s2.LanguageCode.ToString()
						 });


            var listValueTranslationListDtos = await query.ToListAsync();

            return _listValueTranslationsExcelExporter.ExportToFile(listValueTranslationListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ListValueTranslations)]
         public async Task<PagedResultDto<ListValueTranslationListValueLookupTableDto>> GetAllListValueForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_listValueRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.KeyValue != null && e.KeyValue.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var listValueList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ListValueTranslationListValueLookupTableDto>();
			foreach(var listValue in listValueList){
				lookupTableDtoList.Add(new ListValueTranslationListValueLookupTableDto
				{
					Id = listValue.Id,
					DisplayName = listValue.KeyValue?.ToString()
				});
			}

            return new PagedResultDto<ListValueTranslationListValueLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ListValueTranslations)]
         public async Task<PagedResultDto<ListValueTranslationLocaleLookupTableDto>> GetAllLocaleForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_localeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.LanguageCode != null && e.LanguageCode.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var localeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ListValueTranslationLocaleLookupTableDto>();
			foreach(var locale in localeList){
				lookupTableDtoList.Add(new ListValueTranslationLocaleLookupTableDto
				{
					Id = locale.Id,
					DisplayName = locale.LanguageCode?.ToString()
				});
			}

            return new PagedResultDto<ListValueTranslationLocaleLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}