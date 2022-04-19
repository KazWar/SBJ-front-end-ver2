

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
	[AbpAuthorize(AppPermissions.Pages_ProcessEvents)]
    public class ProcessEventsAppService : RMSAppServiceBase, IProcessEventsAppService
    {
		 private readonly IRepository<ProcessEvent, long> _processEventRepository;
		 private readonly IProcessEventsExcelExporter _processEventsExcelExporter;
		 

		  public ProcessEventsAppService(IRepository<ProcessEvent, long> processEventRepository, IProcessEventsExcelExporter processEventsExcelExporter ) 
		  {
			_processEventRepository = processEventRepository;
			_processEventsExcelExporter = processEventsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetProcessEventForViewDto>> GetAll(GetAllProcessEventsInput input)
         {
			
			var filteredProcessEvents = _processEventRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) );

			var pagedAndFilteredProcessEvents = filteredProcessEvents
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var processEvents = from o in pagedAndFilteredProcessEvents
                         select new GetProcessEventForViewDto() {
							ProcessEvent = new ProcessEventDto
							{
                                Name = o.Name,
                                IsActive = o.IsActive,
                                Id = o.Id
							}
						};

            var totalCount = await filteredProcessEvents.CountAsync();

            return new PagedResultDto<GetProcessEventForViewDto>(
                totalCount,
                await processEvents.ToListAsync()
            );
         }
		 
		 public async Task<GetProcessEventForViewDto> GetProcessEventForView(long id)
         {
            var processEvent = await _processEventRepository.GetAsync(id);

            var output = new GetProcessEventForViewDto { ProcessEvent = ObjectMapper.Map<ProcessEventDto>(processEvent) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ProcessEvents_Edit)]
		 public async Task<GetProcessEventForEditOutput> GetProcessEventForEdit(EntityDto<long> input)
         {
            var processEvent = await _processEventRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProcessEventForEditOutput {ProcessEvent = ObjectMapper.Map<CreateOrEditProcessEventDto>(processEvent)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProcessEventDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ProcessEvents_Create)]
		 protected virtual async Task Create(CreateOrEditProcessEventDto input)
         {
            var processEvent = ObjectMapper.Map<ProcessEvent>(input);

			
			if (AbpSession.TenantId != null)
			{
				processEvent.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _processEventRepository.InsertAsync(processEvent);
         }

		 [AbpAuthorize(AppPermissions.Pages_ProcessEvents_Edit)]
		 protected virtual async Task Update(CreateOrEditProcessEventDto input)
         {
            var processEvent = await _processEventRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, processEvent);
         }

		 [AbpAuthorize(AppPermissions.Pages_ProcessEvents_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _processEventRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProcessEventsToExcel(GetAllProcessEventsForExcelInput input)
         {
			
			var filteredProcessEvents = _processEventRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) );

			var query = (from o in filteredProcessEvents
                         select new GetProcessEventForViewDto() { 
							ProcessEvent = new ProcessEventDto
							{
                                Name = o.Name,
                                IsActive = o.IsActive,
                                Id = o.Id
							}
						 });


            var processEventListDtos = await query.ToListAsync();

            return _processEventsExcelExporter.ExportToFile(processEventListDtos);
         }


    }
}