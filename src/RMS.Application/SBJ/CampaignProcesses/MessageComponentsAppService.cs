using RMS.SBJ.CodeTypeTables;
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
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.CampaignProcesses
{
    [AbpAuthorize(AppPermissions.Pages_MessageComponents)]
    public class MessageComponentsAppService : RMSAppServiceBase, IMessageComponentsAppService
    {
        private readonly IRepository<MessageComponent, long> _messageComponentRepository;
        private readonly IMessageComponentsExcelExporter _messageComponentsExcelExporter;
        private readonly IRepository<MessageType, long> _lookup_messageTypeRepository;
        private readonly IRepository<MessageComponentType, long> _lookup_messageComponentTypeRepository;

        public MessageComponentsAppService(IRepository<MessageComponent, long> messageComponentRepository, IMessageComponentsExcelExporter messageComponentsExcelExporter, IRepository<MessageType, long> lookup_messageTypeRepository, IRepository<MessageComponentType, long> lookup_messageComponentTypeRepository)
        {
            _messageComponentRepository = messageComponentRepository;
            _messageComponentsExcelExporter = messageComponentsExcelExporter;
            _lookup_messageTypeRepository = lookup_messageTypeRepository;
            _lookup_messageComponentTypeRepository = lookup_messageComponentTypeRepository;
        }

        public async Task<PagedResultDto<GetMessageComponentForViewDto>> GetAll(GetAllMessageComponentsInput input)
        {
            var filteredMessageComponents = _messageComponentRepository.GetAll()
                .Include(e => e.MessageTypeFk)
                .Include(e => e.MessageComponentTypeFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                .WhereIf(input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                .WhereIf(!string.IsNullOrWhiteSpace(input.MessageTypeNameFilter), e => e.MessageTypeFk != null && e.MessageTypeFk.Name == input.MessageTypeNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.MessageComponentTypeNameFilter), e => e.MessageComponentTypeFk != null && e.MessageComponentTypeFk.Name == input.MessageComponentTypeNameFilter);

            var pagedAndFilteredMessageComponents = filteredMessageComponents
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var messageComponents = 
                from o in pagedAndFilteredMessageComponents
                join o1 in _lookup_messageTypeRepository.GetAll() on o.MessageTypeId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()

                join o2 in _lookup_messageComponentTypeRepository.GetAll() on o.MessageComponentTypeId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()

                select new GetMessageComponentForViewDto()
                {
                    MessageComponent = new MessageComponentDto
                    {
                        Id = o.Id,
                        MessageTypeId = o.MessageTypeId,
                        MessageComponentTypeId = o.MessageComponentTypeId,
                        IsActive = o.IsActive
                    },
                    MessageTypeName = s1 == null ? "" : s1.Name.ToString(),
                    MessageComponentTypeName = s2 == null ? "" : s2.Name.ToString()
                };

            var totalCount = await filteredMessageComponents.CountAsync();

            return new PagedResultDto<GetMessageComponentForViewDto>(
                totalCount,
                await messageComponents.ToListAsync()
            );
        }

        public async Task<PagedResultDto<GetMessageComponentForViewDto>> GetMessageComponentsByMessageTypeId(long messageTypeId, string messageTypeName, long messageId)
        {
            if (messageTypeId <= 0) return null;
            var templateMessageTypeId = _lookup_messageTypeRepository.GetAll().Where(mt => mt.Name == messageTypeName && mt.IsActive).FirstOrDefault().Id;
            List<MessageComponent> messageComponents = await _messageComponentRepository.GetAll().Where(mcomponent => mcomponent.IsActive && mcomponent.MessageTypeId == templateMessageTypeId).ToListAsync();
            int totalCount = messageComponents.Count();

            var messageComponentForViewList = new List<GetMessageComponentForViewDto>();
            foreach (var messageComponent in messageComponents)
            {
                if (messageComponent == null) continue;

                MessageType messageType = _lookup_messageTypeRepository.Get(messageComponent.MessageTypeId);
                MessageComponentType messageComponentType = _lookup_messageComponentTypeRepository.Get(messageComponent.MessageComponentTypeId);

                if (messageType == null) continue;
                if (messageComponentType == null) continue;

                messageComponentForViewList.Add(new GetMessageComponentForViewDto
                {
                    MessageComponent = new MessageComponentDto
                    {
                        Id = messageComponent.Id,
                        IsActive = messageComponent.IsActive,
                    },
                    MessageTypeName = messageType.Name ?? "",
                    MessageComponentTypeName = messageComponentType.Name ?? ""
                });
            }

            return new PagedResultDto<GetMessageComponentForViewDto>(
                totalCount,
                messageComponentForViewList
            );
        }

        public async Task<GetMessageComponentForViewDto> GetMessageComponentForView(long id)
        {
            var messageComponent = await _messageComponentRepository.GetAsync(id);

            var output = new GetMessageComponentForViewDto { MessageComponent = ObjectMapper.Map<MessageComponentDto>(messageComponent) };

            if (output.MessageComponent.MessageTypeId != null)
            {
                var _lookupMessageType = await _lookup_messageTypeRepository.FirstOrDefaultAsync((long)output.MessageComponent.MessageTypeId);
                output.MessageTypeName = _lookupMessageType.Name.ToString();
            }

            if (output.MessageComponent.MessageComponentTypeId != null)
            {
                var _lookupMessageComponentType = await _lookup_messageComponentTypeRepository.FirstOrDefaultAsync((long)output.MessageComponent.MessageComponentTypeId);
                output.MessageComponentTypeName = _lookupMessageComponentType.Name.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MessageComponents_Edit)]
        public async Task<GetMessageComponentForEditOutput> GetMessageComponentForEdit(EntityDto<long> input)
        {
            var messageComponent = await _messageComponentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMessageComponentForEditOutput { MessageComponent = ObjectMapper.Map<CreateOrEditMessageComponentDto>(messageComponent) };

            if (output.MessageComponent.MessageTypeId != null)
            {
                var _lookupMessageType = await _lookup_messageTypeRepository.FirstOrDefaultAsync((long)output.MessageComponent.MessageTypeId);
                output.MessageTypeName = _lookupMessageType.Name.ToString();
            }

            if (output.MessageComponent.MessageComponentTypeId != null)
            {
                var _lookupMessageComponentType = await _lookup_messageComponentTypeRepository.FirstOrDefaultAsync((long)output.MessageComponent.MessageComponentTypeId);
                output.MessageComponentTypeName = _lookupMessageComponentType.Name.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMessageComponentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MessageComponents_Create)]
        protected virtual async Task Create(CreateOrEditMessageComponentDto input)
        {
            var messageComponent = ObjectMapper.Map<MessageComponent>(input);


            if (AbpSession.TenantId != null)
            {
                messageComponent.TenantId = (int?)AbpSession.TenantId;
            }


            await _messageComponentRepository.InsertAsync(messageComponent);
        }

        [AbpAuthorize(AppPermissions.Pages_MessageComponents_Edit)]
        protected virtual async Task Update(CreateOrEditMessageComponentDto input)
        {
            var messageComponent = await _messageComponentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, messageComponent);
        }

        [AbpAuthorize(AppPermissions.Pages_MessageComponents_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _messageComponentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMessageComponentsToExcel(GetAllMessageComponentsForExcelInput input)
        {

            var filteredMessageComponents = _messageComponentRepository.GetAll()
                        .Include(e => e.MessageTypeFk)
                        .Include(e => e.MessageComponentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MessageTypeNameFilter), e => e.MessageTypeFk != null && e.MessageTypeFk.Name == input.MessageTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MessageComponentTypeNameFilter), e => e.MessageComponentTypeFk != null && e.MessageComponentTypeFk.Name == input.MessageComponentTypeNameFilter);

            var query = 
                (from o in filteredMessageComponents
                    join o1 in _lookup_messageTypeRepository.GetAll() on o.MessageTypeId equals o1.Id into j1
                    from s1 in j1.DefaultIfEmpty()

                    join o2 in _lookup_messageComponentTypeRepository.GetAll() on o.MessageComponentTypeId equals o2.Id into j2
                    from s2 in j2.DefaultIfEmpty()

                    select new GetMessageComponentForViewDto()
                    {
                        MessageComponent = new MessageComponentDto
                        {
                            IsActive = o.IsActive,
                            Id = o.Id
                        },
                        MessageTypeName = s1 == null ? "" : s1.Name.ToString(),
                        MessageComponentTypeName = s2 == null ? "" : s2.Name.ToString()
                    }
                );

            var messageComponentListDtos = await query.ToListAsync();

            return _messageComponentsExcelExporter.ExportToFile(messageComponentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_MessageComponents)]
        public async Task<PagedResultDto<MessageComponentMessageTypeLookupTableDto>> GetAllMessageTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_messageTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var messageTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<MessageComponentMessageTypeLookupTableDto>();
            foreach (var messageType in messageTypeList)
            {
                lookupTableDtoList.Add(new MessageComponentMessageTypeLookupTableDto
                {
                    Id = messageType.Id,
                    DisplayName = messageType.Name?.ToString()
                });
            }

            return new PagedResultDto<MessageComponentMessageTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_MessageComponents)]
        public async Task<PagedResultDto<MessageComponentMessageComponentTypeLookupTableDto>> GetAllMessageComponentTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_messageComponentTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var messageComponentTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<MessageComponentMessageComponentTypeLookupTableDto>();
            foreach (var messageComponentType in messageComponentTypeList)
            {
                lookupTableDtoList.Add(new MessageComponentMessageComponentTypeLookupTableDto
                {
                    Id = messageComponentType.Id,
                    DisplayName = messageComponentType.Name?.ToString()
                });
            }

            return new PagedResultDto<MessageComponentMessageComponentTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<PagedResultDto<GetMessageComponentForViewDto>> GetAllMessageComponents()
        {
            var allMessageComponents = _messageComponentRepository.GetAll()
                .Include(e => e.MessageTypeFk)
                .Include(e => e.MessageComponentTypeFk);

            var messageComponents =
                from o in allMessageComponents
                join o1 in _lookup_messageTypeRepository.GetAll() on o.MessageTypeId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()

                join o2 in _lookup_messageComponentTypeRepository.GetAll() on o.MessageComponentTypeId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()

                select new GetMessageComponentForViewDto()
                {
                    MessageComponent = new MessageComponentDto
                    {
                        Id = o.Id,
                        MessageTypeId = o.MessageTypeId,
                        MessageComponentTypeId = o.MessageComponentTypeId,
                        IsActive = o.IsActive
                    },
                    MessageTypeName = s1 == null ? "" : s1.Name.ToString(),
                    MessageComponentTypeName = s2 == null ? "" : s2.Name.ToString()
                };

            var totalCount = await allMessageComponents.CountAsync();

            return new PagedResultDto<GetMessageComponentForViewDto>(
                totalCount,
                await messageComponents.ToListAsync()
            );
        }
    }
}