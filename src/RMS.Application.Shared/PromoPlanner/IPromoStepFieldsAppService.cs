using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;


namespace RMS.PromoPlanner
{
    public interface IPromoStepFieldsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPromoStepFieldForViewDto>> GetAll(GetAllPromoStepFieldsInput input);

        Task<GetPromoStepFieldForViewDto> GetPromoStepFieldForView(int id);

		Task<GetPromoStepFieldForEditOutput> GetPromoStepFieldForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPromoStepFieldDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPromoStepFieldsToExcel(GetAllPromoStepFieldsForExcelInput input);

		
		Task<PagedResultDto<PromoStepFieldPromoStepLookupTableDto>> GetAllPromoStepForLookupTable(GetAllForLookupTableInput input);
		
    }
}