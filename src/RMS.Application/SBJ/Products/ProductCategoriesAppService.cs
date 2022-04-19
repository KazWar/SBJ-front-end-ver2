using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.Products.Exporting;
using RMS.SBJ.Products.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.Products
{
    [AbpAuthorize(AppPermissions.Pages_ProductCategories)]
    public class ProductCategoriesAppService : RMSAppServiceBase, IProductCategoriesAppService
    {
        private readonly IRepository<ProductCategory, long> _productCategoryRepository;
        private readonly IProductCategoriesExcelExporter _productCategoriesExcelExporter;


        public ProductCategoriesAppService(IRepository<ProductCategory, long> productCategoryRepository, IProductCategoriesExcelExporter productCategoriesExcelExporter)
        {
            _productCategoryRepository = productCategoryRepository;
            _productCategoriesExcelExporter = productCategoriesExcelExporter;

        }

        public async Task<PagedResultDto<GetProductCategoryForViewDto>> GetAll(GetAllProductCategoriesInput input)
        {

            var filteredProductCategories = _productCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.PoHandling.Contains(input.Filter) || e.PoCashBack.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.POHandlingFilter), e => e.PoHandling == input.POHandlingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.POCashbackFilter), e => e.PoCashBack == input.POCashbackFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorFilter), e => e.Color == input.ColorFilter);

            var pagedAndFilteredProductCategories = filteredProductCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productCategories = from o in pagedAndFilteredProductCategories
                                    select new GetProductCategoryForViewDto()
                                    {
                                        ProductCategory = new ProductCategoryDto
                                        {
                                            Code = o.Code,
                                            Description = o.Description,
                                            POHandling = o.PoHandling,
                                            POCashback = o.PoCashBack,
                                            Color = o.Color,
                                            Id = o.Id
                                        }
                                    };

            var totalCount = await filteredProductCategories.CountAsync();

            return new PagedResultDto<GetProductCategoryForViewDto>(
                totalCount,
                await productCategories.ToListAsync()
            );
        }

        public async Task<IEnumerable<GetProductCategoryForViewDto>> GetAllWithoutPaging()
        {

            var productCategories = _productCategoryRepository.GetAll().OrderBy(x => x.Description)
                        .Select(x => new GetProductCategoryForViewDto
                        {
                            ProductCategory = new ProductCategoryDto { Id = x.Id, Description = x.Description }
                        });

            return await productCategories.ToListAsync();
        }

        public async Task<GetProductCategoryForViewDto> GetProductCategoryForView(long id)
        {
            var productCategory = await _productCategoryRepository.GetAsync(id);

            var output = new GetProductCategoryForViewDto { ProductCategory = ObjectMapper.Map<ProductCategoryDto>(productCategory) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategories_Edit)]
        public async Task<GetProductCategoryForEditOutput> GetProductCategoryForEdit(EntityDto<long> input)
        {
            var productCategory = await _productCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductCategoryForEditOutput { ProductCategory = ObjectMapper.Map<CreateOrEditProductCategoryDto>(productCategory) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductCategories_Create)]
        protected virtual async Task Create(CreateOrEditProductCategoryDto input)
        {
            var productCategory = ObjectMapper.Map<ProductCategory>(input);


            if (AbpSession.TenantId != null)
            {
                productCategory.TenantId = (int?)AbpSession.TenantId;
            }


            await _productCategoryRepository.InsertAsync(productCategory);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategories_Edit)]
        protected virtual async Task Update(CreateOrEditProductCategoryDto input)
        {
            var productCategory = await _productCategoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productCategory);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductCategoriesToExcel(GetAllProductCategoriesForExcelInput input)
        {

            var filteredProductCategories = _productCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.PoHandling.Contains(input.Filter) || e.PoCashBack.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.POHandlingFilter), e => e.PoHandling == input.POHandlingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.POCashbackFilter), e => e.PoCashBack == input.POCashbackFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorFilter), e => e.Color == input.ColorFilter);

            var query = (from o in filteredProductCategories
                         select new GetProductCategoryForViewDto()
                         {
                             ProductCategory = new ProductCategoryDto
                             {
                                 Code = o.Code,
                                 Description = o.Description,
                                 POHandling = o.PoHandling,
                                 POCashback = o.PoCashBack,
                                 Color = o.Color,
                                 Id = o.Id
                             }
                         });


            var productCategoryListDtos = await query.ToListAsync();

            return _productCategoriesExcelExporter.ExportToFile(productCategoryListDtos);
        }


    }
}