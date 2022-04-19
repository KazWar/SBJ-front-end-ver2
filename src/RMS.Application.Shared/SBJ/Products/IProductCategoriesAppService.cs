using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Products.Dtos;
using RMS.Dto;

namespace RMS.SBJ.Products
{
    public interface IProductCategoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductCategoryForViewDto>> GetAll(GetAllProductCategoriesInput input);

        Task<IEnumerable<GetProductCategoryForViewDto>> GetAllWithoutPaging();

        Task<GetProductCategoryForViewDto> GetProductCategoryForView(long id);

		Task<GetProductCategoryForEditOutput> GetProductCategoryForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProductCategoryDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProductCategoriesToExcel(GetAllProductCategoriesForExcelInput input);

		
    }
}