using System.Threading.Tasks;
using Abp.Application.Services;
using RMS.Configuration.Host.Dto;

namespace RMS.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
