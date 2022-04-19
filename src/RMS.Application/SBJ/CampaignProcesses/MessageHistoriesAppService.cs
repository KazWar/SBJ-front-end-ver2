using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.CampaignProcesses
{
    [AbpAuthorize(AppPermissions.Pages_MessageHistories)]
    public class MessageHistoriesAppService : RMSAppServiceBase, IMessageHistoriesAppService
    {
        private readonly IRepository<MessageHistory, long> _messageHistoryRepository;

        public MessageHistoriesAppService(IRepository<MessageHistory, long> messageHistoryRepository)
        {
            _messageHistoryRepository = messageHistoryRepository;

        }

        public async Task<PagedResultDto<GetMessageHistoryForViewDto>> GetAll(GetAllMessageHistoriesInput input)
        {

            var filteredMessageHistories = _messageHistoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Content.Contains(input.Filter) || e.MessageName.Contains(input.Filter) || e.Subject.Contains(input.Filter) || e.To.Contains(input.Filter))
                        .WhereIf(input.MinRegistrationIdFilter != null, e => e.RegistrationId >= input.MinRegistrationIdFilter)
                        .WhereIf(input.MaxRegistrationIdFilter != null, e => e.RegistrationId <= input.MaxRegistrationIdFilter)
                        .WhereIf(input.MinAbpUserIdFilter != null, e => e.AbpUserId >= input.MinAbpUserIdFilter)
                        .WhereIf(input.MaxAbpUserIdFilter != null, e => e.AbpUserId <= input.MaxAbpUserIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter), e => e.Content == input.ContentFilter)
                        .WhereIf(input.MinTimeStampFilter != null, e => e.TimeStamp >= input.MinTimeStampFilter)
                        .WhereIf(input.MaxTimeStampFilter != null, e => e.TimeStamp <= input.MaxTimeStampFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MessageNameFilter), e => e.MessageName == input.MessageNameFilter)
                        .WhereIf(input.MinMessageIdFilter != null, e => e.MessageId >= input.MinMessageIdFilter)
                        .WhereIf(input.MaxMessageIdFilter != null, e => e.MessageId <= input.MaxMessageIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubjectFilter), e => e.Subject == input.SubjectFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ToFilter), e => e.To == input.ToFilter);

            var pagedAndFilteredMessageHistories = filteredMessageHistories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var messageHistories = from o in pagedAndFilteredMessageHistories
                                   select new GetMessageHistoryForViewDto()
                                   {
                                       MessageHistory = new MessageHistoryDto
                                       {
                                           RegistrationId = o.RegistrationId,
                                           AbpUserId = o.AbpUserId,
                                           Content = o.Content,
                                           TimeStamp = o.TimeStamp,
                                           MessageName = o.MessageName,
                                           MessageId = o.MessageId,
                                           Subject = o.Subject,
                                           To = o.To,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredMessageHistories.CountAsync();

            return new PagedResultDto<GetMessageHistoryForViewDto>(
                totalCount,
                await messageHistories.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_MessageHistories_Edit)]
        public async Task<GetMessageHistoryForEditOutput> GetMessageHistoryForEdit(EntityDto<long> input)
        {
            var messageHistory = await _messageHistoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMessageHistoryForEditOutput { MessageHistory = ObjectMapper.Map<CreateOrEditMessageHistoryDto>(messageHistory) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMessageHistoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MessageHistories_Create)]
        protected virtual async Task Create(CreateOrEditMessageHistoryDto input)
        {
            var messageHistory = ObjectMapper.Map<MessageHistory>(input);

            if (AbpSession.TenantId != null)
            {
                messageHistory.TenantId = (int?)AbpSession.TenantId;
            }

            await _messageHistoryRepository.InsertAsync(messageHistory);
        }

        [AbpAuthorize(AppPermissions.Pages_MessageHistories_Edit)]
        protected virtual async Task Update(CreateOrEditMessageHistoryDto input)
        {
            var messageHistory = await _messageHistoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, messageHistory);
        }

        [AbpAuthorize(AppPermissions.Pages_MessageHistories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _messageHistoryRepository.DeleteAsync(input.Id);
        }
    }
}