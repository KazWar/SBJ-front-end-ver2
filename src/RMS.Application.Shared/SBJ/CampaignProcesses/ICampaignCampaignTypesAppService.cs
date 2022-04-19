using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using System.Collections.Generic;
using System.Collections.Generic;


namespace RMS.SBJ.CampaignProcesses
{
    public interface ICampaignCampaignTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCampaignCampaignTypeForViewDto>> GetAll(GetAllCampaignCampaignTypesInput input);

        Task<GetCampaignCampaignTypeForViewDto> GetCampaignCampaignTypeForView(long id);

		Task<GetCampaignCampaignTypeForEditOutput> GetCampaignCampaignTypeForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCampaignCampaignTypeDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCampaignCampaignTypesToExcel(GetAllCampaignCampaignTypesForExcelInput input);

		
		Task<List<CampaignCampaignTypeCampaignLookupTableDto>> GetAllCampaignForTableDropdown();
		
		Task<List<CampaignCampaignTypeCampaignTypeLookupTableDto>> GetAllCampaignTypeForTableDropdown();

		Task<List<GetCampaignCampaignTypeForViewDto>> GetAllCampaignCampaignTypes();

	}
}