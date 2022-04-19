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
    [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineStatuses)]
    public class HandlingBatchLineStatusesAppService : RMSAppServiceBase, IHandlingBatchLineStatusesAppService
    {
        private readonly IRepository<HandlingBatchLineStatus, long> _handlingBatchLineStatusRepository;

        public HandlingBatchLineStatusesAppService(IRepository<HandlingBatchLineStatus, long> handlingBatchLineStatusRepository)
        {
            _handlingBatchLineStatusRepository = handlingBatchLineStatusRepository;

        }

        public async Task<GetHandlingBatchLineStatusForViewDto> GetById(long id)
        {
            var handlingBatchLineStatus = await _handlingBatchLineStatusRepository.SingleAsync(x => x.Id == id);

            return new GetHandlingBatchLineStatusForViewDto
            {
                HandlingBatchLineStatus = new HandlingBatchLineStatusDto
                {
                    Id = handlingBatchLineStatus.Id,
                    StatusCode = handlingBatchLineStatus.StatusCode,
                    StatusDescription = handlingBatchLineStatus.StatusDescription
                }
            };
        }

        public async Task<GetHandlingBatchLineStatusForViewDto> GetByStatusCode(string statusCode)
        {
            var handlingBatchLineStatus = await _handlingBatchLineStatusRepository.SingleAsync(x => x.StatusCode == statusCode);

            return new GetHandlingBatchLineStatusForViewDto
            {
                HandlingBatchLineStatus = new HandlingBatchLineStatusDto
                {
                    Id = handlingBatchLineStatus.Id,
                    StatusCode = handlingBatchLineStatus.StatusCode,
                    StatusDescription = handlingBatchLineStatus.StatusDescription
                }
            };
        }

        public async Task<PagedResultDto<GetHandlingBatchLineStatusForViewDto>> GetAll(GetAllHandlingBatchLineStatusesInput input)
        {

            var filteredHandlingBatchLineStatuses = _handlingBatchLineStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.StatusCode.Contains(input.Filter) || e.StatusDescription.Contains(input.Filter));

            var pagedAndFilteredHandlingBatchLineStatuses = filteredHandlingBatchLineStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var handlingBatchLineStatuses = from o in pagedAndFilteredHandlingBatchLineStatuses
                                            select new GetHandlingBatchLineStatusForViewDto()
                                            {
                                                HandlingBatchLineStatus = new HandlingBatchLineStatusDto
                                                {
                                                    Id = o.Id
                                                }
                                            };

            var totalCount = await filteredHandlingBatchLineStatuses.CountAsync();

            return new PagedResultDto<GetHandlingBatchLineStatusForViewDto>(
                totalCount,
                await handlingBatchLineStatuses.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineStatuses_Edit)]
        public async Task<GetHandlingBatchLineStatusForEditOutput> GetHandlingBatchLineStatusForEdit(EntityDto<long> input)
        {
            var handlingBatchLineStatus = await _handlingBatchLineStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHandlingBatchLineStatusForEditOutput { HandlingBatchLineStatus = ObjectMapper.Map<CreateOrEditHandlingBatchLineStatusDto>(handlingBatchLineStatus) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHandlingBatchLineStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineStatuses_Create)]
        protected virtual async Task Create(CreateOrEditHandlingBatchLineStatusDto input)
        {
            var handlingBatchLineStatus = ObjectMapper.Map<HandlingBatchLineStatus>(input);

            if (AbpSession.TenantId != null)
            {
                handlingBatchLineStatus.TenantId = (int?)AbpSession.TenantId;
            }

            await _handlingBatchLineStatusRepository.InsertAsync(handlingBatchLineStatus);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditHandlingBatchLineStatusDto input)
        {
            var handlingBatchLineStatus = await _handlingBatchLineStatusRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, handlingBatchLineStatus);
        }

        [AbpAuthorize(AppPermissions.Pages_HandlingBatchLineStatuses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _handlingBatchLineStatusRepository.DeleteAsync(input.Id);
        }
    }
}