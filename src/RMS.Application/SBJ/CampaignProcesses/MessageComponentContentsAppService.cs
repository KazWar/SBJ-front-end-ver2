using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CampaignProcesses.Exporting;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Runtime.Validation;
using System.Collections.Generic;

namespace RMS.SBJ.CampaignProcesses
{
    [AbpAuthorize(AppPermissions.Pages_MessageComponentContents)]
    public class MessageComponentContentsAppService : RMSAppServiceBase, IMessageComponentContentsAppService
    {
        private readonly IRepository<MessageComponentContent, long> _messageComponentContentRepository;
        private readonly IMessageComponentContentsExcelExporter _messageComponentContentsExcelExporter;
        private readonly IRepository<MessageComponent, long> _lookup_messageComponentRepository;
        private readonly IRepository<CampaignTypeEventRegistrationStatus, long> _lookup_campaignTypeEventRegistrationStatusRepository;


        public MessageComponentContentsAppService(IRepository<MessageComponentContent, long> messageComponentContentRepository, IMessageComponentContentsExcelExporter messageComponentContentsExcelExporter, IRepository<MessageComponent, long> lookup_messageComponentRepository, IRepository<CampaignTypeEventRegistrationStatus, long> lookup_campaignTypeEventRegistrationStatusRepository)
        {
            _messageComponentContentRepository = messageComponentContentRepository;
            _messageComponentContentsExcelExporter = messageComponentContentsExcelExporter;
            _lookup_messageComponentRepository = lookup_messageComponentRepository;
            _lookup_campaignTypeEventRegistrationStatusRepository = lookup_campaignTypeEventRegistrationStatusRepository;

        }

