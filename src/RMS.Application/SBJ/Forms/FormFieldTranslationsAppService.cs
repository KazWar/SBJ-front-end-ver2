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
	[AbpAuthorize(AppPermissions.Pages_FormFieldTranslations)]
    public class FormFieldTranslationsAppService : RMSAppServiceBase, IFormFieldTranslationsAppService
    {
		 private readonly IRepository<FormFieldTranslation, long> _formFieldTranslationRepository;
		 private readonly IFormFieldTranslationsExcelExporter _formFieldTranslationsExcelExporter;
		 private readonly IRepository<FormField,long> _lookup_formFieldRepository;
		 private readonly IRepository<Locale,long> _lookup_localeRepository;
		 

		  public FormFieldTranslationsAppService(IRepository<FormFieldTranslation, long> formFieldTranslationRepository, IFormFieldTranslationsExcelExporter formFieldTranslationsExcelExporter , IRepository<FormField, long> lookup_formFieldRepository, IRepository<Locale, long> lookup_localeRepository) 
		  {
			_formFieldTranslationRepository = formFieldTranslationRepository;
			_formFieldTranslationsExcelExporter = formFieldTranslationsExcelExporter;
			_lookup_formFieldRepository = lookup_formFieldRepository;
		_lookup_localeRepository = lookup_localeRepository;
		
		  }

		 public async Task<PagedResultDto<GetFormFieldTranslationForViewDto>> GetAll(GetAllFormFieldTranslationsInput input)
         {
			var filteredFormFieldTranslations = _formFieldTranslationRepository.GetAll()
						.Include( e => e.FormFieldFk)
						.Include( e => e.LocaleFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Label.Contains(input.Filter) || e.DefaultValue.Contains(input.Filter) || e.RegularExpression.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.LabelFilter),  e => e.Label == input.LabelFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DefaultValueFilter),  e => e.DefaultValue == input.DefaultValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RegularExpressionFilter),  e => e.RegularExpression == input.RegularExpressionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormFieldDescriptionFilter), e => e.FormFieldFk != null && e.FormFieldFk.Description == input.FormFieldDescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocaleLanguageCodeFilter), e => e.LocaleFk != null && e.LocaleFk.LanguageCode == input.LocaleLanguageCodeFilter);

            var pagedAndFilteredFormFieldTranslations = filteredFormFieldTranslations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var formFieldTranslations = from o in pagedAndFilteredFormFieldTranslations
                         join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_localeRepository.GetAll() on o.LocaleId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetFormFieldTranslationForViewDto() {
							FormFieldTranslation = new FormFieldTranslationDto
							{
                                Label = o.Label,
                                DefaultValue = o.DefaultValue,
                                RegularExpression = o.RegularExpression,
                                Id = o.Id,
                                FormFieldId = o.FormFieldId,
                                LocaleId = o.LocaleId
							},
                         	FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                         	LocaleLanguageCode = s2 == null || s2.LanguageCode == null ? "" : s2.LanguageCode.ToString()
						};

            var totalCount = await filteredFormFieldTranslations.CountAsync();

            return new PagedResultDto<GetFormFieldTranslationForViewDto>(
                totalCount,
                await formFieldTranslations.ToListAsync()
            );
         }
		 
		 public async Task<GetFormFieldTranslationForViewDto> GetFormFieldTranslationForView(long id)
         {
            var formFieldTranslation = await _formFieldTranslationRepository.GetAsync(id);

            var output = new GetFormFieldTranslationForViewDto { FormFieldTranslation = ObjectMapper.Map<FormFieldTranslationDto>(formFieldTranslation) };

		    if (output.FormFieldTranslation.FormFieldId != null)
            {
                var _lookupFormField = await _lookup_formFieldRepository.FirstOrDefaultAsync((long)output.FormFieldTranslation.FormFieldId);
                output.FormFieldDescription = _lookupFormField?.Description?.ToString();
            }

		    if (output.FormFieldTranslation.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.FormFieldTranslation.LocaleId);
                output.LocaleLanguageCode = _lookupLocale?.LanguageCode?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_FormFieldTranslations_Edit)]
		 public async Task<GetFormFieldTranslationForEditOutput> GetFormFieldTranslationForEdit(EntityDto<long> input)
         {
            var formFieldTranslation = await _formFieldTranslationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetFormFieldTranslationForEditOutput {FormFieldTranslation = ObjectMapper.Map<CreateOrEditFormFieldTranslationDto>(formFieldTranslation)};

		    if (output.FormFieldTranslation.FormFieldId != null)
            {
                var _lookupFormField = await _lookup_formFieldRepository.FirstOrDefaultAsync((long)output.FormFieldTranslation.FormFieldId);
                output.FormFieldDescription = _lookupFormField?.Description?.ToString();
            }

		    if (output.FormFieldTranslation.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.FormFieldTranslation.LocaleId);
                output.LocaleLanguageCode = _lookupLocale?.LanguageCode?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditFormFieldTranslationDto input)
         {
            if(input.Id == null || input.Id == 0){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_FormFieldTranslations_Create)]
		 protected virtual async Task Create(CreateOrEditFormFieldTranslationDto input)
         {
            var formFieldTranslation = ObjectMapper.Map<FormFieldTranslation>(input);

			
			if (AbpSession.TenantId != null)
			{
				formFieldTranslation.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _formFieldTranslationRepository.InsertAsync(formFieldTranslation);
         }

		 [AbpAuthorize(AppPermissions.Pages_FormFieldTranslations_Edit)]
		 protected virtual async Task Update(CreateOrEditFormFieldTranslationDto input)
         {
            var formFieldTranslation = await _formFieldTranslationRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, formFieldTranslation);
         }

		 [AbpAuthorize(AppPermissions.Pages_FormFieldTranslations_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _formFieldTranslationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetFormFieldTranslationsToExcel(GetAllFormFieldTranslationsForExcelInput input)
         {
			
			var filteredFormFieldTranslations = _formFieldTranslationRepository.GetAll()
						.Include( e => e.FormFieldFk)
						.Include( e => e.LocaleFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Label.Contains(input.Filter) || e.DefaultValue.Contains(input.Filter) || e.RegularExpression.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.LabelFilter),  e => e.Label == input.LabelFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DefaultValueFilter),  e => e.DefaultValue == input.DefaultValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RegularExpressionFilter),  e => e.RegularExpression == input.RegularExpressionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormFieldDescriptionFilter), e => e.FormFieldFk != null && e.FormFieldFk.Description == input.FormFieldDescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocaleLanguageCodeFilter), e => e.LocaleFk != null && e.LocaleFk.LanguageCode == input.LocaleLanguageCodeFilter);

			var query = (from o in filteredFormFieldTranslations
                         join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_localeRepository.GetAll() on o.LocaleId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetFormFieldTranslationForViewDto() { 
							FormFieldTranslation = new FormFieldTranslationDto
							{
                                Label = o.Label,
                                DefaultValue = o.DefaultValue,
                                RegularExpression = o.RegularExpression,
                                Id = o.Id
							},
                         	FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                         	LocaleLanguageCode = s2 == null || s2.LanguageCode == null ? "" : s2.LanguageCode.ToString()
						 });


            var formFieldTranslationListDtos = await query.ToListAsync();

            return _formFieldTranslationsExcelExporter.ExportToFile(formFieldTranslationListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_FormFieldTranslations)]
         public async Task<PagedResultDto<FormFieldTranslationFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_formFieldRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var formFieldList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<FormFieldTranslationFormFieldLookupTableDto>();
			foreach(var formField in formFieldList){
				lookupTableDtoList.Add(new FormFieldTranslationFormFieldLookupTableDto
				{
					Id = formField.Id,
					DisplayName = formField.Description?.ToString()
				});
			}

            return new PagedResultDto<FormFieldTranslationFormFieldLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_FormFieldTranslations)]
         public async Task<PagedResultDto<FormFieldTranslationLocaleLookupTableDto>> GetAllLocaleForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_localeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.LanguageCode != null && e.LanguageCode.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var localeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<FormFieldTranslationLocaleLookupTableDto>();
			foreach(var locale in localeList){
				lookupTableDtoList.Add(new FormFieldTranslationLocaleLookupTableDto
				{
					Id = locale.Id,
					DisplayName = locale.LanguageCode?.ToString()
				});
			}

            return new PagedResultDto<FormFieldTranslationLocaleLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

        public async Task<List<GetFormFieldTranslationForViewDto>> GetAllFormFieldTranslations()
        {
            var allFormFieldTranslations = _formFieldTranslationRepository.GetAll()
                        .Include(e => e.FormFieldFk)
                        .Include(e => e.LocaleFk);

            var formFieldTranslations = from o in allFormFieldTranslations
                                        join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()
                                        join o2 in _lookup_localeRepository.GetAll() on o.LocaleId equals o2.Id into j2
                                        from s2 in j2.DefaultIfEmpty()
                                        select new GetFormFieldTranslationForViewDto()
                                        {
                                            FormFieldTranslation = new FormFieldTranslationDto
                                            {
                                                Label = o.Label,
                                                DefaultValue = o.DefaultValue,
                                                RegularExpression = o.RegularExpression,
                                                Id = o.Id,
                                                FormFieldId = o.FormFieldId,
                                                LocaleId = o.LocaleId
                                            },
                                            FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                                            LocaleLanguageCode = s2 == null || s2.LanguageCode == null ? "" : s2.LanguageCode.ToString()
                                        };

            return await formFieldTranslations.ToListAsync();
        }
    }
}