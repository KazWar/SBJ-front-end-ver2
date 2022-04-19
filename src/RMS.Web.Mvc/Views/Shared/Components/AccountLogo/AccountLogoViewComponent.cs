using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Session;

namespace RMS.Web.Views.Shared.Components.AccountLogo
{
    public class AccountLogoViewComponent : RMSViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AccountLogoViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string skin, string height)
        {
            var loginInfo = await _sessionCache.GetCurrentLoginInformationsAsync();
            return View(new AccountLogoViewModel(loginInfo, skin, height));
        }
    }
}
