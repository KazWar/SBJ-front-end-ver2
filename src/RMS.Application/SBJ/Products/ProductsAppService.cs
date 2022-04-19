using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.Products.Exporting;
using RMS.SBJ.Products.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using RMS.SBJ.ProductHandlings;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.HandlingLineProducts;

namespace RMS.SBJ.Products
{
    [AbpAuthorize(AppPermissions.Pages_Products)]
    public class ProductsAppService : RMSAppServiceBase, IProductsAppService
    {
        private readonly IRepository<Product, long> _productRepository;
        private readonly IProductsExcelExporter _productsExcelExporter;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;
        private readonly IRepository<ProductHandling, long> _lookup_productHandlingRepository;
        private readonly IRepository<HandlingLine, long> _lookup_handlingLineRepository;
        private readonly IRepository<HandlingLineProduct, long> _lookup_handlingLineProductRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public ProductsAppService(
            IRepository<Product, long> productRepository, 
            IProductsExcelExporter productsExcelExporter, 
            IRepository<ProductCategory, long> lookup_productCategoryRepository,
            IRepository<ProductHandling, long> lookup_productHandlingRepository,
            IRepository<HandlingLine, long> lookup_handlingLineRepository,
            IRepository<HandlingLineProduct, long> lookup_handlingLineProductRepository,
            IRepository<Product, long> lookup_productRepository)
        {
            _productRepository = productRepository;
            _productsExcelExporter = productsExcelExporter;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_productHandlingRepository = lookup_productHandlingRepository;
            _lookup_handlingLineRepository = lookup_handlingLineRepository;
            _lookup_handlingLineProductRepository = lookup_handlingLineProductRepository;
            _lookup_productRepository = lookup_productRepository;
        }
        
        public async Task<PagedResultDto<GetProductForViewDto>> GetAllProductsForCampaign(long campaignId)
        {
            var productHandling = await _lookup_productHandlingRepository.GetAll().Where(ph => ph.CampaignId == campaignId).FirstOrDefaultAsync();
            if (productHandling == null)
            {
                Logger.Error($"Error in {nameof(GetAllProductsForCampaign)}: could not find ProductHandling record with Campaign ID {campaignId}.");
                return null;
            }
            
            var handlingLines = await _lookup_handlingLineRepository.GetAllListAsync(x => x.ProductHandlingId == productHandling.Id);
            if (handlingLines == null)
            {
                Logger.Error($"Error in {nameof(GetAllProductsForCampaign)}: could not find HandlingLine records with Product Handling ID {productHandling.Id}.");
                return null;
            }

            var allProductsForCampaign = new List<Product>();

            foreach(var handlingLine in handlingLines)
            {
                var handlingLineProducts = await _lookup_handlingLineProductRepository.GetAllListAsync(x => x.HandlingLineId == handlingLine.Id);
                if (handlingLineProducts == null)
                {
                    continue;
                }

                foreach(var handlingLineProduct in handlingLineProducts)
                {
                    var product = await _lookup_productRepository.GetAllIncluding(x => x.ProductCategoryFk).FirstAsync(x => x.Id == handlingLineProduct.ProductId);
                    if (product == null)
                    {
                        continue;
                    }

                    if (!allProductsForCampaign.Any(x => x.Id == product.Id))
                    {
                        allProductsForCampaign.Add(product);
                    }
                }
            }

            var totalCount = allProductsForCampaign.Count();

            return new PagedResultDto<GetProductForViewDto>
            {
                TotalCount = totalCount,
                Items = allProductsForCampaign.Select(q => new GetProductForViewDto
                {
                    Product = new ProductDto
                    {
                        Id = q.Id,
                        Description = q.Description,
                        Ean = q.Ean,
                        ProductCategoryId = q.ProductCategoryId,
                        ProductCode = q.ProductCode
                    },
                    ProductCategoryDescription = q.ProductCategoryFk.Description
                }).OrderBy(q => q.Product.ProductCode).ToList()
            };
        }

        public async Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input)
        {
            var filteredProducts = _productRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProductCode.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Ean.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCodeFilter), e => e.ProductCode == input.ProductCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EanFilter), e => e.Ean == input.EanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryDescriptionFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Description == input.ProductCategoryDescriptionFilter);

            var pagedAndFilteredProducts = filteredProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var products = from o in pagedAndFilteredProducts
                           join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           select new GetProductForViewDto()
                           {
                               Product = new ProductDto
                               {
                                   ProductCode = o.ProductCode,
                                   Description = o.Description,
                                   Ean = o.Ean,
                                   Id = o.Id
                               },
                               ProductCategoryDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
                           };

            var totalCount = await filteredProducts.CountAsync();

            return new PagedResultDto<GetProductForViewDto>(
                totalCount,
                await products.ToListAsync()
            );
        }

        public async Task<GetProductForViewDto> GetProductForView(long id)
        {
            var product = await _productRepository.GetAsync(id);

            var output = new GetProductForViewDto { Product = ObjectMapper.Map<ProductDto>(product) };

            if (output.Product.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.Product.ProductCategoryId);
                output.ProductCategoryDescription = _lookupProductCategory?.Description?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        public async Task<GetProductForEditOutput> GetProductForEdit(EntityDto<long> input)
        {
            var product = await _productRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductForEditOutput { Product = ObjectMapper.Map<CreateOrEditProductDto>(product) };

            if (output.Product.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.Product.ProductCategoryId);
                output.ProductCategoryDescription = _lookupProductCategory?.Description?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Products_Create)]
        protected virtual async Task Create(CreateOrEditProductDto input)
        {
            var product = ObjectMapper.Map<Product>(input);

            if (AbpSession.TenantId != null)
            {
                product.TenantId = (int?)AbpSession.TenantId;
            }


            await _productRepository.InsertAsync(product);
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        protected virtual async Task Update(CreateOrEditProductDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, product);
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input)
        {
            var filteredProducts = _productRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProductCode.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Ean.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCodeFilter), e => e.ProductCode == input.ProductCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EanFilter), e => e.Ean == input.EanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryDescriptionFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Description == input.ProductCategoryDescriptionFilter);

            var query = (from o in filteredProducts
                         join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetProductForViewDto()
                         {
                             Product = new ProductDto
                             {
                                 ProductCode = o.ProductCode,
                                 Description = o.Description,
                                 Ean = o.Ean,
                                 Id = o.Id
                             },
                             ProductCategoryDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
                         });

            var productListDtos = await query.ToListAsync();

            return _productsExcelExporter.ExportToFile(productListDtos);
        }
    }
}