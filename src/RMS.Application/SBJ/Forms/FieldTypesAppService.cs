

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
	[AbpAuthorize(AppPermissions.Pages_FieldTypes)]
    public class FieldTypesAppService : RMSAppServiceBase, IFieldTypesAppService
    {
		 private readonly IRepository<FieldType, long> _fieldTypeRepository;
		 private readonly IFieldTypesExcelExporter _fieldTypesExcelExporter;
		 

		  public FieldTypesAppService(IRepository<FieldType, long> fieldTypeRepository, IFieldTypesExcelExporter fieldTypesExcelExporter ) 
		  {
			_fieldTypeRepository = fieldTypeRepository;
			_fieldTypesExcelExporter = fieldTypesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetFieldTypeForViewDto>> GetAll(GetAllFieldTypesInput input)
         {
			
			var filteredFieldTypes = _fieldTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter);

			var pagedAndFilteredFieldTypes = filteredFieldTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var fieldTypes = from o in pagedAndFilteredFieldTypes
                         select new GetFieldTypeForViewDto() {
							FieldType = new FieldTypeDto
							{
                                Description = o.Description,
                                Id = o.Id
							}
						};

            var totalCount = await filteredFieldTypes.CountAsync();

            return new PagedResultDto<GetFieldTypeForViewDto>(
                totalCount,
                await fieldTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetFieldTypeForViewDto> GetFieldTypeForView(long id)
         {
            var fieldType = await _fieldTypeRepository.GetAsync(id);

            var output = new GetFieldTypeForViewDto { FieldType = ObjectMapper.Map<FieldTypeDto>(fieldType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_FieldTypes_Edit)]
		 public async Task<GetFieldTypeForEditOutput> GetFieldTypeForEdit(EntityDto<long> input)
         {
            var fieldType = await _fieldTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetFieldTypeForEditOutput {FieldType = ObjectMapper.Map<CreateOrEditFieldTypeDto>(fieldType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditFieldTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_FieldTypes_Create)]
		 protected virtual async Task Create(CreateOrEditFieldTypeDto input)
         {
            var fieldType = ObjectMapper.Map<FieldType>(input);

			
			if (AbpSession.TenantId != null)
			{
				fieldType.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _fieldTypeRepository.InsertAsync(fieldType);
         }

		 [AbpAuthorize(AppPermissions.Pages_FieldTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditFieldTypeDto input)
         {
            var fieldType = await _fieldTypeRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, fieldType);
         }

		 [AbpAuthorize(AppPermissions.Pages_FieldTypes_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _fieldTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetFieldTypesToExcel(GetAllFieldTypesForExcelInput input)
         {
			
			var filteredFieldTypes = _fieldTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter);

			var query = (from o in filteredFieldTypes
                         select new GetFieldTypeForViewDto() { 
							FieldType = new FieldTypeDto
							{
                                Description = o.Description,
                                Id = o.Id
							}
						 });


            var fieldTypeListDtos = await query.ToListAsync();

            return _fieldTypesExcelExporter.ExportToFile(fieldTypeListDtos);
         }


    }
}