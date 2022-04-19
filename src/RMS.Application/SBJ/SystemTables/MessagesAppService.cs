using RMS.SBJ.SystemTables;


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
	[AbpAuthorize(AppPermissions.Pages_Messages)]
    public class MessagesAppService : RMSAppServiceBase, IMessagesAppService
    {
		 private readonly IRepository<Message, long> _messageRepository;
		 private readonly IMessagesExcelExporter _messagesExcelExporter;
		 private readonly IRepository<SystemLevel,long> _lookup_systemLevelRepository;
		 

		  public MessagesAppService(IRepository<Message, long> messageRepository, IMessagesExcelExporter messagesExcelExporter , IRepository<SystemLevel, long> lookup_systemLevelRepository) 
		  {
			_messageRepository = messageRepository;
			_messagesExcelExporter = messagesExcelExporter;
			_lookup_systemLevelRepository = lookup_systemLevelRepository;
		
		  }

		 public async Task<PagedResultDto<GetMessageForViewDto>> GetAll(GetAllMessagesInput input)
         {
			
			var filteredMessages = _messageRepository.GetAll()
						.Include( e => e.SystemLevelFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Version.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.VersionFilter),  e => e.Version == input.VersionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SystemLevelDescriptionFilter), e => e.SystemLevelFk != null && e.SystemLevelFk.Description == input.SystemLevelDescriptionFilter);

			var pagedAndFilteredMessages = filteredMessages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var messages = from o in pagedAndFilteredMessages
                         join o1 in _lookup_systemLevelRepository.GetAll() on o.SystemLevelId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetMessageForViewDto() {
							Message = new MessageDto
							{
                                Version = o.Version,
                                Id = o.Id
							},
                         	SystemLevelDescription = s1 == null ? "" : s1.Description.ToString()
						};

            var totalCount = await filteredMessages.CountAsync();

            return new PagedResultDto<GetMessageForViewDto>(
                totalCount,
                await messages.ToListAsync()
            );
         }
		 
		 public async Task<GetMessageForViewDto> GetMessageForView(long id)
         {
            var message = await _messageRepository.GetAsync(id);

            var output = new GetMessageForViewDto { Message = ObjectMapper.Map<MessageDto>(message) };

		    if (output.Message.SystemLevelId != null)
            {
                var _lookupSystemLevel = await _lookup_systemLevelRepository.FirstOrDefaultAsync((long)output.Message.SystemLevelId);
                output.SystemLevelDescription = _lookupSystemLevel.Description.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Messages_Edit)]
		 public async Task<GetMessageForEditOutput> GetMessageForEdit(EntityDto<long> input)
         {
            var message = await _messageRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetMessageForEditOutput {Message = ObjectMapper.Map<CreateOrEditMessageDto>(message)};

		    if (output.Message.SystemLevelId != null)
            {
                var _lookupSystemLevel = await _lookup_systemLevelRepository.FirstOrDefaultAsync((long)output.Message.SystemLevelId);
                output.SystemLevelDescription = _lookupSystemLevel.Description.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditMessageDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Messages_Create)]
		 protected virtual async Task Create(CreateOrEditMessageDto input)
         {
            var message = ObjectMapper.Map<Message>(input);

			
			if (AbpSession.TenantId != null)
			{
				message.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _messageRepository.InsertAsync(message);
         }

		 [AbpAuthorize(AppPermissions.Pages_Messages_Edit)]
		 protected virtual async Task Update(CreateOrEditMessageDto input)
         {
            var message = await _messageRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, message);
         }

		 [AbpAuthorize(AppPermissions.Pages_Messages_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _messageRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetMessagesToExcel(GetAllMessagesForExcelInput input)
         {
			
			var filteredMessages = _messageRepository.GetAll()
						.Include( e => e.SystemLevelFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Version.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.VersionFilter),  e => e.Version == input.VersionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SystemLevelDescriptionFilter), e => e.SystemLevelFk != null && e.SystemLevelFk.Description == input.SystemLevelDescriptionFilter);

			var query = (from o in filteredMessages
                         join o1 in _lookup_systemLevelRepository.GetAll() on o.SystemLevelId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetMessageForViewDto() { 
							Message = new MessageDto
							{
                                Version = o.Version,
                                Id = o.Id
							},
                         	SystemLevelDescription = s1 == null ? "" : s1.Description.ToString()
						 });


            var messageListDtos = await query.ToListAsync();

            return _messagesExcelExporter.ExportToFile(messageListDtos);
         }

		public async Task<List<GetMessageForViewDto>> GetAllMessages()
		{
			var allMessages = _messageRepository.GetAll()
						.Include(e => e.SystemLevelFk);

			var messages = from o in allMessages
						   join o1 in _lookup_systemLevelRepository.GetAll() on o.SystemLevelId equals o1.Id into j1
						   from s1 in j1.DefaultIfEmpty()

						   select new GetMessageForViewDto()
						   {
							   Message = new MessageDto
							   {
								   Version = o.Version,
								   Id = o.Id,
								   SystemLevelId = o.SystemLevelId
							   },
							   SystemLevelDescription = s1 == null ? "" : s1.Description.ToString()
						   };

			return await messages.ToListAsync();
		}

		[AbpAuthorize(AppPermissions.Pages_Messages)]
		public async Task<PagedResultDto<MessageSystemLevelLookupTableDto>> GetAllSystemLevelForLookupTable(GetAllForLookupTableInput input)
		{
			var query = _lookup_systemLevelRepository.GetAll().WhereIf(
				   !string.IsNullOrWhiteSpace(input.Filter),
				  e => e.Description.ToString().Contains(input.Filter)
			   );

			var totalCount = await query.CountAsync();

			var systemLevelList = await query
				.PageBy(input)
				.ToListAsync();

			var lookupTableDtoList = new List<MessageSystemLevelLookupTableDto>();
			foreach (var systemLevel in systemLevelList)
			{
				lookupTableDtoList.Add(new MessageSystemLevelLookupTableDto
				{
					Id = systemLevel.Id,
					DisplayName = systemLevel.Description?.ToString()
				});
			}

			return new PagedResultDto<MessageSystemLevelLookupTableDto>(
				totalCount,
				lookupTableDtoList
			);
		}
	}
}