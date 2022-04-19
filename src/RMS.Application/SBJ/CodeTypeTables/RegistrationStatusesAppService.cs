using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CodeTypeTables.Exporting;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.CodeTypeTables
{
    [AbpAuthorize(AppPermissions.Pages_RegistrationStatuses)]
    public class RegistrationStatusesAppService : RMSAppServiceBase, IRegistrationStatusesAppService
    {
        private readonly IRepository<RegistrationStatus, long> _registrationStatusRepository;
        private readonly IRegistrationStatusesExcelExporter _registrationStatusesExcelExporter;
        
        public RegistrationStatusesAppService(IRepository<RegistrationStatus, long> registrationStatusRepository,
            IRegistrationStatusesExcelExporter registrationStatusesExcelExporter)
        {
            _registrationStatusRepository = registrationStatusRepository;
            _registrationStatusesExcelExporter = registrationStatusesExcelExporter;
        }

        public async Task<GetRegistrationStatusForViewDto> GetByStatusCode(string statusCode)
        {
            var registrationStatus =
                await _registrationStatusRepository.SingleAsync(x => x.StatusCode == statusCode);

            return new GetRegistrationStatusForViewDto() 
            {
                RegistrationStatus = new RegistrationStatusDto()
                {
                    Id = registrationStatus.Id,
                    Description = registrationStatus.Description,
                    StatusCode = registrationStatus.StatusCode
                }
            };
        }
        
        public async Task<PagedResultDto<GetRegistrationStatusForViewDto>> GetAll(GetAllRegistrationStatusesInput input)
        {
            var filteredRegistrationStatuses = _registrationStatusRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.StatusCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.StatusCodeFilter),
                    e => e.StatusCode == input.StatusCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),
                    e => e.Description == input.DescriptionFilter)
                .WhereIf(input.IsActiveFilter > -1,
                    e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredRegistrationStatuses = filteredRegistrationStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var registrationStatuses = from o in pagedAndFilteredRegistrationStatuses
                select new GetRegistrationStatusForViewDto()
                {
                    RegistrationStatus = new RegistrationStatusDto
                    {
                        StatusCode = o.StatusCode,
                        Description = o.Description,
                        IsActive = o.IsActive,
                        Id = o.Id
                    }
                };

            var totalCount = await filteredRegistrationStatuses.CountAsync();

            return new PagedResultDto<GetRegistrationStatusForViewDto>(
                totalCount,
                await registrationStatuses.ToListAsync()
            );
        }

        public async Task<GetRegistrationStatusForViewDto> GetRegistrationStatusForView(long id)
        {
            var registrationStatus = await _registrationStatusRepository.GetAsync(id);

            var output = new GetRegistrationStatusForViewDto
                {RegistrationStatus = ObjectMapper.Map<RegistrationStatusDto>(registrationStatus)};

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationStatuses_Edit)]
        public async Task<GetRegistrationStatusForEditOutput> GetRegistrationStatusForEdit(EntityDto<long> input)
        {
            var registrationStatus = await _registrationStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRegistrationStatusForEditOutput
                {RegistrationStatus = ObjectMapper.Map<CreateOrEditRegistrationStatusDto>(registrationStatus)};

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRegistrationStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_RegistrationStatuses_Create)]
        protected virtual async Task Create(CreateOrEditRegistrationStatusDto input)
        {
            var registrationStatus = ObjectMapper.Map<RegistrationStatus>(input);


            if (AbpSession.TenantId != null)
            {
                registrationStatus.TenantId = (int?) AbpSession.TenantId;
            }


            await _registrationStatusRepository.InsertAsync(registrationStatus);
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditRegistrationStatusDto input)
        {
            var registrationStatus = await _registrationStatusRepository.FirstOrDefaultAsync((long) input.Id);
            ObjectMapper.Map(input, registrationStatus);
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationStatuses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _registrationStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRegistrationStatusesToExcel(GetAllRegistrationStatusesForExcelInput input)
        {
            var filteredRegistrationStatuses = _registrationStatusRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.StatusCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.StatusCodeFilter),
                    e => e.StatusCode == input.StatusCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),
                    e => e.Description == input.DescriptionFilter)
                .WhereIf(input.IsActiveFilter > -1,
                    e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = from o in filteredRegistrationStatuses
                select new GetRegistrationStatusForViewDto
                {
                    RegistrationStatus = new RegistrationStatusDto
                    {
                        StatusCode = o.StatusCode,
                        Description = o.Description,
                        IsActive = o.IsActive,
                        Id = o.Id
                    }
                };


            var registrationStatusListDtos = await query.ToListAsync();

            return _registrationStatusesExcelExporter.ExportToFile(registrationStatusListDtos);
        }
    }
}