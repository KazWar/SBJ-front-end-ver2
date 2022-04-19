using RMS.SBJ.Forms;
using RMS.SBJ.Forms;


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
	[AbpAuthorize(AppPermissions.Pages_FormFieldValueLists)]
    public class FormFieldValueListsAppService : RMSAppServiceBase, IFormFieldValueListsAppService
    {
		 private readonly IRepository<FormFieldValueList, long> _formFieldValueListRepository;
		 private readonly IFormFieldValueListsExcelExporter _formFieldValueListsExcelExporter;
		 private readonly IRepository<FormField,long> _lookup_formFieldRepository;
		 private readonly IRepository<ValueList,long> _lookup_valueListRepository;
		 

		  public FormFieldValueListsAppService(IRepository<FormFieldValueList, long> formFieldValueListRepository, IFormFieldValueListsExcelExporter formFieldValueListsExcelExporter , IRepository<FormField, long> lookup_formFieldRepository, IRepository<ValueList, long> lookup_valueListRepository) 
		  {
			_formFieldValueListRepository = formFieldValueListRepository;
			_formFieldValueListsExcelExporter = formFieldValueListsExcelExporter;
			_lookup_formFieldRepository = lookup_formFieldRepository;
		_lookup_valueListRepository = lookup_valueListRepository;
		
		  }

		 public async Task<PagedResultDto<GetFormFieldValueListForViewDto>> GetAll(GetAllFormFieldValueListsInput input)
         {
			
			var filteredFormFieldValueLists = _formFieldValueListRepository.GetAll()
						.Include( e => e.FormFieldFk)
						.Include( e => e.ValueListFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.PossibleListValues.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.PossibleListValuesFilter),  e => e.PossibleListValues == input.PossibleListValuesFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormFieldDescriptionFilter), e => e.FormFieldFk != null && e.FormFieldFk.Description == input.FormFieldDescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueListDescriptionFilter), e => e.ValueListFk != null && e.ValueListFk.Description == input.ValueListDescriptionFilter);

			var pagedAndFilteredFormFieldValueLists = filteredFormFieldValueLists
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var formFieldValueLists = from o in pagedAndFilteredFormFieldValueLists
                         join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_valueListRepository.GetAll() on o.ValueListId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetFormFieldValueListForViewDto() {
							FormFieldValueList = new FormFieldValueListDto
							{
                                PossibleListValues = o.PossibleListValues,
                                Id = o.Id
							},
                         	FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                         	ValueListDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
						};

            var totalCount = await filteredFormFieldValueLists.CountAsync();

            return new PagedResultDto<GetFormFieldValueListForViewDto>(
                totalCount,
                await formFieldValueLists.ToListAsync()
            );
         }
		 
		 public async Task<GetFormFieldValueListForViewDto> GetFormFieldValueListForView(long id)
         {
            var formFieldValueList = await _formFieldValueListRepository.GetAsync(id);

            var output = new GetFormFieldValueListForViewDto { FormFieldValueList = ObjectMapper.Map<FormFieldValueListDto>(formFieldValueList) };

		    if (output.FormFieldValueList.FormFieldId != null)
            {
                var _lookupFormField = await _lookup_formFieldRepository.FirstOrDefaultAsync((long)output.FormFieldValueList.FormFieldId);
                output.FormFieldDescription = _lookupFormField?.Description?.ToString();
            }

		    if (output.FormFieldValueList.ValueListId != null)
            {
                var _lookupValueList = await _lookup_valueListRepository.FirstOrDefaultAsync((long)output.FormFieldValueList.ValueListId);
                output.ValueListDescription = _lookupValueList?.Description?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_FormFieldValueLists_Edit)]
		 public async Task<GetFormFieldValueListForEditOutput> GetFormFieldValueListForEdit(EntityDto<long> input)
         {
            var formFieldValueList = await _formFieldValueListRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetFormFieldValueListForEditOutput {FormFieldValueList = ObjectMapper.Map<CreateOrEditFormFieldValueListDto>(formFieldValueList)};

		    if (output.FormFieldValueList.FormFieldId != null)
            {
                var _lookupFormField = await _lookup_formFieldRepository.FirstOrDefaultAsync((long)output.FormFieldValueList.FormFieldId);
                output.FormFieldDescription = _lookupFormField?.Description?.ToString();
            }

		    if (output.FormFieldValueList.ValueListId != null)
            {
                var _lookupValueList = await _lookup_valueListRepository.FirstOrDefaultAsync((long)output.FormFieldValueList.ValueListId);
                output.ValueListDescription = _lookupValueList?.Description?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditFormFieldValueListDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_FormFieldValueLists_Create)]
		 protected virtual async Task Create(CreateOrEditFormFieldValueListDto input)
         {
            var formFieldValueList = ObjectMapper.Map<FormFieldValueList>(input);

			
			if (AbpSession.TenantId != null)
			{
				formFieldValueList.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _formFieldValueListRepository.InsertAsync(formFieldValueList);
         }

		 [AbpAuthorize(AppPermissions.Pages_FormFieldValueLists_Edit)]
		 protected virtual async Task Update(CreateOrEditFormFieldValueListDto input)
         {
            var formFieldValueList = await _formFieldValueListRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, formFieldValueList);
         }

		 [AbpAuthorize(AppPermissions.Pages_FormFieldValueLists_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _formFieldValueListRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetFormFieldValueListsToExcel(GetAllFormFieldValueListsForExcelInput input)
         {
			
			var filteredFormFieldValueLists = _formFieldValueListRepository.GetAll()
						.Include( e => e.FormFieldFk)
						.Include( e => e.ValueListFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.PossibleListValues.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.PossibleListValuesFilter),  e => e.PossibleListValues == input.PossibleListValuesFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormFieldDescriptionFilter), e => e.FormFieldFk != null && e.FormFieldFk.Description == input.FormFieldDescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueListDescriptionFilter), e => e.ValueListFk != null && e.ValueListFk.Description == input.ValueListDescriptionFilter);

			var query = (from o in filteredFormFieldValueLists
                         join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_valueListRepository.GetAll() on o.ValueListId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetFormFieldValueListForViewDto() { 
							FormFieldValueList = new FormFieldValueListDto
							{
                                PossibleListValues = o.PossibleListValues,
                                Id = o.Id
							},
                         	FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                         	ValueListDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
						 });


            var formFieldValueListListDtos = await query.ToListAsync();

            return _formFieldValueListsExcelExporter.ExportToFile(formFieldValueListListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_FormFieldValueLists)]
         public async Task<PagedResultDto<FormFieldValueListFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_formFieldRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var formFieldList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<FormFieldValueListFormFieldLookupTableDto>();
			foreach(var formField in formFieldList){
				lookupTableDtoList.Add(new FormFieldValueListFormFieldLookupTableDto
				{
					Id = formField.Id,
					DisplayName = formField.Description?.ToString()
				});
			}

            return new PagedResultDto<FormFieldValueListFormFieldLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_FormFieldValueLists)]
         public async Task<PagedResultDto<FormFieldValueListValueListLookupTableDto>> GetAllValueListForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_valueListRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var valueListList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<FormFieldValueListValueListLookupTableDto>();
			foreach(var valueList in valueListList){
				lookupTableDtoList.Add(new FormFieldValueListValueListLookupTableDto
				{
					Id = valueList.Id,
					DisplayName = valueList.Description?.ToString()
				});
			}

            return new PagedResultDto<FormFieldValueListValueListLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}