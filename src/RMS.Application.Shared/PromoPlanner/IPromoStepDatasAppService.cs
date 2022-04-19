using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using RMS.PromoPlanner;


namespace RMS.PromoPlanner
{
    public interface IPromoStepDatasAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPromoStepDataForViewDto>> GetAll(GetAllPromoStepDatasInput input);

        Task<GetPromoStepDataForViewDto> GetPromoStepDataForView(int id);
		Task<long?> GetPromoStepDataIdForFks(long promoId, long promoStepId);

		Task<GetPromoStepDataForEditOutput> GetPromoStepDataForEdit(EntityDto input);

		Task<long> CreateOrEdit(CreateOrEditPromoStepDataDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPromoStepDatasToExcel(GetAllPromoStepDatasForExcelInput input);

		
		Task<PagedResultDto<PromoStepDataPromoStepLookupTableDto>> GetAllPromoStepForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PromoStepDataPromoLookupTableDto>> GetAllPromoForLookupTable(GetAllForLookupTableInput input);
		
    }
}