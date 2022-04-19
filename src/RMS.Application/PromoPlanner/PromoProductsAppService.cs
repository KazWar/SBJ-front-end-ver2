using RMS.SBJ.Products;
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
    [AbpAuthorize(AppPermissions.Pages_PromoProducts)]
    public class PromoProductsAppService : RMSAppServiceBase, IPromoProductsAppService
    {
        private readonly IRepository<PromoProduct, long> _promoProductRepository;
        private readonly IPromoProductsExcelExporter _promoProductsExcelExporter;
        private readonly IRepository<Promo, long> _lookup_promoRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;


        public PromoProductsAppService(IRepository<PromoProduct, long> promoProductRepository, IPromoProductsExcelExporter promoProductsExcelExporter, IRepository<Promo, long> lookup_promoRepository, IRepository<Product, long> lookup_productRepository)
        {
            _promoProductRepository = promoProductRepository;
            _promoProductsExcelExporter = promoProductsExcelExporter;
            _lookup_promoRepository = lookup_promoRepository;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetPromoProductForViewDto>> GetAll(GetAllPromoProductsInput input)
        {

            var filteredPromoProducts = _promoProductRepository.GetAll()
                        .Include(e => e.PromoFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoPromocodeFilter), e => e.PromoFk != null && e.PromoFk.Promocode == input.PromoPromocodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCtnFilter), e => e.ProductFk != null && e.ProductFk.ProductCode == input.ProductCtnFilter);

            var pagedAndFilteredPromoProducts = filteredPromoProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var promoProducts = from o in pagedAndFilteredPromoProducts
                                join o1 in _lookup_promoRepository.GetAll() on o.PromoId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new GetPromoProductForViewDto()
                                {
                                    PromoProduct = new PromoProductDto
                                    {
                                        Id = o.Id
                                    },
                                    PromoPromocode = s1 == null ? "" : s1.Promocode.ToString(),
                                    ProductCtn = s2 == null ? "" : s2.ProductCode.ToString()
                                };

            var totalCount = await filteredPromoProducts.CountAsync();

            return new PagedResultDto<GetPromoProductForViewDto>(
                totalCount,
                await promoProducts.ToListAsync()
            );
        }

        /// <summary>
        /// 
        /// Gets all products for a specific promo and by its category.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CustomProductForView>> GetAllProductsForPromo(GetAllProductsForPromoInput input)
        {
            var filteredPromoProducts =
                _promoProductRepository.GetAll()
                    .Include(e => e.PromoFk)
                    .Include(e => e.ProductFk)
                    .Where(promoProduct => promoProduct.PromoId == input.PromoId);

            var pagedAndFilteredPromoProducts = filteredPromoProducts
                .OrderBy("id asc")
                .PageBy(input);

            var promoProducts =
                from o in pagedAndFilteredPromoProducts
                join o1 in _lookup_promoRepository.GetAll() on o.PromoId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()

                join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()

                select new CustomProductForView()
                {
                    Id = o.Id,
                    ProductId = s2.Id,
                    CtnCode = s2.ProductCode,
                    EanCode = s2.Ean,
                    Description = s2.Description
                };

            var totalCount = await filteredPromoProducts.CountAsync();

            return new PagedResultDto<CustomProductForView>(
                totalCount,
                await promoProducts.OrderBy(x => x.Description).ToListAsync());
        }

        public async Task<GetPromoProductForViewDto> GetPromoProductForView(long id)
        {
            var promoProduct = await _promoProductRepository.GetAsync(id);

            var output = new GetPromoProductForViewDto { PromoProduct = ObjectMapper.Map<PromoProductDto>(promoProduct) };

            var _lookupPromo = await _lookup_promoRepository.FirstOrDefaultAsync((long)output.PromoProduct.PromoId);
            output.PromoPromocode = _lookupPromo.Promocode.ToString();

            var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.PromoProduct.ProductId);
            output.ProductCtn = _lookupProduct.ProductCode.ToString();

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PromoProducts_Edit)]
        public async Task<GetPromoProductForEditOutput> GetPromoProductForEdit(EntityDto<long> input)
        {
            var promoProduct = await _promoProductRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPromoProductForEditOutput { PromoProduct = ObjectMapper.Map<CreateOrEditPromoProductDto>(promoProduct) };

            var _lookupPromo = await _lookup_promoRepository.FirstOrDefaultAsync((long)output.PromoProduct.PromoId);
            output.PromoPromocode = _lookupPromo.Promocode.ToString();

            var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.PromoProduct.ProductId);
            output.ProductCtn = _lookupProduct.ProductCode.ToString();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPromoProductDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PromoProducts_Create)]
        protected virtual async Task Create(CreateOrEditPromoProductDto input)
        {
            var promoProduct = ObjectMapper.Map<PromoProduct>(input);


            if (AbpSession.TenantId != null)
            {
                promoProduct.TenantId = (int?)AbpSession.TenantId;
            }


            await _promoProductRepository.InsertAsync(promoProduct);
        }

        [AbpAuthorize(AppPermissions.Pages_PromoProducts_Edit)]
        protected virtual async Task Update(CreateOrEditPromoProductDto input)
        {
            var promoProduct = await _promoProductRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, promoProduct);
        }

        [AbpAuthorize(AppPermissions.Pages_PromoProducts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _promoProductRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPromoProductsToExcel(GetAllPromoProductsForExcelInput input)
        {

            var filteredPromoProducts = _promoProductRepository.GetAll()
                        .Include(e => e.PromoFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromoPromocodeFilter), e => e.PromoFk != null && e.PromoFk.Promocode == input.PromoPromocodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCtnFilter), e => e.ProductFk != null && e.ProductFk.ProductCode == input.ProductCtnFilter);

            var query = (from o in filteredPromoProducts
                         join o1 in _lookup_promoRepository.GetAll() on o.PromoId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetPromoProductForViewDto()
                         {
                             PromoProduct = new PromoProductDto
                             {
                                 Id = o.Id
                             },
                             PromoPromocode = s1 == null ? "" : s1.Promocode.ToString(),
                             ProductCtn = s2 == null ? "" : s2.ProductCode.ToString()
                         });


            var promoProductListDtos = await query.ToListAsync();

            return _promoProductsExcelExporter.ExportToFile(promoProductListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_PromoProducts)]
        public async Task<PagedResultDto<PromoProductPromoLookupTableDto>> GetAllPromoForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_promoRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Promocode != null && e.Promocode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var promoList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoProductPromoLookupTableDto>();
            foreach (var promo in promoList)
            {
                lookupTableDtoList.Add(new PromoProductPromoLookupTableDto
                {
                    Id = promo.Id,
                    DisplayName = promo.Promocode?.ToString()
                });
            }

            return new PagedResultDto<PromoProductPromoLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PromoProducts)]
        public async Task<PagedResultDto<PromoProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ProductCode != null && e.ProductCode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PromoProductProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new PromoProductProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.ProductCode?.ToString()
                });
            }

            return new PagedResultDto<PromoProductProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PromoProducts)]
        public async Task<PagedResultDto<PromoProductProductLookupTableDto>> GetAvailableProductsByCategoryForLookupTable(GetAvailableProductsByProductCategoryForLookupTableInput input)
        {
            var occupiedProductsByExId = input.FilterEx.Split(',').Select(long.Parse).ToList();

            var query = _lookup_productRepository
                .GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.ProductCode != null && e.ProductCode.Contains(input.Filter))
                .WhereIf(occupiedProductsByExId.Count > 0, e => !occupiedProductsByExId.Contains(e.Id))
                .WhereIf(input.FilterId != null && input.FilterId > 0, e => e.ProductCategoryId == input.FilterId)
                .WhereIf(input.ProductCategory != null, e => e.ProductCategoryFk.Description == input.ProductCategory)
                .OrderBy(e => e.Description);

            var totalCount = await query.CountAsync();
            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = from product in productList
                                     select new PromoProductProductLookupTableDto
                                     {
                                         Id = product.Id,
                                         DisplayName = string.Format("{0} ({1})", product.Description?.ToString(), product.ProductCode?.ToString())
                                     };


            return new PagedResultDto<PromoProductProductLookupTableDto>(
                totalCount,
                lookupTableDtoList.ToList()
            ); ;
        }

    }
}