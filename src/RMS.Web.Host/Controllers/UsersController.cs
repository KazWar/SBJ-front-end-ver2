using Abp.AspNetCore.Mvc.Authorization;
using RMS.Authorization;
using RMS.Storage;
using Abp.BackgroundJobs;

namespace RMS.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}