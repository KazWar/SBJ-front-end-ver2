using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.Layout;
using RMS.Web.Session;
using RMS.Web.Views;

namespace RMS.Web.Areas.App.Views.Shared.Components.AppTheme10Brand
{
    public class AppTheme10BrandViewComponent : RMSViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme10BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
