

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.SystemTables.Exporting;
using RMS.SBJ.SystemTables.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.SystemTables
{
	[AbpAuthorize(AppPermissions.Pages_SystemLevels)]
    public class SystemLevelsAppService : RMSAppServiceBase, ISystemLevelsAppService
    {
		 private readonly IRepository<SystemLevel, long> _systemLevelRepository;
		 private readonly ISystemLevelsExcelExporter _systemLevelsExcelExporter;
		 

		  public SystemLevelsAppService(IRepository<SystemLevel, long> systemLevelRepository, ISystemLevelsExcelExporter systemLevelsExcelExporter ) 
		  {
			_systemLevelRepository = systemLevelRepository;
			_systemLevelsExcelExporter = systemLevelsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetSystemLevelForViewDto>> GetAll(GetAllSystemLevelsInput input)
         {
			
			var filteredSystemLevels = _systemLevelRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter);

			var pagedAndFilteredSystemLevels = filteredSystemLevels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var systemLevels = from o in pagedAndFilteredSystemLevels
                         select new GetSystemLevelForViewDto() {
							SystemLevel = new SystemLevelDto
							{
                                Description = o.Description,
                                Id = o.Id
							}
						};

            var totalCount = await filteredSystemLevels.CountAsync();

            return new PagedResultDto<GetSystemLevelForViewDto>(
                totalCount,
                await systemLevels.ToListAsync()
            );
         }
		 
		 public async Task<GetSystemLevelForViewDto> GetSystemLevelForView(long id)
         {
            var systemLevel = await _systemLevelRepository.GetAsync(id);

            var output = new GetSystemLevelForViewDto { SystemLevel = ObjectMapper.Map<SystemLevelDto>(systemLevel) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_SystemLevels_Edit)]
		 public async Task<GetSystemLevelForEditOutput> GetSystemLevelForEdit(EntityDto<long> input)
         {
            var systemLevel = await _systemLevelRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetSystemLevelForEditOutput {SystemLevel = ObjectMapper.Map<CreateOrEditSystemLevelDto>(systemLevel)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditSystemLevelDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemLevels_Create)]
		 protected virtual async Task Create(CreateOrEditSystemLevelDto input)
         {
            var systemLevel = ObjectMapper.Map<SystemLevel>(input);

			
			if (AbpSession.TenantId != null)
			{
				systemLevel.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _systemLevelRepository.InsertAsync(systemLevel);
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemLevels_Edit)]
		 protected virtual async Task Update(CreateOrEditSystemLevelDto input)
         {
            var systemLevel = await _systemLevelRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, systemLevel);
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemLevels_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _systemLevelRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetSystemLevelsToExcel(GetAllSystemLevelsForExcelInput input)
         {
			
			var filteredSystemLevels = _systemLevelRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter);

			var query = (from o in filteredSystemLevels
                         select new GetSystemLevelForViewDto() { 
							SystemLevel = new SystemLevelDto
							{
                                Description = o.Description,
                                Id = o.Id
							}
						 });


            var systemLevelListDtos = await query.ToListAsync();

            return _systemLevelsExcelExporter.ExportToFile(systemLevelListDtos);
         }


    }
}