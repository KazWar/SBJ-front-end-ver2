

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
	[AbpAuthorize(AppPermissions.Pages_MessageComponentTypes)]
    public class MessageComponentTypesAppService : RMSAppServiceBase, IMessageComponentTypesAppService
    {
		 private readonly IRepository<MessageComponentType, long> _messageComponentTypeRepository;
		 private readonly IMessageComponentTypesExcelExporter _messageComponentTypesExcelExporter;
		 

		  public MessageComponentTypesAppService(IRepository<MessageComponentType, long> messageComponentTypeRepository, IMessageComponentTypesExcelExporter messageComponentTypesExcelExporter ) 
		  {
			_messageComponentTypeRepository = messageComponentTypeRepository;
			_messageComponentTypesExcelExporter = messageComponentTypesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetMessageComponentTypeForViewDto>> GetAll(GetAllMessageComponentTypesInput input)
         {
			
			var filteredMessageComponentTypes = _messageComponentTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var pagedAndFilteredMessageComponentTypes = filteredMessageComponentTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var messageComponentTypes = from o in pagedAndFilteredMessageComponentTypes
                         select new GetMessageComponentTypeForViewDto() {
							MessageComponentType = new MessageComponentTypeDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						};

            var totalCount = await filteredMessageComponentTypes.CountAsync();

            return new PagedResultDto<GetMessageComponentTypeForViewDto>(
                totalCount,
                await messageComponentTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetMessageComponentTypeForViewDto> GetMessageComponentTypeForView(long id)
         {
            var messageComponentType = await _messageComponentTypeRepository.GetAsync(id);

            var output = new GetMessageComponentTypeForViewDto { MessageComponentType = ObjectMapper.Map<MessageComponentTypeDto>(messageComponentType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_MessageComponentTypes_Edit)]
		 public async Task<GetMessageComponentTypeForEditOutput> GetMessageComponentTypeForEdit(EntityDto<long> input)
         {
            var messageComponentType = await _messageComponentTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetMessageComponentTypeForEditOutput {MessageComponentType = ObjectMapper.Map<CreateOrEditMessageComponentTypeDto>(messageComponentType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditMessageComponentTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageComponentTypes_Create)]
		 protected virtual async Task Create(CreateOrEditMessageComponentTypeDto input)
         {
            var messageComponentType = ObjectMapper.Map<MessageComponentType>(input);

			
			if (AbpSession.TenantId != null)
			{
				messageComponentType.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _messageComponentTypeRepository.InsertAsync(messageComponentType);
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageComponentTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditMessageComponentTypeDto input)
         {
            var messageComponentType = await _messageComponentTypeRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, messageComponentType);
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageComponentTypes_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _messageComponentTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetMessageComponentTypesToExcel(GetAllMessageComponentTypesForExcelInput input)
         {
			
			var filteredMessageComponentTypes = _messageComponentTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var query = (from o in filteredMessageComponentTypes
                         select new GetMessageComponentTypeForViewDto() { 
							MessageComponentType = new MessageComponentTypeDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						 });


            var messageComponentTypeListDtos = await query.ToListAsync();

            return _messageComponentTypesExcelExporter.ExportToFile(messageComponentTypeListDtos);
         }

		public async Task<List<GetMessageComponentTypeForViewDto>> GetAllMessageComponentTypes()
		{
			var allMessageComponentTypes = _messageComponentTypeRepository.GetAll();

			var messageComponentTypes = from o in allMessageComponentTypes
										select new GetMessageComponentTypeForViewDto()
										{
											MessageComponentType = new MessageComponentTypeDto
											{
												Name = o.Name,
												Id = o.Id
											}
										};

			return await messageComponentTypes.ToListAsync();
		}


	}
}