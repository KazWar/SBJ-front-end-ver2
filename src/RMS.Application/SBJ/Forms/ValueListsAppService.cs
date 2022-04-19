

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
	[AbpAuthorize(AppPermissions.Pages_ValueLists)]
    public class ValueListsAppService : RMSAppServiceBase, IValueListsAppService
    {
		 private readonly IRepository<ValueList, long> _valueListRepository;
		 private readonly IValueListsExcelExporter _valueListsExcelExporter;
		 

		  public ValueListsAppService(IRepository<ValueList, long> valueListRepository, IValueListsExcelExporter valueListsExcelExporter ) 
		  {
			_valueListRepository = valueListRepository;
			_valueListsExcelExporter = valueListsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetValueListForViewDto>> GetAll(GetAllValueListsInput input)
         {
			
			var filteredValueLists = _valueListRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.ListValueApiCall.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ListValueApiCallFilter),  e => e.ListValueApiCall == input.ListValueApiCallFilter);

			var pagedAndFilteredValueLists = filteredValueLists
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var valueLists = from o in pagedAndFilteredValueLists
                         select new GetValueListForViewDto() {
							ValueList = new ValueListDto
							{
                                Description = o.Description,
                                ListValueApiCall = o.ListValueApiCall,
                                Id = o.Id
							}
						};

            var totalCount = await filteredValueLists.CountAsync();

            return new PagedResultDto<GetValueListForViewDto>(
                totalCount,
                await valueLists.ToListAsync()
            );
         }
		 
		 public async Task<GetValueListForViewDto> GetValueListForView(long id)
         {
            var valueList = await _valueListRepository.GetAsync(id);

            var output = new GetValueListForViewDto { ValueList = ObjectMapper.Map<ValueListDto>(valueList) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ValueLists_Edit)]
		 public async Task<GetValueListForEditOutput> GetValueListForEdit(EntityDto<long> input)
         {
            var valueList = await _valueListRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetValueListForEditOutput {ValueList = ObjectMapper.Map<CreateOrEditValueListDto>(valueList)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditValueListDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ValueLists_Create)]
		 protected virtual async Task Create(CreateOrEditValueListDto input)
         {
            var valueList = ObjectMapper.Map<ValueList>(input);

			
			if (AbpSession.TenantId != null)
			{
				valueList.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _valueListRepository.InsertAsync(valueList);
         }

		 [AbpAuthorize(AppPermissions.Pages_ValueLists_Edit)]
		 protected virtual async Task Update(CreateOrEditValueListDto input)
         {
            var valueList = await _valueListRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, valueList);
         }

		 [AbpAuthorize(AppPermissions.Pages_ValueLists_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _valueListRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetValueListsToExcel(GetAllValueListsForExcelInput input)
         {
			
			var filteredValueLists = _valueListRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.ListValueApiCall.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ListValueApiCallFilter),  e => e.ListValueApiCall == input.ListValueApiCallFilter);

			var query = (from o in filteredValueLists
                         select new GetValueListForViewDto() { 
							ValueList = new ValueListDto
							{
                                Description = o.Description,
                                ListValueApiCall = o.ListValueApiCall,
                                Id = o.Id
							}
						 });


            var valueListListDtos = await query.ToListAsync();

            return _valueListsExcelExporter.ExportToFile(valueListListDtos);
         }


    }
}