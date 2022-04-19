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
	[AbpAuthorize(AppPermissions.Pages_ListValues)]
    public class ListValuesAppService : RMSAppServiceBase, IListValuesAppService
    {
		 private readonly IRepository<ListValue, long> _listValueRepository;
		 private readonly IListValuesExcelExporter _listValuesExcelExporter;
		 private readonly IRepository<ValueList,long> _lookup_valueListRepository;
		 

		  public ListValuesAppService(IRepository<ListValue, long> listValueRepository, IListValuesExcelExporter listValuesExcelExporter , IRepository<ValueList, long> lookup_valueListRepository) 
		  {
			_listValueRepository = listValueRepository;
			_listValuesExcelExporter = listValuesExcelExporter;
			_lookup_valueListRepository = lookup_valueListRepository;
		
		  }

		 public async Task<PagedResultDto<GetListValueForViewDto>> GetAll(GetAllListValuesInput input)
         {
			
			var filteredListValues = _listValueRepository.GetAll()
						.Include( e => e.ValueListFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.KeyValue.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeyValueFilter),  e => e.KeyValue == input.KeyValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
						.WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueListDescriptionFilter), e => e.ValueListFk != null && e.ValueListFk.Description == input.ValueListDescriptionFilter);

			var pagedAndFilteredListValues = filteredListValues
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var listValues = from o in pagedAndFilteredListValues
                         join o1 in _lookup_valueListRepository.GetAll() on o.ValueListId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetListValueForViewDto() {
							ListValue = new ListValueDto
							{
                                KeyValue = o.KeyValue,
                                Description = o.Description,
                                SortOrder = o.SortOrder,
                                Id = o.Id
							},
                         	ValueListDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						};

            var totalCount = await filteredListValues.CountAsync();

            return new PagedResultDto<GetListValueForViewDto>(
                totalCount,
                await listValues.ToListAsync()
            );
         }
		 
		 public async Task<GetListValueForViewDto> GetListValueForView(long id)
         {
            var listValue = await _listValueRepository.GetAsync(id);

            var output = new GetListValueForViewDto { ListValue = ObjectMapper.Map<ListValueDto>(listValue) };

		    if (output.ListValue.ValueListId != null)
            {
                var _lookupValueList = await _lookup_valueListRepository.FirstOrDefaultAsync((long)output.ListValue.ValueListId);
                output.ValueListDescription = _lookupValueList?.Description?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ListValues_Edit)]
		 public async Task<GetListValueForEditOutput> GetListValueForEdit(EntityDto<long> input)
         {
            var listValue = await _listValueRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetListValueForEditOutput {ListValue = ObjectMapper.Map<CreateOrEditListValueDto>(listValue)};

		    if (output.ListValue.ValueListId != null)
            {
                var _lookupValueList = await _lookup_valueListRepository.FirstOrDefaultAsync((long)output.ListValue.ValueListId);
                output.ValueListDescription = _lookupValueList?.Description?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditListValueDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ListValues_Create)]
		 protected virtual async Task Create(CreateOrEditListValueDto input)
         {
            var listValue = ObjectMapper.Map<ListValue>(input);

			
			if (AbpSession.TenantId != null)
			{
				listValue.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _listValueRepository.InsertAsync(listValue);
         }

		 [AbpAuthorize(AppPermissions.Pages_ListValues_Edit)]
		 protected virtual async Task Update(CreateOrEditListValueDto input)
         {
            var listValue = await _listValueRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, listValue);
         }

		 [AbpAuthorize(AppPermissions.Pages_ListValues_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _listValueRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetListValuesToExcel(GetAllListValuesForExcelInput input)
         {
			
			var filteredListValues = _listValueRepository.GetAll()
						.Include( e => e.ValueListFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.KeyValue.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeyValueFilter),  e => e.KeyValue == input.KeyValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
						.WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueListDescriptionFilter), e => e.ValueListFk != null && e.ValueListFk.Description == input.ValueListDescriptionFilter);

			var query = (from o in filteredListValues
                         join o1 in _lookup_valueListRepository.GetAll() on o.ValueListId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetListValueForViewDto() { 
							ListValue = new ListValueDto
							{
                                KeyValue = o.KeyValue,
                                Description = o.Description,
                                SortOrder = o.SortOrder,
                                Id = o.Id
							},
                         	ValueListDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						 });


            var listValueListDtos = await query.ToListAsync();

            return _listValuesExcelExporter.ExportToFile(listValueListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ListValues)]
         public async Task<PagedResultDto<ListValueValueListLookupTableDto>> GetAllValueListForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_valueListRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var valueListList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ListValueValueListLookupTableDto>();
			foreach(var valueList in valueListList){
				lookupTableDtoList.Add(new ListValueValueListLookupTableDto
				{
					Id = valueList.Id,
					DisplayName = valueList.Description?.ToString()
				});
			}

            return new PagedResultDto<ListValueValueListLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}