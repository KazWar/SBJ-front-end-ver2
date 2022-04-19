using Abp.Application.Services;
using RMS.Dto;
using RMS.Logging.Dto;

namespace RMS.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
