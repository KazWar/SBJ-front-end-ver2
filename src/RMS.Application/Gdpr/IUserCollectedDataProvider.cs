using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using RMS.Dto;

namespace RMS.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
