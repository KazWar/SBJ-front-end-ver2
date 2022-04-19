using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace RMS.Web.Controllers
{
    public class HomeController : RMSControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
