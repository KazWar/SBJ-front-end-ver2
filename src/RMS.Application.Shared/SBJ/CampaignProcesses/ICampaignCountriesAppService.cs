using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;

namespace RMS.SBJ.CampaignProcesses
{
    public interface ICampaignCountriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCampaignCountryForViewDto>> GetAll(GetAllCampaignCountriesInput input);

        Task<GetCampaignCountryForViewDto> GetCampaignCountryForView(long id);

        Task<GetCampaignCountryForEditOutput> GetCampaignCountryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCampaignCountryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCampaignCountriesToExcel(GetAllCampaignCountriesForExcelInput input);

        Task<PagedResultDto<CampaignCountryCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CampaignCountryCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input);

    }
}