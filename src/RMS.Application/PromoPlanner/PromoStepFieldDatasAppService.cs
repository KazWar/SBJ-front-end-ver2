using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.PromoPlanner.Exporting;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.PromoPlanner
{
    [AbpAuthorize(AppPermissions.Pages_Promos)]
    public class PromoStepFieldDatasAppService : RMSAppServiceBase, IPromoStepFieldDatasAppService
    {
        private readonly IRepository<PromoStepFieldData> _promoStepFieldDataRepository;
        private readonly IPromoStepFieldDatasExcelExporter _promoStepFieldDatasExcelExporter;
        private readonly IRepository<PromoStepField, int> _lookup_promoStepFieldRepository;
        private readonly IRepository<PromoStepData, int> _lookup_promoStepDataRepository;


        public PromoStepFieldDatasAppService(IRepository<PromoStepFieldData> promoStepFieldDataRepository, IPromoStepFieldDatasExcelExporter promoStepFieldDatasExcelExporter, IRepository<PromoStepField, int> lookup_promoStepFieldRepository, IRepository<PromoStepData, int> lookup_promoStepDataRepository)
        {
            _promoStepFieldDataRepository = promoStepFieldDataRepository;
            _promoStepFieldDatasExcelExporter = promoStepFieldDatasExcelExporter;
            _lookup_promoStepFieldRepository = lookup_promoStepFieldRepository;
            _lookup_promoStepDataRepository = lookup_promoStepDataRepository;
        }

        public async Task<PagedResultDto<GetPromoStepFieldDataForViewDto>> GetAll(GetAllPromoStepFieldDatasInput input)
        {

            var filteredPromoStepFieldDatas = _promoStepFieldDataRepository.GetAll()
                        .Include(e => e.PromoStepFieldFk)
                        .Include(e => e.PromoStepDataFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Value.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter), e => e.Value == input.ValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoStepFieldDescriptionFilter), e => e.PromoStepFieldFk != null && e.PromoStepFieldFk.Description == input.PromoStepFieldDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoStepDataDescriptionFilter), e => e.PromoStepDataFk != null && e.PromoStepDataFk.Description == input.PromoStepDataDescriptionFilter);

            var pagedAndFilteredPromoStepFieldDatas = filteredPromoStepFieldDatas
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var promoStepFieldDatas = from o in pagedAndFilteredPromoStepFieldDatas
                                      join o1 in _lookup_promoStepFieldRepository.GetAll() on o.PromoStepFieldId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      join o2 in _lookup_promoStepDataRepository.GetAll() on o.PromoStepDataId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      select new GetPromoStepFieldDataForViewDto()
                                      {
                                          PromoStepFieldData = new PromoStepFieldDataDto
                                          {
                                              Value = o.Value,
                                              Id = o.Id
                                          },
                                          PromoStepFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                                          PromoStepDataDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                                      };

            var totalCount = await filteredPromoStepFieldDatas.CountAsync();

            return new PagedResultDto<GetPromoStepFieldDataForViewDto>(
                totalCount,
                await promoStepFieldDatas.ToListAsync()
            );
        }

        public async Task<long?> GetPromoStepDataFieldIdForFks(long promoStepFieldId, long promoStepDataId)
        {
            var promoStepFieldData = await _promoStepFieldDataRepository
                .GetAllListAsync(x => x.PromoStepFieldId == promoStepFieldId && x.PromoStepDataId == promoStepDataId);
            var result = promoStepFieldData.FirstOrDefault();
            return result?.Id;
        }

        public async Task<GetPromoStepFieldDataForViewDto> GetPromoStepFieldDataForView(int id)
        {
            var promoStepFieldData = await _promoStepFieldDataRepository.GetAsync(id);

            var output = new GetPromoStepFieldDataForViewDto { PromoStepFieldData = ObjectMapper.Map<PromoStepFieldDataDto>(promoStepFieldData) };

            if (output.PromoStepFieldData.PromoStepFieldId != null)
            {
                var _lookupPromoStepField = await _lookup_promoStepFieldRepository.FirstOrDefaultAsync((int)output.PromoStepFieldData.PromoStepFieldId);
                output.PromoStepFieldDescription = _lookupPromoStepField?.Description?.ToString();
            }

            if (output.PromoStepFieldData.PromoStepDataId != null)
            {
                var _lookupPromoStepData = await _lookup_promoStepDataRepository.FirstOrDefaultAsync((int)output.PromoStepFieldData.PromoStepDataId);
                output.PromoStepDataDescription = _lookupPromoStepData?.Description?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task<GetPromoStepFieldDataForEditOutput> GetPromoStepFieldDataForEdit(EntityDto input)
        {
            var promoStepFieldData = await _promoStepFieldDataRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPromoStepFieldDataForEditOutput { PromoStepFieldData = ObjectMapper.Map<CreateOrEditPromoStepFieldDataDto>(promoStepFieldData) };

            if (output.PromoStepFieldData.PromoStepFieldId != null)
            {
                var _lookupPromoStepField = await _lookup_promoStepFieldRepository.FirstOrDefaultAsync((int)output.PromoStepFieldData.PromoStepFieldId);
                output.PromoStepFieldDescription = _lookupPromoStepField?.Description?.ToString();
            }

            if (output.PromoStepFieldData.PromoStepDataId != null)
            {
                var _lookupPromoStepData = await _lookup_promoStepDataRepository.FirstOrDefaultAsync((int)output.PromoStepFieldData.PromoStepDataId);
                output.PromoStepDataDescription = _lookupPromoStepData?.Description?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPromoStepFieldDataDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        protected virtual async Task Create(CreateOrEditPromoStepFieldDataDto input)
        {
            var promoStepFieldData = ObjectMapper.Map<PromoStepFieldData>(input);


            if (AbpSession.TenantId != null)
            {
                promoStepFieldData.TenantId = (int?)AbpSession.TenantId;
            }


            await _promoStepFieldDataRepository.InsertAsync(promoStepFieldData);
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        protected virtual async Task Update(CreateOrEditPromoStepFieldDataDto input)
        {
            var promoStepFieldData = await _promoStepFieldDataRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, promoStepFieldData);
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task Delete(EntityDto input)
        {
            await _promoStepFieldDataRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPromoStepFieldDatasToExcel(GetAllPromoStepFieldDatasForExcelInput input)
        {

            var filteredPromoStepFieldDatas = _promoStepFieldDataRepository.GetAll()
                        .Include(e => e.PromoStepFieldFk)
                        .Include(e => e.PromoStepDataFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Value.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter), e => e.Value == input.ValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoStepFieldDescriptionFilter), e => e.PromoStepFieldFk != null && e.PromoStepFieldFk.Description == input.PromoStepFieldDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoStepDataDescriptionFilter), e => e.PromoStepDataFk != null && e.PromoStepDataFk.Description == input.PromoStepDataDescriptionFilter);

            var query = (from o in filteredPromoStepFieldDatas
                         join o1 in _lookup_promoStepFieldRepository.GetAll() on o.PromoStepFieldId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_promoStepDataRepository.GetAll() on o.PromoStepDataId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetPromoStepFieldDataForViewDto()
                         {
                             PromoStepFieldData = new PromoStepFieldDataDto
                             {
                                 Value = o.Value,
                                 Id = o.Id
                             },
                             PromoStepFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                             PromoStepDataDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                         });


            var promoStepFieldDataListDtos = await query.ToListAsync();

            return _promoStepFieldDatasExcelExporter.ExportToFile(promoStepFieldDataListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task<PagedResultDto<PromoStepFieldDataPromoStepFieldLookupTableDto>> GetAllPromoStepFieldForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_promoStepFieldRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var promoStepFieldList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoStepFieldDataPromoStepFieldLookupTableDto>();
            foreach (var promoStepField in promoStepFieldList)
            {
                lookupTableDtoList.Add(new PromoStepFieldDataPromoStepFieldLookupTableDto
                {
                    Id = promoStepField.Id,
                    DisplayName = promoStepField.Description?.ToString()
                });
            }

            return new PagedResultDto<PromoStepFieldDataPromoStepFieldLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task<PagedResultDto<PromoStepFieldDataPromoStepFieldLookupTableDto>> GetAvailablePromoStepFieldForLookupTable(GetAvailableForLookupTableInput input)
        {
            var occupiedPromoStepFieldsByEx = !String.IsNullOrEmpty(input.FilterEx) && !String.IsNullOrWhiteSpace(input.FilterEx) ? input.FilterEx.Split("|") : new string[] { };

            var query = _lookup_promoStepFieldRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               ).Where(e => e.PromoStepId == input.FilterId
               ).WhereIf(occupiedPromoStepFieldsByEx.Length > 0,
                  e => e.Description != null && !occupiedPromoStepFieldsByEx.Contains(e.Description)
               ).OrderBy(e => e.Description);

            var totalCount = await query.CountAsync();

            var promoStepFieldList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoStepFieldDataPromoStepFieldLookupTableDto>();
            foreach (var promoStepField in promoStepFieldList)
            {
                lookupTableDtoList.Add(new PromoStepFieldDataPromoStepFieldLookupTableDto
                {
                    Id = promoStepField.Id,
                    DisplayName = promoStepField.Description?.ToString()
                });
            }

            return new PagedResultDto<PromoStepFieldDataPromoStepFieldLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task<PagedResultDto<PromoStepFieldDataPromoStepDataLookupTableDto>> GetAllPromoStepDataForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_promoStepDataRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var promoStepDataList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoStepFieldDataPromoStepDataLookupTableDto>();
            foreach (var promoStepData in promoStepDataList)
            {
                lookupTableDtoList.Add(new PromoStepFieldDataPromoStepDataLookupTableDto
                {
                    Id = promoStepData.Id,
                    DisplayName = promoStepData.Description?.ToString()
                });
            }

            return new PagedResultDto<PromoStepFieldDataPromoStepDataLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}