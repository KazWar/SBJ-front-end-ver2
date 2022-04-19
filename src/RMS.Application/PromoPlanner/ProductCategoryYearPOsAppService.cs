using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Linq.Extensions;
using RMS.Dto;
using RMS.Authorization;
using RMS.SBJ.Products;
using RMS.PromoPlanner.Exporting;
using RMS.PromoPlanner.Dtos;

namespace RMS.PromoPlanner
{
    [AbpAuthorize(AppPermissions.Pages_ProductCategoryYearPos)]
    public class ProductCategoryYearPosAppService : RMSAppServiceBase, IProductCategoryYearPosAppService
    {
        private readonly IRepository<ProductCategoryYearPo, long> _productCategoryYearPoRepository;
        private readonly IProductCategoryYearPosExcelExporter _productCategoryYearPosExcelExporter;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;

        public ProductCategoryYearPosAppService(IRepository<ProductCategoryYearPo, long> productCategoryYearPoRepository, IProductCategoryYearPosExcelExporter productCategoryYearPOsExcelExporter, IRepository<ProductCategory, long> lookup_productCategoryRepository)
        {
            _productCategoryYearPoRepository = productCategoryYearPoRepository;
            _productCategoryYearPosExcelExporter = productCategoryYearPOsExcelExporter;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
        }

        public async Task<PagedResultDto<GetProductCategoryYearPoForViewDto>> GetAll(GetAllProductCategoryYearPosInput input)
        {
            var filteredProductCategoryYearPos = _productCategoryYearPoRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PoNumberHandling.Contains(input.Filter) || e.PoNumberCashback.Contains(input.Filter))
                        .WhereIf(input.MinYearFilter != null, e => e.Year >= input.MinYearFilter)
                        .WhereIf(input.MaxYearFilter != null, e => e.Year <= input.MaxYearFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PONumberHandlingFilter), e => e.PoNumberHandling == input.PONumberHandlingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PONumberCashbackFilter), e => e.PoNumberCashback == input.PONumberCashbackFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryCodeFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Code == input.ProductCategoryCodeFilter);

            var pagedAndFilteredProductCategoryYearPos = filteredProductCategoryYearPos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productCategoryYearPos = from o in pagedAndFilteredProductCategoryYearPos
                                         join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         select new GetProductCategoryYearPoForViewDto()
                                         {
                                             ProductCategoryYearPo = new ProductCategoryYearPoDto
                                             {
                                                 Year = o.Year,
                                                 PoNumberHandling = o.PoNumberHandling,
                                                 PoNumberCashback = o.PoNumberCashback,
                                                 Id = o.Id
                                             },
                                             ProductCategoryCode = s1 == null ? "" : s1.Code.ToString()
                                         };

            var totalCount = await filteredProductCategoryYearPos.CountAsync();

            return new PagedResultDto<GetProductCategoryYearPoForViewDto>(
                totalCount,
                await productCategoryYearPos.ToListAsync()
            );
        }

        public async Task<GetProductCategoryYearPoForViewDto> GetProductCategoryYearPoForView(long id)
        {
            var productCategoryYearPo = await _productCategoryYearPoRepository.GetAsync(id);

            var output = new GetProductCategoryYearPoForViewDto
            {
                ProductCategoryYearPo = ObjectMapper.Map<ProductCategoryYearPoDto>(productCategoryYearPo)
            };

            if (output.ProductCategoryYearPo.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.ProductCategoryYearPo.ProductCategoryId);
                output.ProductCategoryCode = _lookupProductCategory.Code.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryYearPos_Edit)]
        public async Task<GetProductCategoryYearPoForEditOutput> GetProductCategoryYearPoForEdit(EntityDto<long> input)
        {
            var productCategoryYearPo = await _productCategoryYearPoRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductCategoryYearPoForEditOutput
            {
                ProductCategoryYearPo = ObjectMapper.Map<CreateOrEditProductCategoryYearPoDto>(productCategoryYearPo)
            };

            var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync(output.ProductCategoryYearPo.ProductCategoryId);
            output.ProductCategoryCode = _lookupProductCategory.Code.ToString();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductCategoryYearPoDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryYearPos_Create)]
        protected virtual async Task Create(CreateOrEditProductCategoryYearPoDto input)
        {
            var productCategoryYearPo = ObjectMapper.Map<ProductCategoryYearPo>(input);

            if (AbpSession.TenantId != null)
            {
                productCategoryYearPo.TenantId = (int?)AbpSession.TenantId;
            }

            await _productCategoryYearPoRepository.InsertAsync(productCategoryYearPo);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryYearPos_Edit)]
        protected virtual async Task Update(CreateOrEditProductCategoryYearPoDto input)
        {
            var productCategoryYearPo = await _productCategoryYearPoRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productCategoryYearPo);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryYearPos_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productCategoryYearPoRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductCategoryYearPosToExcel(GetAllProductCategoryYearPosForExcelInput input)
        {
            var filteredProductCategoryYearPos = _productCategoryYearPoRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PoNumberHandling.Contains(input.Filter) || e.PoNumberCashback.Contains(input.Filter))
                        .WhereIf(input.MinYearFilter != null, e => e.Year >= input.MinYearFilter)
                        .WhereIf(input.MaxYearFilter != null, e => e.Year <= input.MaxYearFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PoNumberHandlingFilter), e => e.PoNumberHandling == input.PoNumberHandlingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PoNumberCashbackFilter), e => e.PoNumberCashback == input.PoNumberCashbackFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryCodeFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Code == input.ProductCategoryCodeFilter);

            var query = from o in filteredProductCategoryYearPos
                        join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        select new GetProductCategoryYearPoForViewDto()
                        {
                            ProductCategoryYearPo = new ProductCategoryYearPoDto
                            {
                                Year = o.Year,
                                PoNumberHandling = o.PoNumberHandling,
                                PoNumberCashback = o.PoNumberCashback,
                                Id = o.Id
                            },
                            ProductCategoryCode = s1 == null ? "" : s1.Code.ToString()
                        };

            var productCategoryYearPoListDtos = await query.ToListAsync();

            return _productCategoryYearPosExcelExporter.ExportToFile(productCategoryYearPoListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryYearPos)]
        public async Task<PagedResultDto<ProductCategoryYearPoProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Code != null && e.Code.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCategoryYearPoProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new ProductCategoryYearPoProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Code?.ToString()
                });
            }

            return new PagedResultDto<ProductCategoryYearPoProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}