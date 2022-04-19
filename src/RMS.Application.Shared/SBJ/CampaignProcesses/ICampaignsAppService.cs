using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using System.Collections.Generic;

namespace RMS.SBJ.CampaignProcesses
{
    public interface ICampaignsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCampaignForViewDto>> GetAll(GetAllCampaignsInput input);

        Task<int> GetSuggestedNewCampaignCode();

        Task<GetCampaignForViewDto> GetCampaignForView(long id);

		Task<GetCampaignForEditOutput> GetCampaignForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCampaignDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCampaignsToExcel(GetAllCampaignsForExcelInput input);

        Task<long> CreateOrEditAndGetId(CreateOrEditCampaignDto input);

        Task<long> CreateOrEditCustomized(CreateOrEditCampaignDto input, string selectedCampaignTypeIds, string selectedLocaleIds, long? sourceCampaignId);

        Task<bool> CheckCompanySetup();

        Task<PagedResultDto<CampaignFormCompanyLookupTableDto>> GetAllCampaignFormFromCompanyForLookupTable(RMS.SBJ.CampaignProcesses.Dtos.GetAllForLookupTableInput input);

        Task<GetCampaignForFormDto> GetCampaignForForm(string currentLocale ,long currentCampaignCode);
        
        Task<List<GetCampaignLocalesDto>> GetCampaignLocales();

        Task<long> GetLatestFormIdForCampaign();

        Task<long> GetLatestMessageIdForCampaign();

        Task<List<GetCampaignForViewDto>> GetAllCampaignsForDropdown(bool? activeCampaignsOnly);

        Task<List<GetCampaignLocalesDto>> GetCampaignOverview(string currentLocale);
    }  
}