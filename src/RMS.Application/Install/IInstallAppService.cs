using System.Threading.Tasks;
using Abp.Application.Services;
using RMS.Install.Dto;

namespace RMS.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}