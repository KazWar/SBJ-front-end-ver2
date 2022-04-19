using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.HandlingBatch.Dtos;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.HandlingBatch
{
    [AbpAuthorize(AppPermissions.Pages_HandlingBatchStatuses)]
    public class HandlingBatchStatusesAppService : RMSAppServiceBase, IHandlingBatchStatusesAppService
    {
        private readonly IRepository<HandlingBatchStatus, long> _handlingBatchStatusRepository;

        public HandlingBatchStatusesAppService(IRepository<HandlingBatchStatus, long> handlingBatchStatusRepository)
        {
            _handlingBatchStatusRepository = handlingBatchStatusRepository;

        }

        public async Task<GetHandlingBatchStatusForViewDto> GetById(long id)
        {
            var handlingBatchStatus = await _handlingBatchStatusRepository.SingleAsync(x => x.Id == id);

            return new GetHandlingBatchStatusForViewDto
            {
                HandlingBatchStatus = new HandlingBatchStatusDto
                {
                    Id = handlingBatchStatus.Id,
                    StatusCode = handlingBatchStatus.StatusCode,
                    StatusDescription = handlingBatchStatus.StatusDescription
                }
            };
        }

        public async Task<GetHandlingBatchStatusForViewDto> GetByStatusCode(string statusCode)
        {
            var handlingBatchStatus = await _handlingBatchStatusRepository.SingleAsync(x => x.StatusCode == statusCode);

            return new GetHandlingBatchStatusForViewDto
            {
                HandlingBatchStatus = new HandlingBatchStatusDto
                {
                    Id = handlingBatchStatus.Id,
                    StatusCode = handlingBatchStatus.StatusCode,
                    StatusDescription = handlingBatchStatus.StatusDescription
                }
            };
        }

        public async Task<PagedResultDto<GetHandlingBatchStatusForViewDto>> GetAll(GetAllHandlingBatchStatusesInput input)
        {

            var filteredHandlingBatchStatuses = _handlingBatchStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.StatusCode.Contains(input.Filter) || e.StatusDescription.Contains(input.Filter));

            var pagedAndFilteredHandlingBatchStatuses = filteredHandlingBatchStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var handlingBatchStatuses = from o in pagedAndFilteredHandlingBatchStatuses
                                        select new GetHandlingBatchStatusForViewDto()
                                        {
                                            HandlingBatchStatus = new HandlingBatchStatusDto
                                            {
                                                Id = o.Id,
                                                StatusCode = o.StatusCode,
                                                StatusDescription = o.StatusDescription
                                            }
                                        };

            var totalCount = await filteredHandlingBatchStatuses.CountAsync();

            return new PagedResultDto<GetHandlingBatchStatusForViewDto>(
                totalCount,
                await handlingBatchStatuses.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchStatuses_Edit)]
        public async Task<GetHandlingBatchStatusForEditOutput> GetHandlingBatchStatusForEdit(EntityDto<long> input)
        {
            var handlingBatchStatus = await _handlingBatchStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHandlingBatchStatusForEditOutput { HandlingBatchStatus = ObjectMapper.Map<CreateOrEditHandlingBatchStatusDto>(handlingBatchStatus) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHandlingBatchStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchStatuses_Create)]
        protected virtual async Task Create(CreateOrEditHandlingBatchStatusDto input)
        {
            var handlingBatchStatus = ObjectMapper.Map<HandlingBatchStatus>(input);

            if (AbpSession.TenantId != null)
            {
                handlingBatchStatus.TenantId = (int?)AbpSession.TenantId;
            }

            await _handlingBatchStatusRepository.InsertAsync(handlingBatchStatus);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditHandlingBatchStatusDto input)
        {
            var handlingBatchStatus = await _handlingBatchStatusRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, handlingBatchStatus);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchStatuses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _handlingBatchStatusRepository.DeleteAsync(input.Id);
        }
    }
}