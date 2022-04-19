using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.RetailerLocations.Dtos;
using RMS.Dto;


namespace RMS.SBJ.RetailerLocations
{
    public interface IRetailerLocationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRetailerLocationForViewDto>> GetAll(GetAllRetailerLocationsInput input);

        Task<GetRetailerLocationForViewDto> GetRetailerLocationForView(long id);

		Task<GetRetailerLocationForEditOutput> GetRetailerLocationForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditRetailerLocationDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetRetailerLocationsToExcel(GetAllRetailerLocationsForExcelInput input);

		
		Task<PagedResultDto<RetailerLocationRetailerLookupTableDto>> GetAllRetailerForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<RetailerLocationAddressLookupTableDto>> GetAllAddressForLookupTable(GetAllForLookupTableInput input);
		
    }
}