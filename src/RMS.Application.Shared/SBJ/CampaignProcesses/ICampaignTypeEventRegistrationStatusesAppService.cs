using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;


namespace RMS.SBJ.CampaignProcesses
{
    public interface ICampaignTypeEventRegistrationStatusesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCampaignTypeEventRegistrationStatusForViewDto>> GetAll(GetAllCampaignTypeEventRegistrationStatusesInput input);

        Task<GetCampaignTypeEventRegistrationStatusForViewDto> GetCampaignTypeEventRegistrationStatusForView(long id);

		Task<GetCampaignTypeEventRegistrationStatusForEditOutput> GetCampaignTypeEventRegistrationStatusForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCampaignTypeEventRegistrationStatusDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCampaignTypeEventRegistrationStatusesToExcel(GetAllCampaignTypeEventRegistrationStatusesForExcelInput input);


        Task<PagedResultDto<CampaignTypeEventRegistrationStatusCampaignTypeEventLookupTableDto>> GetAllCampaignTypeEventForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CampaignTypeEventRegistrationStatusRegistrationStatusLookupTableDto>> GetAllRegistrationStatusForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<GetCampaignTypeEventRegistrationStatusForViewDto>> GetAllCampaignTypeEventRegistrationStatus();

    }
}