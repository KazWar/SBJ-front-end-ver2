using Abp.Application.Services;
using System.Threading.Tasks;

namespace RMS.AzureBlobStorage
{
    public interface IAzureBlobStorageAppService : IApplicationService
    {
        Task<string> UploadBinaryAsync(string azureStorageConnection, string container, byte[] bytes, string fileName);
        Task<byte[]> DownloadBinaryAsync(string azureStorageConnection, string container, string fileName);
        Task<string> DownloadBase64StringAsync(string azureStorageConnection, string container, string fileName);
    }
}
