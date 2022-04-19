using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;


namespace RMS.PromoPlanner
{
    public interface IPromoRetailersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPromoRetailerForViewDto>> GetAll(GetAllPromoRetailersInput input);

        Task<GetPromoRetailerForViewDto> GetPromoRetailerForView(long id);

		Task<GetPromoRetailerForEditOutput> GetPromoRetailerForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditPromoRetailerDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetPromoRetailersToExcel(GetAllPromoRetailersForExcelInput input);

		
		Task<PagedResultDto<PromoRetailerPromoLookupTableDto>> GetAllPromoForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PromoRetailerRetailerLookupTableDto>> GetAllRetailerForLookupTable(GetAllForLookupTableInput input);
		
    }
}