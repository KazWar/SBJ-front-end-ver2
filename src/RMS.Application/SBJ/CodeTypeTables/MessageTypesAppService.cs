using RMS.SBJ.SystemTables;


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
	[AbpAuthorize(AppPermissions.Pages_MessageTypes)]
    public class MessageTypesAppService : RMSAppServiceBase, IMessageTypesAppService
    {
		 private readonly IRepository<MessageType, long> _messageTypeRepository;
		 private readonly IMessageTypesExcelExporter _messageTypesExcelExporter;
		 private readonly IRepository<Message,long> _lookup_messageRepository;
		 

		  public MessageTypesAppService(IRepository<MessageType, long> messageTypeRepository, IMessageTypesExcelExporter messageTypesExcelExporter , IRepository<Message, long> lookup_messageRepository) 
		  {
			_messageTypeRepository = messageTypeRepository;
			_messageTypesExcelExporter = messageTypesExcelExporter;
			_lookup_messageRepository = lookup_messageRepository;
		
		  }

		 public async Task<PagedResultDto<GetMessageTypeForViewDto>> GetAll(GetAllMessageTypesInput input)
         {
			
			var filteredMessageTypes = _messageTypeRepository.GetAll()
						.Include( e => e.MessageFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Source.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SourceFilter),  e => e.Source == input.SourceFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.MessageVersionFilter), e => e.MessageFk != null && e.MessageFk.Version == input.MessageVersionFilter);

			var pagedAndFilteredMessageTypes = filteredMessageTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var messageTypes = from o in pagedAndFilteredMessageTypes
                         join o1 in _lookup_messageRepository.GetAll() on o.MessageId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetMessageTypeForViewDto() {
							MessageType = new MessageTypeDto
							{
								Id = o.Id,
								MessageId = o.MessageId,
                                Name = o.Name,
                                Source = o.Source,
                                IsActive = o.IsActive
							},
                         	MessageVersion = s1 == null ? "" : s1.Version.ToString()
						};

            var totalCount = await filteredMessageTypes.CountAsync();

            return new PagedResultDto<GetMessageTypeForViewDto>(
                totalCount,
                await messageTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetMessageTypeForViewDto> GetMessageTypeForView(long id)
         {
            var messageType = await _messageTypeRepository.GetAsync(id);

            var output = new GetMessageTypeForViewDto { MessageType = ObjectMapper.Map<MessageTypeDto>(messageType) };

		    if (output.MessageType.MessageId != null)
            {
                var _lookupMessage = await _lookup_messageRepository.FirstOrDefaultAsync((long)output.MessageType.MessageId);
                output.MessageVersion = _lookupMessage.Version.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_MessageTypes_Edit)]
		 public async Task<GetMessageTypeForEditOutput> GetMessageTypeForEdit(EntityDto<long> input)
         {
            var messageType = await _messageTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetMessageTypeForEditOutput {MessageType = ObjectMapper.Map<CreateOrEditMessageTypeDto>(messageType)};

		    if (output.MessageType.MessageId != null)
            {
                var _lookupMessage = await _lookup_messageRepository.FirstOrDefaultAsync((long)output.MessageType.MessageId);
                output.MessageVersion = _lookupMessage.Version.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditMessageTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageTypes_Create)]
		 protected virtual async Task Create(CreateOrEditMessageTypeDto input)
         {
            var messageType = ObjectMapper.Map<MessageType>(input);

			
			if (AbpSession.TenantId != null)
			{
				messageType.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _messageTypeRepository.InsertAsync(messageType);
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditMessageTypeDto input)
         {
            var messageType = await _messageTypeRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, messageType);
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageTypes_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _messageTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetMessageTypesToExcel(GetAllMessageTypesForExcelInput input)
         {
			
			var filteredMessageTypes = _messageTypeRepository.GetAll()
						.Include( e => e.MessageFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Source.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SourceFilter),  e => e.Source == input.SourceFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.MessageVersionFilter), e => e.MessageFk != null && e.MessageFk.Version == input.MessageVersionFilter);

			var query = (from o in filteredMessageTypes
                         join o1 in _lookup_messageRepository.GetAll() on o.MessageId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetMessageTypeForViewDto() { 
							MessageType = new MessageTypeDto
							{
                                Name = o.Name,
                                Source = o.Source,
                                IsActive = o.IsActive,
                                Id = o.Id
							},
                         	MessageVersion = s1 == null ? "" : s1.Version.ToString()
						 });


            var messageTypeListDtos = await query.ToListAsync();

            return _messageTypesExcelExporter.ExportToFile(messageTypeListDtos);
         }

		public async Task<List<GetMessageTypeForViewDto>> GetAllMessageTypes()
		{
			var allMessageTypes = _messageTypeRepository.GetAll()
						.Include(e => e.MessageFk);

			var messageTypes = from o in allMessageTypes
							   join o1 in _lookup_messageRepository.GetAll() on o.MessageId equals o1.Id into j1
							   from s1 in j1.DefaultIfEmpty()

							   select new GetMessageTypeForViewDto()
							   {
								   MessageType = new MessageTypeDto
								   {
									   Id = o.Id,
									   MessageId = o.MessageId,
									   Name = o.Name,
									   Source = o.Source,
									   IsActive = o.IsActive
								   },
								   MessageVersion = s1 == null ? "" : s1.Version.ToString()
							   };

			return await messageTypes.ToListAsync();
		}

		[AbpAuthorize(AppPermissions.Pages_MessageTypes)]
		public async Task<PagedResultDto<MessageTypeMessageLookupTableDto>> GetAllMessageForLookupTable(GetAllForLookupTableInput input)
		{
			var query = _lookup_messageRepository.GetAll().WhereIf(
				   !string.IsNullOrWhiteSpace(input.Filter),
				  e => e.Version.ToString().Contains(input.Filter)
			   );

			var totalCount = await query.CountAsync();

			var messageList = await query
				.PageBy(input)
				.ToListAsync();

			var lookupTableDtoList = new List<MessageTypeMessageLookupTableDto>();
			foreach (var message in messageList)
			{
				lookupTableDtoList.Add(new MessageTypeMessageLookupTableDto
				{
					Id = message.Id,
					DisplayName = message.Version?.ToString()
				});
			}

			return new PagedResultDto<MessageTypeMessageLookupTableDto>(
				totalCount,
				lookupTableDtoList
			);
		}

	}
}