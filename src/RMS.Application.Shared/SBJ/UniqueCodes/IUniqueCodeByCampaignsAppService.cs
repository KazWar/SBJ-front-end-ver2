using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.UniqueCodes.Dtos;
using RMS.Dto;

namespace RMS.SBJ.UniqueCodes
{
    public interface IUniqueCodeByCampaignsAppService : IApplicationService
    {
        Task<PagedResultDto<GetUniqueCodeByCampaignForViewDto>> GetAll(GetAllUniqueCodeByCampaignsInput input);

        Task<GetUniqueCodeByCampaignForViewDto> GetUniqueCodeByCampaignForView(string id);

        Task<GetUniqueCodeByCampaignForEditOutput> GetUniqueCodeByCampaignForEdit(EntityDto<string> input);

        Task CreateOrEdit(CreateOrEditUniqueCodeByCampaignDto input);

        Task Delete(EntityDto<string> input);

        Task<PagedResultDto<UniqueCodeByCampaignCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input);

    }
}