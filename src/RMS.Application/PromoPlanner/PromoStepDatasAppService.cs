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
    public class PromoStepDatasAppService : RMSAppServiceBase, IPromoStepDatasAppService
    {
        private readonly IRepository<PromoStepData> _promoStepDataRepository;
        private readonly IPromoStepDatasExcelExporter _promoStepDatasExcelExporter;
        private readonly IRepository<PromoStep, int> _lookup_promoStepRepository;
        private readonly IRepository<Promo, long> _lookup_promoRepository;

        public PromoStepDatasAppService(IRepository<PromoStepData> promoStepDataRepository, IPromoStepDatasExcelExporter promoStepDatasExcelExporter, IRepository<PromoStep, int> lookup_promoStepRepository, IRepository<Promo, long> lookup_promoRepository)
        {
            _promoStepDataRepository = promoStepDataRepository;
            _promoStepDatasExcelExporter = promoStepDatasExcelExporter;
            _lookup_promoStepRepository = lookup_promoStepRepository;
            _lookup_promoRepository = lookup_promoRepository;
        }

        public async Task<PagedResultDto<GetPromoStepDataForViewDto>> GetAll(GetAllPromoStepDatasInput input)
        {
            var filteredPromoStepDatas = _promoStepDataRepository.GetAll()
                        .Include(e => e.PromoStepFk)
                        .Include(e => e.PromoFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinConfirmationDateFilter != null, e => e.ConfirmationDate >= input.MinConfirmationDateFilter)
                        .WhereIf(input.MaxConfirmationDateFilter != null, e => e.ConfirmationDate <= input.MaxConfirmationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoStepDescriptionFilter), e => e.PromoStepFk != null && e.PromoStepFk.Description == input.PromoStepDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoPromocodeFilter), e => e.PromoFk != null && e.PromoFk.Promocode == input.PromoPromocodeFilter);

            var pagedAndFilteredPromoStepDatas = filteredPromoStepDatas
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var promoStepDatas = from o in pagedAndFilteredPromoStepDatas
                                 join o1 in _lookup_promoStepRepository.GetAll() on o.PromoStepId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_promoRepository.GetAll() on o.PromoId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 select new GetPromoStepDataForViewDto()
                                 {
                                     PromoStepData = new PromoStepDataDto
                                     {
                                         ConfirmationDate = o.ConfirmationDate,
                                         Description = o.Description,
                                         Id = o.Id
                                     },
                                     PromoStepDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                                     PromoPromocode = s2 == null || s2.Promocode == null ? "" : s2.Promocode.ToString()
                                 };

            var totalCount = await filteredPromoStepDatas.CountAsync();

            return new PagedResultDto<GetPromoStepDataForViewDto>(
                totalCount,
                await promoStepDatas.ToListAsync()
            );
        }

        public async Task<GetPromoStepDataForViewDto> GetPromoStepDataForView(int id)
        {
            var promoStepData = await _promoStepDataRepository.GetAsync(id);

            var output = new GetPromoStepDataForViewDto { PromoStepData = ObjectMapper.Map<PromoStepDataDto>(promoStepData) };

            if (output.PromoStepData.PromoStepId != null)
            {
                var _lookupPromoStep = await _lookup_promoStepRepository.FirstOrDefaultAsync((int)output.PromoStepData.PromoStepId);
                output.PromoStepDescription = _lookupPromoStep?.Description?.ToString();
            }

            if (output.PromoStepData.PromoId != null)
            {
                var _lookupPromo = await _lookup_promoRepository.FirstOrDefaultAsync((long)output.PromoStepData.PromoId);
                output.PromoPromocode = _lookupPromo?.Promocode?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task<long?> GetPromoStepDataIdForFks(long promoId, long promoStepId)
        {
            var promoStepData = await _promoStepDataRepository.GetAllListAsync(x => x.PromoId == promoId && x.PromoStepId == promoStepId);
            var result = promoStepData.FirstOrDefault();
            return result?.Id;
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task<GetPromoStepDataForEditOutput> GetPromoStepDataForEdit(EntityDto input)
        {
            var promoStepData = await _promoStepDataRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPromoStepDataForEditOutput { PromoStepData = ObjectMapper.Map<CreateOrEditPromoStepDataDto>(promoStepData) };

            if (output.PromoStepData.PromoStepId != null)
            {
                var _lookupPromoStep = await _lookup_promoStepRepository.FirstOrDefaultAsync((int)output.PromoStepData.PromoStepId);
                output.PromoStepDescription = _lookupPromoStep?.Description?.ToString();
            }

            if (output.PromoStepData.PromoId != null)
            {
                var _lookupPromo = await _lookup_promoRepository.FirstOrDefaultAsync((long)output.PromoStepData.PromoId);
                output.PromoPromocode = _lookupPromo?.Promocode?.ToString();
            }

            return output;
        }

        public async Task<long> CreateOrEdit(CreateOrEditPromoStepDataDto input)
        {
            if (input.Id == null)
            {
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        protected virtual async Task<long> Create(CreateOrEditPromoStepDataDto input)
        {
            var promoStepData = ObjectMapper.Map<PromoStepData>(input);


            if (AbpSession.TenantId != null)
            {
                promoStepData.TenantId = (int?)AbpSession.TenantId;
            }


            return await _promoStepDataRepository.InsertAndGetIdAsync(promoStepData);
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        protected virtual async Task<long> Update(CreateOrEditPromoStepDataDto input)
        {
            var promoStepData = await _promoStepDataRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, promoStepData);
            return promoStepData.Id;
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task Delete(EntityDto input)
        {
            await _promoStepDataRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPromoStepDatasToExcel(GetAllPromoStepDatasForExcelInput input)
        {

            var filteredPromoStepDatas = _promoStepDataRepository.GetAll()
                        .Include(e => e.PromoStepFk)
                        .Include(e => e.PromoFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinConfirmationDateFilter != null, e => e.ConfirmationDate >= input.MinConfirmationDateFilter)
                        .WhereIf(input.MaxConfirmationDateFilter != null, e => e.ConfirmationDate <= input.MaxConfirmationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoStepDescriptionFilter), e => e.PromoStepFk != null && e.PromoStepFk.Description == input.PromoStepDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoPromocodeFilter), e => e.PromoFk != null && e.PromoFk.Promocode == input.PromoPromocodeFilter);

            var query = (from o in filteredPromoStepDatas
                         join o1 in _lookup_promoStepRepository.GetAll() on o.PromoStepId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_promoRepository.GetAll() on o.PromoId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetPromoStepDataForViewDto()
                         {
                             PromoStepData = new PromoStepDataDto
                             {
                                 ConfirmationDate = o.ConfirmationDate,
                                 Description = o.Description,
                                 Id = o.Id
                             },
                             PromoStepDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                             PromoPromocode = s2 == null || s2.Promocode == null ? "" : s2.Promocode.ToString()
                         });


            var promoStepDataListDtos = await query.ToListAsync();

            return _promoStepDatasExcelExporter.ExportToFile(promoStepDataListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task<PagedResultDto<PromoStepDataPromoStepLookupTableDto>> GetAllPromoStepForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_promoStepRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var promoStepList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoStepDataPromoStepLookupTableDto>();
            foreach (var promoStep in promoStepList)
            {
                lookupTableDtoList.Add(new PromoStepDataPromoStepLookupTableDto
                {
                    Id = promoStep.Id,
                    DisplayName = promoStep.Description?.ToString()
                });
            }

            return new PagedResultDto<PromoStepDataPromoStepLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Promos)]
        public async Task<PagedResultDto<PromoStepDataPromoLookupTableDto>> GetAllPromoForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_promoRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Promocode != null && e.Promocode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var promoList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoStepDataPromoLookupTableDto>();
            foreach (var promo in promoList)
            {
                lookupTableDtoList.Add(new PromoStepDataPromoLookupTableDto
                {
                    Id = promo.Id,
                    DisplayName = promo.Promocode?.ToString()
                });
            }

            return new PagedResultDto<PromoStepDataPromoLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}