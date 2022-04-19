using RMS.SBJ.Retailers;
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
using RMS.SBJ.CodeTypeTables;

namespace RMS.PromoPlanner
{
    [AbpAuthorize(AppPermissions.Pages_PromoRetailers)]
    public class PromoRetailersAppService : RMSAppServiceBase, IPromoRetailersAppService
    {
        private readonly IRepository<PromoRetailer, long> _promoRetailerRepository;
        private readonly IPromoRetailersExcelExporter _promoRetailersExcelExporter;
        private readonly IRepository<Promo, long> _lookup_promoRepository;
        private readonly IRepository<Retailer, long> _lookup_retailerRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;


        public PromoRetailersAppService(IRepository<PromoRetailer, long> promoRetailerRepository, IPromoRetailersExcelExporter promoRetailersExcelExporter, IRepository<Promo, long> lookup_promoRepository, IRepository<Retailer, long> lookup_retailerRepository)
        {
            _promoRetailerRepository = promoRetailerRepository;
            _promoRetailersExcelExporter = promoRetailersExcelExporter;
            _lookup_promoRepository = lookup_promoRepository;
            _lookup_retailerRepository = lookup_retailerRepository;

        }

        public async Task<PagedResultDto<GetPromoRetailerForViewDto>> GetAll(GetAllPromoRetailersInput input)
        {

            var filteredPromoRetailers = _promoRetailerRepository.GetAll()
                .Include(e => e.PromoFk)
                .Include(e => e.RetailerFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                .WhereIf(!string.IsNullOrWhiteSpace(input.PromoPromocodeFilter), e => e.PromoFk != null && e.PromoFk.Promocode == input.PromoPromocodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.RetailerCodeFilter), e => e.RetailerFk != null && e.RetailerFk.Code == input.RetailerCodeFilter);

            var pagedAndFilteredPromoRetailers = filteredPromoRetailers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var promoRetailers = from o in pagedAndFilteredPromoRetailers
                                 join o1 in _lookup_promoRepository.GetAll() on o.PromoId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_retailerRepository.GetAll() on o.RetailerId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 select new GetPromoRetailerForViewDto()
                                 {
                                     PromoRetailer = new PromoRetailerDto
                                     {
                                         Id = o.Id
                                     },
                                     PromoPromocode = s1 == null || s1.Promocode == null ? "" : s1.Promocode.ToString(),
                                     RetailerCode = s2 == null || s2.Code == null ? "" : s2.Code.ToString()
                                 };

            var totalCount = await filteredPromoRetailers.CountAsync();

            return new PagedResultDto<GetPromoRetailerForViewDto>(
                totalCount,
                await promoRetailers.ToListAsync()
            );
        }

        public async Task<PagedResultDto<CustomPromoRetailerForView>> GetAllRetailersForPromo(GetAllRetailersForPromoInput input)
        {
            var filteredPromoRetailers =
                    _promoRetailerRepository.GetAll()
                    .Include(e => e.PromoFk)
                    .Include(e => e.RetailerFk)
                    .Where(retailer => retailer.PromoId == input.PromoId)
                    .OrderBy("id asc")
                    .PageBy(input);

            var listing = from o in filteredPromoRetailers
                          select new CustomPromoRetailerForView()
                          {
                              Id = o.Id,
                              RetailerId = o.RetailerId,
                              RetailerCode = o.RetailerFk.Code,
                              RetailerName = o.RetailerFk.Name,
                              RetailerCountry = o.RetailerFk.CountryFk.Description
                          };

            var totalCount = await listing.CountAsync();

            return new PagedResultDto<CustomPromoRetailerForView>(
                totalCount,
                await listing.OrderBy(x => x.RetailerName).ToListAsync());
        }

