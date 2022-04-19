using System.Threading.Tasks;
using Abp.Application.Services;
using RMS.Sessions.Dto;

namespace RMS.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
