using System.Threading.Tasks;
using RMS.Sessions.Dto;

namespace RMS.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