        public async Task<PagedResultDto<GetMessageComponentContentForViewDto>> GetAll(GetAllMessageComponentContentsInput input)
        {

            var filteredMessageComponentContents = _messageComponentContentRepository.GetAll()
                        .Include(e => e.MessageComponentFk)
                        .Include(e => e.CampaignTypeEventRegistrationStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Content.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter), e => e.Content == input.ContentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MessageComponentIsActiveFilter), e => e.MessageComponentFk != null && e.MessageComponentFk.IsActive.ToString().ToLower() == input.MessageComponentIsActiveFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeEventRegistrationStatusSortOrderFilter), e => e.CampaignTypeEventRegistrationStatusFk != null && e.CampaignTypeEventRegistrationStatusFk.SortOrder.ToString().ToLower() == input.CampaignTypeEventRegistrationStatusSortOrderFilter.ToLower().Trim());

            var pagedAndFilteredMessageComponentContents = filteredMessageComponentContents
                .OrderBy(input.Sorting ?? "id asc");
                // .PageBy(input);

            var messageComponentContents = from o in pagedAndFilteredMessageComponentContents
                                           join o1 in _lookup_messageComponentRepository.GetAll() on o.MessageComponentId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()

                                           join o2 in _lookup_campaignTypeEventRegistrationStatusRepository.GetAll() on o.CampaignTypeEventRegistrationStatusId equals o2.Id into j2
                                           from s2 in j2.DefaultIfEmpty()

                                           select new GetMessageComponentContentForViewDto()
                                           {
                                               MessageComponentContent = new MessageComponentContentDto
                                               {
                                                   Id = o.Id,
                                                   CampaignTypeEventRegistrationStatusId = o.CampaignTypeEventRegistrationStatusId,
                                                   MessageComponentId = o.MessageComponentId,
                                                   Content = o.Content
                                               },
                                               MessageComponentIsActive = s1 == null ? "" : s1.IsActive.ToString(),
                                               CampaignTypeEventRegistrationStatusSortOrder = s2 == null ? "" : s2.SortOrder.ToString()
                                           };

            var totalCount = await filteredMessageComponentContents.CountAsync();

            return new PagedResultDto<GetMessageComponentContentForViewDto>(
                totalCount,
                await messageComponentContents.ToListAsync()
            );
        }

        public async Task<GetMessageComponentContentForViewDto> GetMessageComponentContentForView(long id)
        {
            var messageComponentContent = await _messageComponentContentRepository.GetAsync(id);

            var output = new GetMessageComponentContentForViewDto { MessageComponentContent = ObjectMapper.Map<MessageComponentContentDto>(messageComponentContent) };

            if (output.MessageComponentContent.MessageComponentId != null)
            {
                var _lookupMessageComponent = await _lookup_messageComponentRepository.FirstOrDefaultAsync((long)output.MessageComponentContent.MessageComponentId);
                output.MessageComponentIsActive = _lookupMessageComponent.IsActive.ToString();
            }

            if (output.MessageComponentContent.CampaignTypeEventRegistrationStatusId != null)
            {
                var _lookupCampaignTypeEventRegistrationStatus = await _lookup_campaignTypeEventRegistrationStatusRepository.FirstOrDefaultAsync((long)output.MessageComponentContent.CampaignTypeEventRegistrationStatusId);
                output.CampaignTypeEventRegistrationStatusSortOrder = _lookupCampaignTypeEventRegistrationStatus.SortOrder.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MessageComponentContents_Edit)]
        public async Task<GetMessageComponentContentForEditOutput> GetMessageComponentContentForEdit(EntityDto<long> input)
        {
            var messageComponentContent = await _messageComponentContentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMessageComponentContentForEditOutput { MessageComponentContent = ObjectMapper.Map<CreateOrEditMessageComponentContentDto>(messageComponentContent) };

            if (output.MessageComponentContent.MessageComponentId != null)
            {
                var _lookupMessageComponent = await _lookup_messageComponentRepository.FirstOrDefaultAsync((long)output.MessageComponentContent.MessageComponentId);
                output.MessageComponentIsActive = _lookupMessageComponent.IsActive.ToString();
            }

            if (output.MessageComponentContent.CampaignTypeEventRegistrationStatusId != null)
            {
                var _lookupCampaignTypeEventRegistrationStatus = await _lookup_campaignTypeEventRegistrationStatusRepository.FirstOrDefaultAsync((long)output.MessageComponentContent.CampaignTypeEventRegistrationStatusId);
                output.CampaignTypeEventRegistrationStatusSortOrder = _lookupCampaignTypeEventRegistrationStatus.SortOrder.ToString();
            }

            return output;
        }

        [DisableValidation]
        public async Task CreateOrEdit(CreateOrEditMessageComponentContentDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_MessageComponentContents_Create)]
        protected virtual async Task Create(CreateOrEditMessageComponentContentDto input)
        {
            var messageComponentContent = ObjectMapper.Map<MessageComponentContent>(input);


            if (AbpSession.TenantId != null)
            {
                messageComponentContent.TenantId = (int?)AbpSession.TenantId;
            }


            await _messageComponentContentRepository.InsertAsync(messageComponentContent);
        }

        [AbpAuthorize(AppPermissions.Pages_MessageComponentContents_Edit)]
        protected virtual async Task Update(CreateOrEditMessageComponentContentDto input)
        {
            var messageComponentContent = await _messageComponentContentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, messageComponentContent);
        }

        [AbpAuthorize(AppPermissions.Pages_MessageComponentContents_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _messageComponentContentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMessageComponentContentsToExcel(GetAllMessageComponentContentsForExcelInput input)
        {


            var filteredMessageComponentContents = _messageComponentContentRepository.GetAll()
                        .Include(e => e.MessageComponentFk)
                        .Include(e => e.CampaignTypeEventRegistrationStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Content.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter), e => e.Content == input.ContentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MessageComponentIsActiveFilter), e => e.MessageComponentFk != null && e.MessageComponentFk.IsActive.ToString().ToLower() == input.MessageComponentIsActiveFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CampaignTypeEventRegistrationStatusSortOrderFilter), e => e.CampaignTypeEventRegistrationStatusFk != null && e.CampaignTypeEventRegistrationStatusFk.SortOrder.ToString().ToLower() == input.CampaignTypeEventRegistrationStatusSortOrderFilter.ToLower().Trim());

            var query = (from o in filteredMessageComponentContents
                         join o1 in _lookup_messageComponentRepository.GetAll() on o.MessageComponentId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_campaignTypeEventRegistrationStatusRepository.GetAll() on o.CampaignTypeEventRegistrationStatusId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetMessageComponentContentForViewDto()
                         {
                             MessageComponentContent = new MessageComponentContentDto
                             {
                                 Content = o.Content,
                                 Id = o.Id
                             },
                             MessageComponentIsActive = s1 == null ? "" : s1.IsActive.ToString(),
                             CampaignTypeEventRegistrationStatusSortOrder = s2 == null ? "" : s2.SortOrder.ToString()
                         });


            var messageComponentContentListDtos = await query.ToListAsync();

            return _messageComponentContentsExcelExporter.ExportToFile(messageComponentContentListDtos);
        }

        public async Task<List<GetMessageComponentContentForViewDto>> GetAllMessageComponentContents()
        {
            var allMessageComponentContents = _messageComponentContentRepository.GetAll()
                        .Include(e => e.MessageComponentFk).ThenInclude(e => e.MessageTypeFk)
                        .Include(e => e.MessageComponentFk).ThenInclude(e => e.MessageComponentTypeFk)
                        .Include(e => e.CampaignTypeEventRegistrationStatusFk);

            var messageComponentContents = from o in allMessageComponentContents
                                           join o1 in _lookup_messageComponentRepository.GetAll() on o.MessageComponentId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()

                                           join o2 in _lookup_campaignTypeEventRegistrationStatusRepository.GetAll() on o.CampaignTypeEventRegistrationStatusId equals o2.Id into j2
                                           from s2 in j2.DefaultIfEmpty()

                                           select new GetMessageComponentContentForViewDto()
                                           {
                                               MessageComponentContent = new MessageComponentContentDto
                                               {
                                                   Id = o.Id,
                                                   CampaignTypeEventRegistrationStatusId = o.CampaignTypeEventRegistrationStatusId,
                                                   MessageComponentId = o.MessageComponentId,
                                                   Content = o.Content,
                                                   MessageType = o.MessageComponentFk.MessageTypeFk.Name,
                                                   MessageComponentType = o.MessageComponentFk.MessageComponentTypeFk.Name
                                               },
                                               MessageComponentIsActive = s1 == null ? "" : s1.IsActive.ToString(),
                                               CampaignTypeEventRegistrationStatusSortOrder = s2 == null ? "" : s2.SortOrder.ToString(),
                                               MessageType = o.MessageComponentFk.MessageTypeFk.Name,
                                               MessageComponentType = o.MessageComponentFk.MessageComponentTypeFk.Name
                                           };

            return await messageComponentContents.ToListAsync();
        }

    }
}