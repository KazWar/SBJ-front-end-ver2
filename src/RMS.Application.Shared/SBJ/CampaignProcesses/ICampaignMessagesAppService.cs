using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.CampaignProcesses
{
    public interface ICampaignMessagesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCampaignMessageForViewDto>> GetAll(GetAllCampaignMessagesInput input);

        Task<GetCampaignMessageForViewDto> GetCampaignMessageForView(long id);

		Task<GetCampaignMessageForEditOutput> GetCampaignMessageForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCampaignMessageDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCampaignMessagesToExcel(GetAllCampaignMessagesForExcelInput input);

		
		Task<PagedResultDto<CampaignMessageCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<CampaignMessageMessageLookupTableDto>> GetAllMessageForLookupTable(GetAllForLookupTableInput input);

		Task<List<GetCampaignMessageForViewDto>> GetAllCampaignMessage();

	}
}