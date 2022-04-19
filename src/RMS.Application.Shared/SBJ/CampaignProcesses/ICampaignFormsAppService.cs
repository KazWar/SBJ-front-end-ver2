using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.CampaignProcesses
{
    public interface ICampaignFormsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCampaignFormForViewDto>> GetAll(GetAllCampaignFormsInput input);

        Task<GetCampaignFormForViewDto> GetCampaignFormForView(long id);

		Task<GetCampaignFormForEditOutput> GetCampaignFormForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCampaignFormDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCampaignFormsToExcel(GetAllCampaignFormsForExcelInput input);
		
		Task<PagedResultDto<CampaignFormCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<CampaignFormFormLookupTableDto>> GetAllFormForLookupTable(GetAllForLookupTableInput input);

		Task<List<GetCampaignFormForViewDto>> GetAllCampaignForms();

		
	}
}