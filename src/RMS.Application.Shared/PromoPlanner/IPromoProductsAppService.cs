using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;


namespace RMS.PromoPlanner
{
    public interface IPromoProductsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPromoProductForViewDto>> GetAll(GetAllPromoProductsInput input);

        Task<GetPromoProductForViewDto> GetPromoProductForView(long id);

		Task<GetPromoProductForEditOutput> GetPromoProductForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditPromoProductDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetPromoProductsToExcel(GetAllPromoProductsForExcelInput input);

		
		Task<PagedResultDto<PromoProductPromoLookupTableDto>> GetAllPromoForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PromoProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}