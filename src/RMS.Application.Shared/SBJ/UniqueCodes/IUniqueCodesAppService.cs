using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.UniqueCodes.Dtos;

namespace RMS.SBJ.UniqueCodes
{
    public interface IUniqueCodesAppService : IApplicationService
    {
        Task<PagedResultDto<GetUniqueCodeForViewDto>> GetAll(GetAllUniqueCodesInput input);

        Task<GetUniqueCodeForEditOutput> GetUniqueCodeForEdit(EntityDto<string> input);

        Task CreateOrEdit(CreateOrEditUniqueCodeDto input);

        Task Delete(EntityDto<string> input);

        Task<bool> IsCodeValid(string code);

        Task<bool> IsCodeValidByCampaign(string code, string campaignCode);

        Task<bool> SetCodeUsed(string code);

        Task<bool> SetCodeUsedByCampaign(string code, string campaignCode);
    }
}