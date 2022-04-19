using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;

namespace RMS.PromoPlanner
{
    public interface IPromoStepFieldDatasAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPromoStepFieldDataForViewDto>> GetAll(GetAllPromoStepFieldDatasInput input);
		Task<long?> GetPromoStepDataFieldIdForFks(long promoStepFieldId, long promoStepDataId);
		Task<GetPromoStepFieldDataForViewDto> GetPromoStepFieldDataForView(int id);
		Task<GetPromoStepFieldDataForEditOutput> GetPromoStepFieldDataForEdit(EntityDto input);
		Task CreateOrEdit(CreateOrEditPromoStepFieldDataDto input);
		Task Delete(EntityDto input);
		Task<FileDto> GetPromoStepFieldDatasToExcel(GetAllPromoStepFieldDatasForExcelInput input);
		Task<PagedResultDto<PromoStepFieldDataPromoStepFieldLookupTableDto>> GetAllPromoStepFieldForLookupTable(GetAllForLookupTableInput input);
		Task<PagedResultDto<PromoStepFieldDataPromoStepDataLookupTableDto>> GetAllPromoStepDataForLookupTable(GetAllForLookupTableInput input);
    }
}