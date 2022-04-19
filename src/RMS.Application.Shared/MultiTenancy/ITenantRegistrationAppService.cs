using System.Threading.Tasks;
using Abp.Application.Services;
using RMS.Editions.Dto;
using RMS.MultiTenancy.Dto;

namespace RMS.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}