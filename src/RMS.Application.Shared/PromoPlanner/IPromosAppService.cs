using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;


namespace RMS.PromoPlanner
{
    public interface IPromosAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPromoForViewDto>> GetAll(GetAllPromosInput input);

        Task<GetPromoForViewDto> GetPromoForView(long id);

		Task<GetPromoForEditOutput> GetPromoForEdit(EntityDto<long> input);

		Task<long> CreateOrEdit(CreateOrEditPromoDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetPromosToExcel(GetAllPromosForExcelInput input);

		
		Task<PagedResultDto<PromoPromoScopeLookupTableDto>> GetAllPromoScopeForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PromoCampaignTypeLookupTableDto>> GetAllCampaignTypeForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PromoProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);
		
    }
}