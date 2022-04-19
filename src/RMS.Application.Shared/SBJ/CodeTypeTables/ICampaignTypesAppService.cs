using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;

namespace RMS.SBJ.CodeTypeTables
{
    public interface ICampaignTypesAppService : IApplicationService 
    {
        Task<GetCampaignTypeForViewDto> GetByCode(string code);

        Task<PagedResultDto<GetCampaignTypeForViewDto>> GetAll(GetAllCampaignTypesInput input);

        Task<IEnumerable<GetCampaignTypeForViewDto>> GetAllWithoutPaging();

        Task<GetCampaignTypeForViewDto> GetCampaignTypeForView(long id);

		Task<GetCampaignTypeForEditOutput> GetCampaignTypeForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCampaignTypeDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCampaignTypesToExcel(GetAllCampaignTypesForExcelInput input);

        Task<PagedResultDto<GetCampaignTypeForViewDto>> GetAllActiveCampaignType();
    }
}