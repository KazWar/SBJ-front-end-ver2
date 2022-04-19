using Abp.Application.Services;
using System.Threading.Tasks;

namespace RMS.SBJ.Migrations
{
    public interface IMigrationsAppService : IApplicationService
    {
        Task<bool> SeedDatabase();
    }
}
