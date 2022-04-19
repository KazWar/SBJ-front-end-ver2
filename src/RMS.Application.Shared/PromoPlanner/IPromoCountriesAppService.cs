using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;


namespace RMS.PromoPlanner
{
    public interface IPromoCountriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPromoCountryForViewDto>> GetAll(GetAllPromoCountriesInput input);
		Task<PagedResultDto<CustomPromoCountryForView>> GetAllCountriesForPromo(GetAllCountriesForPromoInput input);
		Task<GetPromoCountryForViewDto> GetPromoCountryForView(long id);
		Task<GetPromoCountryForEditOutput> GetPromoCountryForEdit(EntityDto<long> input);
		Task CreateOrEdit(CreateOrEditPromoCountryDto input);
		Task Delete(EntityDto<long> input);
		Task<FileDto> GetPromoCountriesToExcel(GetAllPromoCountriesForExcelInput input);
		Task<PagedResultDto<PromoCountryPromoLookupTableDto>> GetAllPromoForLookupTable(GetAllForLookupTableInput input);
		Task<PagedResultDto<PromoCountryCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input);
    }
}