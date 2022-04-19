using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;


namespace RMS.PromoPlanner
{
    public interface IProductCategoryYearPosAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductCategoryYearPoForViewDto>> GetAll(GetAllProductCategoryYearPosInput input);

        Task<GetProductCategoryYearPoForViewDto> GetProductCategoryYearPoForView(long id);

		Task<GetProductCategoryYearPoForEditOutput> GetProductCategoryYearPoForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProductCategoryYearPoDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProductCategoryYearPosToExcel(GetAllProductCategoryYearPosForExcelInput input);

		
		Task<PagedResultDto<ProductCategoryYearPoProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);
		
    }
}