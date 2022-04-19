using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;

namespace RMS.SBJ.CampaignProcesses
{
    public interface ICampaignTranslationsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCampaignTranslationForViewDto>> GetAll(GetAllCampaignTranslationsInput input);

        Task<GetCampaignTranslationForViewDto> GetCampaignTranslationForView(long id);

        Task<GetCampaignTranslationForEditOutput> GetCampaignTranslationForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCampaignTranslationDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCampaignTranslationsToExcel(GetAllCampaignTranslationsForExcelInput input);

        Task<PagedResultDto<CampaignTranslationCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CampaignTranslationLocaleLookupTableDto>> GetAllLocaleForLookupTable(GetAllForLookupTableInput input);

    }
}