using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.SystemTables;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CampaignProcesses.Exporting;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.CampaignProcesses
{
	[AbpAuthorize(AppPermissions.Pages_CampaignMessages)]
    public class CampaignMessagesAppService : RMSAppServiceBase, ICampaignMessagesAppService
    {
		 private readonly IRepository<CampaignMessage, long> _campaignMessageRepository;
		 private readonly ICampaignMessagesExcelExporter _campaignMessagesExcelExporter;
		 private readonly IRepository<Campaign,long> _lookup_campaignRepository;
		 private readonly IRepository<Message,long> _lookup_messageRepository;
		 

		  public CampaignMessagesAppService(IRepository<CampaignMessage, long> campaignMessageRepository, ICampaignMessagesExcelExporter campaignMessagesExcelExporter , IRepository<Campaign, long> lookup_campaignRepository, IRepository<Message, long> lookup_messageRepository) 
		  {
			_campaignMessageRepository = campaignMessageRepository;
			_campaignMessagesExcelExporter = campaignMessagesExcelExporter;
			_lookup_campaignRepository = lookup_campaignRepository;
		_lookup_messageRepository = lookup_messageRepository;
		
		  }

		 public async Task<PagedResultDto<GetCampaignMessageForViewDto>> GetAll(GetAllCampaignMessagesInput input)
         {
			
			var filteredCampaignMessages = _campaignMessageRepository.GetAll()
						.Include( e => e.CampaignFk)
						.Include( e => e.MessageFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MessageVersionFilter), e => e.MessageFk != null && e.MessageFk.Version == input.MessageVersionFilter);

			var pagedAndFilteredCampaignMessages = filteredCampaignMessages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var campaignMessages = from o in pagedAndFilteredCampaignMessages
                         join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_messageRepository.GetAll() on o.MessageId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetCampaignMessageForViewDto() {
							CampaignMessage = new CampaignMessageDto
							{
                                IsActive = o.IsActive,
                                Id = o.Id
							},
                         	CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	MessageVersion = s2 == null || s2.Version == null ? "" : s2.Version.ToString()
						};

            var totalCount = await filteredCampaignMessages.CountAsync();

            return new PagedResultDto<GetCampaignMessageForViewDto>(
                totalCount,
                await campaignMessages.ToListAsync()
            );
         }
		 
		 public async Task<GetCampaignMessageForViewDto> GetCampaignMessageForView(long id)
         {
            var campaignMessage = await _campaignMessageRepository.GetAsync(id);

            var output = new GetCampaignMessageForViewDto { CampaignMessage = ObjectMapper.Map<CampaignMessageDto>(campaignMessage) };

		    if (output.CampaignMessage.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.CampaignMessage.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

		    if (output.CampaignMessage.MessageId != null)
            {
                var _lookupMessage = await _lookup_messageRepository.FirstOrDefaultAsync((long)output.CampaignMessage.MessageId);
                output.MessageVersion = _lookupMessage?.Version?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_CampaignMessages_Edit)]
		 public async Task<GetCampaignMessageForEditOutput> GetCampaignMessageForEdit(EntityDto<long> input)
         {
            var campaignMessage = await _campaignMessageRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCampaignMessageForEditOutput {CampaignMessage = ObjectMapper.Map<CreateOrEditCampaignMessageDto>(campaignMessage)};

		    if (output.CampaignMessage.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.CampaignMessage.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

		    if (output.CampaignMessage.MessageId != null)
            {
                var _lookupMessage = await _lookup_messageRepository.FirstOrDefaultAsync((long)output.CampaignMessage.MessageId);
                output.MessageVersion = _lookupMessage?.Version?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCampaignMessageDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignMessages_Create)]
		 protected virtual async Task Create(CreateOrEditCampaignMessageDto input)
         {
            var campaignMessage = ObjectMapper.Map<CampaignMessage>(input);

			
			if (AbpSession.TenantId != null)
			{
				campaignMessage.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _campaignMessageRepository.InsertAsync(campaignMessage);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignMessages_Edit)]
		 protected virtual async Task Update(CreateOrEditCampaignMessageDto input)
         {
            var campaignMessage = await _campaignMessageRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, campaignMessage);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignMessages_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _campaignMessageRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCampaignMessagesToExcel(GetAllCampaignMessagesForExcelInput input)
         {
			
			var filteredCampaignMessages = _campaignMessageRepository.GetAll()
						.Include( e => e.CampaignFk)
						.Include( e => e.MessageFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MessageVersionFilter), e => e.MessageFk != null && e.MessageFk.Version == input.MessageVersionFilter);

			var query = (from o in filteredCampaignMessages
                         join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_messageRepository.GetAll() on o.MessageId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetCampaignMessageForViewDto() { 
							CampaignMessage = new CampaignMessageDto
							{
                                IsActive = o.IsActive,
                                Id = o.Id
							},
                         	CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	MessageVersion = s2 == null || s2.Version == null ? "" : s2.Version.ToString()
						 });


            var campaignMessageListDtos = await query.ToListAsync();

            return _campaignMessagesExcelExporter.ExportToFile(campaignMessageListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_CampaignMessages)]
         public async Task<PagedResultDto<CampaignMessageCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_campaignRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var campaignList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<CampaignMessageCampaignLookupTableDto>();
			foreach(var campaign in campaignList){
				lookupTableDtoList.Add(new CampaignMessageCampaignLookupTableDto
				{
					Id = campaign.Id,
					DisplayName = campaign.Name?.ToString()
				});
			}

            return new PagedResultDto<CampaignMessageCampaignLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_CampaignMessages)]
         public async Task<PagedResultDto<CampaignMessageMessageLookupTableDto>> GetAllMessageForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_messageRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Version != null && e.Version.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var messageList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<CampaignMessageMessageLookupTableDto>();
			foreach(var message in messageList){
				lookupTableDtoList.Add(new CampaignMessageMessageLookupTableDto
				{
					Id = message.Id,
					DisplayName = message.Version?.ToString()
				});
			}

            return new PagedResultDto<CampaignMessageMessageLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

        public async Task<List<GetCampaignMessageForViewDto>> GetAllCampaignMessage()
        {
            var allCampaignMessages = _campaignMessageRepository.GetAll()
                        .Include(e => e.CampaignFk)
                        .Include(e => e.MessageFk);

            var campaignMessages = from o in allCampaignMessages
                                   join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_messageRepository.GetAll() on o.MessageId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   select new GetCampaignMessageForViewDto()
                                   {
                                       CampaignMessage = new CampaignMessageDto
                                       {
                                           IsActive = o.IsActive,
                                           Id = o.Id,
                                           CampaignId = o.CampaignId,
                                           MessageId = o.MessageId
                                       },
                                       CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                       MessageVersion = s2 == null || s2.Version == null ? "" : s2.Version.ToString()
                                   };

            return await campaignMessages.ToListAsync();
        }
    }
}