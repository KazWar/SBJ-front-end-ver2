using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;


namespace RMS.SBJ.CampaignProcesses
{
    public interface ICampaignTypeEventsAppService : IApplicationService 
    {
		Task<GetCampaignTypeEventForViewDto> GetByIdAsync(long campaignTypeEventId);

		Task<PagedResultDto<GetCampaignTypeEventForViewDto>> GetAll(GetAllCampaignTypeEventsInput input);

        Task<GetCampaignTypeEventForViewDto> GetCampaignTypeEventForView(long id);

		Task<GetCampaignTypeEventForEditOutput> GetCampaignTypeEventForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCampaignTypeEventDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCampaignTypeEventsToExcel(GetAllCampaignTypeEventsForExcelInput input);

		Task<PagedResultDto<CampaignTypeEventCampaignTypeLookupTableDto>> GetAllCampaignTypeForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<CampaignTypeEventProcessEventLookupTableDto>> GetAllProcessEventForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<GetCampaignTypeEventForViewDto>> GetAll();
	}
}