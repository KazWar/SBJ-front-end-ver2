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
    [AbpAuthorize(AppPermissions.Pages_RejectionReasons)]
    public class RejectionReasonsAppService : RMSAppServiceBase, IRejectionReasonsAppService
    {
        private readonly IRepository<RejectionReason, long> _rejectionReasonRepository;
        private readonly IRejectionReasonsExcelExporter _rejectionReasonsExcelExporter;

        public RejectionReasonsAppService(IRepository<RejectionReason, long> rejectionReasonRepository, IRejectionReasonsExcelExporter rejectionReasonsExcelExporter)
        {
            _rejectionReasonRepository = rejectionReasonRepository;
            _rejectionReasonsExcelExporter = rejectionReasonsExcelExporter;

        }

        public async Task<IEnumerable<GetRejectionReasonForViewDto>> GetAll()
        {
            var rejectionReasons = await _rejectionReasonRepository.GetAll().Select(x => new GetRejectionReasonForViewDto {
                RejectionReason = new RejectionReasonDto { Id = x.Id, Description = x.Description }
            }).ToListAsync();

            return rejectionReasons;
        }

        public async Task<IEnumerable<GetRejectionReasonForViewDto>> GetAllForIncomplete()
        {
            var rejectionReasons = await _rejectionReasonRepository.GetAll().Where(r => r.IsIncompleteReason == true).Select(x => new GetRejectionReasonForViewDto
            {
                RejectionReason = new RejectionReasonDto { Id = x.Id, Description = x.Description }
            }).ToListAsync();

            return rejectionReasons;
        }

        public async Task<IEnumerable<GetRejectionReasonForViewDto>> GetAllForRejection()
        {
            var rejectionReasons = await _rejectionReasonRepository.GetAll().Where(r => r.IsIncompleteReason == false).Select(x => new GetRejectionReasonForViewDto
            {
                RejectionReason = new RejectionReasonDto { Id = x.Id, Description = x.Description }
            }).ToListAsync();

            return rejectionReasons;
        }

        public async Task<PagedResultDto<GetRejectionReasonForViewDto>> GetAll(GetAllRejectionReasonsInput input)
        {

            var filteredRejectionReasons = _rejectionReasonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredRejectionReasons = filteredRejectionReasons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var rejectionReasons = from o in pagedAndFilteredRejectionReasons
                                   select new GetRejectionReasonForViewDto()
                                   {
                                       RejectionReason = new RejectionReasonDto
                                       {
                                           Description = o.Description,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredRejectionReasons.CountAsync();

            return new PagedResultDto<GetRejectionReasonForViewDto>(
                totalCount,
                await rejectionReasons.ToListAsync()
            );
        }

        public async Task<PagedResultDto<GetRejectionReasonForViewDto>> GetAllForIncomplete(GetAllRejectionReasonsInput input)
        {

            var filteredRejectionReasons = _rejectionReasonRepository.GetAll().Where(r => r.IsIncompleteReason == true)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredRejectionReasons = filteredRejectionReasons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var rejectionReasons = from o in pagedAndFilteredRejectionReasons
                                   select new GetRejectionReasonForViewDto()
                                   {
                                       RejectionReason = new RejectionReasonDto
                                       {
                                           Description = o.Description,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredRejectionReasons.CountAsync();

            return new PagedResultDto<GetRejectionReasonForViewDto>(
                totalCount,
                await rejectionReasons.ToListAsync()
            );
        }

        public async Task<PagedResultDto<GetRejectionReasonForViewDto>> GetAllForRejection(GetAllRejectionReasonsInput input)
        {

            var filteredRejectionReasons = _rejectionReasonRepository.GetAll().Where(r => r.IsIncompleteReason == false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredRejectionReasons = filteredRejectionReasons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var rejectionReasons = from o in pagedAndFilteredRejectionReasons
                                   select new GetRejectionReasonForViewDto()
                                   {
                                       RejectionReason = new RejectionReasonDto
                                       {
                                           Description = o.Description,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredRejectionReasons.CountAsync();

            return new PagedResultDto<GetRejectionReasonForViewDto>(
                totalCount,
                await rejectionReasons.ToListAsync()
            );
        }

        public async Task<GetRejectionReasonForViewDto> GetRejectionReasonForView(long id)
        {
            var rejectionReason = await _rejectionReasonRepository.GetAsync(id);

            var output = new GetRejectionReasonForViewDto { RejectionReason = ObjectMapper.Map<RejectionReasonDto>(rejectionReason) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_RejectionReasons_Edit)]
        public async Task<GetRejectionReasonForEditOutput> GetRejectionReasonForEdit(EntityDto<long> input)
        {
            var rejectionReason = await _rejectionReasonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRejectionReasonForEditOutput { RejectionReason = ObjectMapper.Map<CreateOrEditRejectionReasonDto>(rejectionReason) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRejectionReasonDto input)
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

        [AbpAuthorize(AppPermissions.Pages_RejectionReasons_Create)]
        protected virtual async Task Create(CreateOrEditRejectionReasonDto input)
        {
            var rejectionReason = ObjectMapper.Map<RejectionReason>(input);

            if (AbpSession.TenantId != null)
            {
                rejectionReason.TenantId = (int?)AbpSession.TenantId;
            }

            await _rejectionReasonRepository.InsertAsync(rejectionReason);
        }

        [AbpAuthorize(AppPermissions.Pages_RejectionReasons_Edit)]
        protected virtual async Task Update(CreateOrEditRejectionReasonDto input)
        {
            var rejectionReason = await _rejectionReasonRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, rejectionReason);
        }

        [AbpAuthorize(AppPermissions.Pages_RejectionReasons_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _rejectionReasonRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRejectionReasonsToExcel(GetAllRejectionReasonsForExcelInput input)
        {

            var filteredRejectionReasons = _rejectionReasonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredRejectionReasons
                         select new GetRejectionReasonForViewDto()
                         {
                             RejectionReason = new RejectionReasonDto
                             {
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var rejectionReasonListDtos = await query.ToListAsync();

            return _rejectionReasonsExcelExporter.ExportToFile(rejectionReasonListDtos);
        }

        public GetRejectionReasonForViewDto GetRejectionReason(long id)
        {
            var rejectionReasonEntity = _rejectionReasonRepository.Get(id);
            var rejectionReasonDto = new GetRejectionReasonForViewDto
            {
                RejectionReason = new RejectionReasonDto { Id = rejectionReasonEntity.Id, Description = rejectionReasonEntity.Description }
            };

            return rejectionReasonDto;
        }
    }
}