        public async Task<GetPromoRetailerForViewDto> GetPromoRetailerForView(long id)
        {
            var promoRetailer = await _promoRetailerRepository.GetAsync(id);

            var output = new GetPromoRetailerForViewDto { PromoRetailer = ObjectMapper.Map<PromoRetailerDto>(promoRetailer) };

            var _lookupPromo = await _lookup_promoRepository.FirstOrDefaultAsync((long)output.PromoRetailer.PromoId);
            output.PromoPromocode = _lookupPromo?.Promocode?.ToString();

            var _lookupRetailer = await _lookup_retailerRepository.FirstOrDefaultAsync((long)output.PromoRetailer.RetailerId);
            output.RetailerCode = _lookupRetailer?.Code?.ToString();

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PromoRetailers_Edit)]
        public async Task<GetPromoRetailerForEditOutput> GetPromoRetailerForEdit(EntityDto<long> input)
        {
            var promoRetailer = await _promoRetailerRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPromoRetailerForEditOutput { PromoRetailer = ObjectMapper.Map<CreateOrEditPromoRetailerDto>(promoRetailer) };

            var _lookupPromo = await _lookup_promoRepository.FirstOrDefaultAsync((long)output.PromoRetailer.PromoId);
            output.PromoPromocode = _lookupPromo?.Promocode?.ToString();

            var _lookupRetailer = await _lookup_retailerRepository.FirstOrDefaultAsync((long)output.PromoRetailer.RetailerId);
            output.RetailerCode = _lookupRetailer?.Code?.ToString();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPromoRetailerDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PromoRetailers_Create)]
        protected virtual async Task Create(CreateOrEditPromoRetailerDto input)
        {
            var promoRetailer = ObjectMapper.Map<PromoRetailer>(input);

            if (AbpSession.TenantId != null)
            {
                promoRetailer.TenantId = (int?)AbpSession.TenantId;
            }

            await _promoRetailerRepository.InsertAsync(promoRetailer);
        }

        [AbpAuthorize(AppPermissions.Pages_PromoRetailers_Edit)]
        protected virtual async Task Update(CreateOrEditPromoRetailerDto input)
        {
            var promoRetailer = await _promoRetailerRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, promoRetailer);
        }

        [AbpAuthorize(AppPermissions.Pages_PromoRetailers_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _promoRetailerRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPromoRetailersToExcel(GetAllPromoRetailersForExcelInput input)
        {

            var filteredPromoRetailers = _promoRetailerRepository.GetAll()
                        .Include(e => e.PromoFk)
                        .Include(e => e.RetailerFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoPromocodeFilter), e => e.PromoFk != null && e.PromoFk.Promocode == input.PromoPromocodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RetailerCodeFilter), e => e.RetailerFk != null && e.RetailerFk.Code == input.RetailerCodeFilter);

            var query = (from o in filteredPromoRetailers
                         join o1 in _lookup_promoRepository.GetAll() on o.PromoId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_retailerRepository.GetAll() on o.RetailerId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetPromoRetailerForViewDto()
                         {
                             PromoRetailer = new PromoRetailerDto
                             {
                                 Id = o.Id
                             },
                             PromoPromocode = s1 == null || s1.Promocode == null ? "" : s1.Promocode.ToString(),
                             RetailerCode = s2 == null || s2.Code == null ? "" : s2.Code.ToString()
                         });


            var promoRetailerListDtos = await query.ToListAsync();

            return _promoRetailersExcelExporter.ExportToFile(promoRetailerListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_PromoRetailers)]
        public async Task<PagedResultDto<PromoRetailerPromoLookupTableDto>> GetAllPromoForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_promoRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Promocode != null && e.Promocode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var promoList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoRetailerPromoLookupTableDto>();
            foreach (var promo in promoList)
            {
                lookupTableDtoList.Add(new PromoRetailerPromoLookupTableDto
                {
                    Id = promo.Id,
                    DisplayName = promo.Promocode?.ToString()
                });
            }

            return new PagedResultDto<PromoRetailerPromoLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PromoRetailers)]
        public async Task<PagedResultDto<PromoRetailerRetailerLookupTableDto>> GetAllRetailerForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_retailerRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Code != null && e.Code.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var retailerList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoRetailerRetailerLookupTableDto>();
            foreach (var retailer in retailerList)
            {
                lookupTableDtoList.Add(new PromoRetailerRetailerLookupTableDto
                {
                    Id = retailer.Id,
                    DisplayName = retailer.Code?.ToString()
                });
            }

            return new PagedResultDto<PromoRetailerRetailerLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PromoRetailers)]
        public async Task<PagedResultDto<PromoRetailerRetailerLookupTableDto>> GetAvailableRetailerForLookupTable(GetAvailableForLookupTableInput input)
        {
            var occupiedRetailersByExId = input.FilterEx.Split(',').Select(long.Parse).ToList();

            var query = _lookup_retailerRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.Code != null && e.Code.Contains(input.Filter)
                ).WhereIf(occupiedRetailersByExId.Count > 0,
                   e => !occupiedRetailersByExId.Contains(e.Id)
                ).OrderBy(e => e.Name);   //.Where(e => !occupiedRetailersById.Contains(e.Id));

            var totalCount = await query.CountAsync();

            var retailerList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = from retailer in retailerList
                                     select new PromoRetailerRetailerLookupTableDto
                                     {
                                         Id = retailer.Id,
                                         DisplayName = string.Format("{0} ({1})", retailer.Name?.ToString(), retailer.Code?.ToString())
                                     };

            return new PagedResultDto<PromoRetailerRetailerLookupTableDto>(
                totalCount,
                lookupTableDtoList.ToList()
            );
        }
    }
}