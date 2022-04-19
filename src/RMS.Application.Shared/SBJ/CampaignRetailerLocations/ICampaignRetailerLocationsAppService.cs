using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignRetailerLocations.Dtos;
using RMS.Dto;


namespace RMS.SBJ.CampaignRetailerLocations
{
    public interface ICampaignRetailerLocationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCampaignRetailerLocationForViewDto>> GetAll(GetAllCampaignRetailerLocationsInput input);

		Task<GetCampaignRetailerLocationForEditOutput> GetCampaignRetailerLocationForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCampaignRetailerLocationDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<CampaignRetailerLocationCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<CampaignRetailerLocationRetailerLocationLookupTableDto>> GetAllRetailerLocationForLookupTable(GetAllForLookupTableInput input);
		
    }
}