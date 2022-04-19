using System.Threading.Tasks;
using Abp.Application.Services;
using RMS.Configuration.Tenants.Dto;

namespace RMS.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
