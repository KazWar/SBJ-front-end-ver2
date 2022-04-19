using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CampaignProcesses;
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
	[AbpAuthorize(AppPermissions.Pages_MessageContentTranslations)]
    public class MessageContentTranslationsAppService : RMSAppServiceBase, IMessageContentTranslationsAppService
    {
		 private readonly IRepository<MessageContentTranslation, long> _messageContentTranslationRepository;
		 private readonly IMessageContentTranslationsExcelExporter _messageContentTranslationsExcelExporter;
		 private readonly IRepository<Locale,long> _lookup_localeRepository;
		 private readonly IRepository<MessageComponentContent,long> _lookup_messageComponentContentRepository;
		 

		  public MessageContentTranslationsAppService(IRepository<MessageContentTranslation, long> messageContentTranslationRepository, IMessageContentTranslationsExcelExporter messageContentTranslationsExcelExporter , IRepository<Locale, long> lookup_localeRepository, IRepository<MessageComponentContent, long> lookup_messageComponentContentRepository) 
		  {
			_messageContentTranslationRepository = messageContentTranslationRepository;
			_messageContentTranslationsExcelExporter = messageContentTranslationsExcelExporter;
			_lookup_localeRepository = lookup_localeRepository;
		_lookup_messageComponentContentRepository = lookup_messageComponentContentRepository;
		
		  }

		 public async Task<PagedResultDto<GetMessageContentTranslationForViewDto>> GetAll(GetAllMessageContentTranslationsInput input)
         {
			
			var filteredMessageContentTranslations = _messageContentTranslationRepository.GetAll()
						.Include( e => e.LocaleFk)
						.Include( e => e.MessageComponentContentFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Content.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter),  e => e.Content == input.ContentFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocaleDescriptionFilter), e => e.LocaleFk != null && e.LocaleFk.Description == input.LocaleDescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MessageComponentContentContentFilter), e => e.MessageComponentContentFk != null && e.MessageComponentContentFk.Content == input.MessageComponentContentContentFilter);

			var pagedAndFilteredMessageContentTranslations = filteredMessageContentTranslations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var messageContentTranslations = from o in pagedAndFilteredMessageContentTranslations
                         join o1 in _lookup_localeRepository.GetAll() on o.LocaleId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_messageComponentContentRepository.GetAll() on o.MessageComponentContentId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetMessageContentTranslationForViewDto() {
							MessageContentTranslation = new MessageContentTranslationDto
							{
                                Content = o.Content,
                                Id = o.Id
							},
                         	LocaleDescription = s1 == null ? "" : s1.Description.ToString(),
                         	MessageComponentContentContent = s2 == null ? "" : s2.Content.ToString()
						};

            var totalCount = await filteredMessageContentTranslations.CountAsync();

            return new PagedResultDto<GetMessageContentTranslationForViewDto>(
                totalCount,
                await messageContentTranslations.ToListAsync()
            );
         }
		 
		 public async Task<GetMessageContentTranslationForViewDto> GetMessageContentTranslationForView(long id)
         {
            var messageContentTranslation = await _messageContentTranslationRepository.GetAsync(id);

            var output = new GetMessageContentTranslationForViewDto { MessageContentTranslation = ObjectMapper.Map<MessageContentTranslationDto>(messageContentTranslation) };

		    if (output.MessageContentTranslation.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.MessageContentTranslation.LocaleId);
                output.LocaleDescription = _lookupLocale.Description.ToString();
            }

		    if (output.MessageContentTranslation.MessageComponentContentId != null)
            {
                var _lookupMessageComponentContent = await _lookup_messageComponentContentRepository.FirstOrDefaultAsync((long)output.MessageContentTranslation.MessageComponentContentId);
                output.MessageComponentContentContent = _lookupMessageComponentContent.Content.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_MessageContentTranslations_Edit)]
		 public async Task<GetMessageContentTranslationForEditOutput> GetMessageContentTranslationForEdit(EntityDto<long> input)
         {
            var messageContentTranslation = await _messageContentTranslationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetMessageContentTranslationForEditOutput {MessageContentTranslation = ObjectMapper.Map<CreateOrEditMessageContentTranslationDto>(messageContentTranslation)};

		    if (output.MessageContentTranslation.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.MessageContentTranslation.LocaleId);
                output.LocaleDescription = _lookupLocale.Description.ToString();
            }

		    if (output.MessageContentTranslation.MessageComponentContentId != null)
            {
                var _lookupMessageComponentContent = await _lookup_messageComponentContentRepository.FirstOrDefaultAsync((long)output.MessageContentTranslation.MessageComponentContentId);
                output.MessageComponentContentContent = _lookupMessageComponentContent.Content.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditMessageContentTranslationDto input)
         {
            if(input.Id == null || input.Id == 0){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageContentTranslations_Create)]
		 protected virtual async Task Create(CreateOrEditMessageContentTranslationDto input)
         {
            var messageContentTranslation = ObjectMapper.Map<MessageContentTranslation>(input);

			
			if (AbpSession.TenantId != null)
			{
				messageContentTranslation.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _messageContentTranslationRepository.InsertAsync(messageContentTranslation);
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageContentTranslations_Edit)]
		 protected virtual async Task Update(CreateOrEditMessageContentTranslationDto input)
         {
            var messageContentTranslation = await _messageContentTranslationRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, messageContentTranslation);
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageContentTranslations_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _messageContentTranslationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetMessageContentTranslationsToExcel(GetAllMessageContentTranslationsForExcelInput input)
         {
			
			var filteredMessageContentTranslations = _messageContentTranslationRepository.GetAll()
						.Include( e => e.LocaleFk)
						.Include( e => e.MessageComponentContentFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Content.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter),  e => e.Content == input.ContentFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocaleDescriptionFilter), e => e.LocaleFk != null && e.LocaleFk.Description == input.LocaleDescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MessageComponentContentContentFilter), e => e.MessageComponentContentFk != null && e.MessageComponentContentFk.Content == input.MessageComponentContentContentFilter);

			var query = (from o in filteredMessageContentTranslations
                         join o1 in _lookup_localeRepository.GetAll() on o.LocaleId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_messageComponentContentRepository.GetAll() on o.MessageComponentContentId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetMessageContentTranslationForViewDto() { 
							MessageContentTranslation = new MessageContentTranslationDto
							{
                                Content = o.Content,
                                Id = o.Id
							},
                         	LocaleDescription = s1 == null ? "" : s1.Description.ToString(),
                         	MessageComponentContentContent = s2 == null ? "" : s2.Content.ToString()
						 });


            var messageContentTranslationListDtos = await query.ToListAsync();

            return _messageContentTranslationsExcelExporter.ExportToFile(messageContentTranslationListDtos);
         }

        [AbpAuthorize(AppPermissions.Pages_MessageContentTranslations)]
        public async Task<PagedResultDto<MessageContentTranslationLocaleLookupTableDto>> GetAllLocaleForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_localeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var localeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<MessageContentTranslationLocaleLookupTableDto>();
            foreach (var locale in localeList)
            {
                lookupTableDtoList.Add(new MessageContentTranslationLocaleLookupTableDto
                {
                    Id = locale.Id,
                    DisplayName = locale.Description?.ToString()
                });
            }

            return new PagedResultDto<MessageContentTranslationLocaleLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_MessageContentTranslations)]
        public async Task<PagedResultDto<MessageContentTranslationMessageComponentContentLookupTableDto>> GetAllMessageComponentContentForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_messageComponentContentRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Content.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var messageComponentContentList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<MessageContentTranslationMessageComponentContentLookupTableDto>();
            foreach (var messageComponentContent in messageComponentContentList)
            {
                lookupTableDtoList.Add(new MessageContentTranslationMessageComponentContentLookupTableDto
                {
                    Id = messageComponentContent.Id,
                    DisplayName = messageComponentContent.Content?.ToString()
                });
            }

            return new PagedResultDto<MessageContentTranslationMessageComponentContentLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<GetMessageContentTranslationForViewDto>> GetAllMessageContentTranslations()
        {
            var allMessageContentTranslations = _messageContentTranslationRepository.GetAll()
                        .Include(e => e.LocaleFk)
                        .Include(e => e.MessageComponentContentFk);
            
            var messageContentTranslations = from o in allMessageContentTranslations
                                             join o1 in _lookup_localeRepository.GetAll() on o.LocaleId equals o1.Id into j1
                                             from s1 in j1.DefaultIfEmpty()

                                             join o2 in _lookup_messageComponentContentRepository.GetAll() on o.MessageComponentContentId equals o2.Id into j2
                                             from s2 in j2.DefaultIfEmpty()

                                             select new GetMessageContentTranslationForViewDto()
                                             {
                                                 MessageContentTranslation = new MessageContentTranslationDto
                                                 {
                                                     Content = o.Content,
                                                     Id = o.Id,
                                                     LocaleId = o.LocaleId,
                                                     MessageComponentContentId = o.MessageComponentContentId
                                                 },
                                                 LocaleDescription = s1 == null ? "" : s1.Description.ToString(),
                                                 MessageComponentContentContent = s2 == null ? "" : s2.Content.ToString()
                                             };

            return await messageContentTranslations.ToListAsync();
        }
    }
}