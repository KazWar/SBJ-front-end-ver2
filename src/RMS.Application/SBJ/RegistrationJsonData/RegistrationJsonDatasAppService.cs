using RMS.SBJ.Registrations;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.RegistrationJsonData.Exporting;
using RMS.SBJ.RegistrationJsonData.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.RegistrationJsonData
{
    [AbpAuthorize(AppPermissions.Pages_RegistrationJsonDatas)]
    public class RegistrationJsonDatasAppService : RMSAppServiceBase, IRegistrationJsonDatasAppService
    {
        private readonly IRepository<RegistrationJsonData, long> _registrationJsonDataRepository;
        private readonly IRegistrationJsonDatasExcelExporter _registrationJsonDatasExcelExporter;
        private readonly IRepository<Registration, long> _lookup_registrationRepository;

        public RegistrationJsonDatasAppService(IRepository<RegistrationJsonData, long> registrationJsonDataRepository, IRegistrationJsonDatasExcelExporter registrationJsonDatasExcelExporter, IRepository<Registration, long> lookup_registrationRepository)
        {
            _registrationJsonDataRepository = registrationJsonDataRepository;
            _registrationJsonDatasExcelExporter = registrationJsonDatasExcelExporter;
            _lookup_registrationRepository = lookup_registrationRepository;

        }

        public async Task<PagedResultDto<GetRegistrationJsonDataForViewDto>> GetAll(GetAllRegistrationJsonDatasInput input)
        {

            var filteredRegistrationJsonDatas = _registrationJsonDataRepository.GetAll()
                        .Include(e => e.RegistrationFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Data.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DataFilter), e => e.Data == input.DataFilter)
                        .WhereIf(input.MinDateCreatedFilter != null, e => e.DateCreated >= input.MinDateCreatedFilter)
                        .WhereIf(input.MaxDateCreatedFilter != null, e => e.DateCreated <= input.MaxDateCreatedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationFirstNameFilter), e => e.RegistrationFk != null && e.RegistrationFk.FirstName == input.RegistrationFirstNameFilter);

            var pagedAndFilteredRegistrationJsonDatas = filteredRegistrationJsonDatas
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var registrationJsonDatas = from o in pagedAndFilteredRegistrationJsonDatas
                                        join o1 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()

                                        select new GetRegistrationJsonDataForViewDto()
                                        {
                                            RegistrationJsonData = new RegistrationJsonDataDto
                                            {
                                                Data = o.Data,
                                                DateCreated = o.DateCreated,
                                                Id = o.Id
                                            },
                                            RegistrationFirstName = s1 == null || s1.FirstName == null ? "" : s1.FirstName.ToString()
                                        };

            var totalCount = await filteredRegistrationJsonDatas.CountAsync();

            return new PagedResultDto<GetRegistrationJsonDataForViewDto>(
                totalCount,
                await registrationJsonDatas.ToListAsync()
            );
        }

        public async Task<GetRegistrationJsonDataForViewDto> GetRegistrationJsonDataForView(long id)
        {
            var registrationJsonData = await _registrationJsonDataRepository.GetAsync(id);

            var output = new GetRegistrationJsonDataForViewDto { RegistrationJsonData = ObjectMapper.Map<RegistrationJsonDataDto>(registrationJsonData) };

            if (output.RegistrationJsonData.RegistrationId != null)
            {
                var _lookupRegistration = await _lookup_registrationRepository.FirstOrDefaultAsync((long)output.RegistrationJsonData.RegistrationId);
                output.RegistrationFirstName = _lookupRegistration?.FirstName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationJsonDatas_Edit)]
        public async Task<GetRegistrationJsonDataForEditOutput> GetRegistrationJsonDataForEdit(EntityDto<long> input)
        {
            var registrationJsonData = await _registrationJsonDataRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRegistrationJsonDataForEditOutput { RegistrationJsonData = ObjectMapper.Map<CreateOrEditRegistrationJsonDataDto>(registrationJsonData) };

            if (output.RegistrationJsonData.RegistrationId != null)
            {
                var _lookupRegistration = await _lookup_registrationRepository.FirstOrDefaultAsync((long)output.RegistrationJsonData.RegistrationId);
                output.RegistrationFirstName = _lookupRegistration?.FirstName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRegistrationJsonDataDto input)
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

        [AbpAuthorize(AppPermissions.Pages_RegistrationJsonDatas_Create)]
        protected virtual async Task Create(CreateOrEditRegistrationJsonDataDto input)
        {
            var registrationJsonData = ObjectMapper.Map<RegistrationJsonData>(input);

            if (AbpSession.TenantId != null)
            {
                registrationJsonData.TenantId = (int?)AbpSession.TenantId;
            }

            await _registrationJsonDataRepository.InsertAsync(registrationJsonData);
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationJsonDatas_Edit)]
        protected virtual async Task Update(CreateOrEditRegistrationJsonDataDto input)
        {
            var registrationJsonData = await _registrationJsonDataRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, registrationJsonData);
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationJsonDatas_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _registrationJsonDataRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRegistrationJsonDatasToExcel(GetAllRegistrationJsonDatasForExcelInput input)
        {

            var filteredRegistrationJsonDatas = _registrationJsonDataRepository.GetAll()
                        .Include(e => e.RegistrationFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Data.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DataFilter), e => e.Data == input.DataFilter)
                        .WhereIf(input.MinDateCreatedFilter != null, e => e.DateCreated >= input.MinDateCreatedFilter)
                        .WhereIf(input.MaxDateCreatedFilter != null, e => e.DateCreated <= input.MaxDateCreatedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationFirstNameFilter), e => e.RegistrationFk != null && e.RegistrationFk.FirstName == input.RegistrationFirstNameFilter);

            var query = (from o in filteredRegistrationJsonDatas
                         join o1 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetRegistrationJsonDataForViewDto()
                         {
                             RegistrationJsonData = new RegistrationJsonDataDto
                             {
                                 Data = o.Data,
                                 DateCreated = o.DateCreated,
                                 Id = o.Id
                             },
                             RegistrationFirstName = s1 == null || s1.FirstName == null ? "" : s1.FirstName.ToString()
                         });

            var registrationJsonDataListDtos = await query.ToListAsync();

            return _registrationJsonDatasExcelExporter.ExportToFile(registrationJsonDataListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_RegistrationJsonDatas)]
        public async Task<PagedResultDto<RegistrationJsonDataRegistrationLookupTableDto>> GetAllRegistrationForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_registrationRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FirstName != null && e.FirstName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var registrationList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RegistrationJsonDataRegistrationLookupTableDto>();
            foreach (var registration in registrationList)
            {
                lookupTableDtoList.Add(new RegistrationJsonDataRegistrationLookupTableDto
                {
                    Id = registration.Id,
                    DisplayName = registration.FirstName?.ToString()
                });
            }

            return new PagedResultDto<RegistrationJsonDataRegistrationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}