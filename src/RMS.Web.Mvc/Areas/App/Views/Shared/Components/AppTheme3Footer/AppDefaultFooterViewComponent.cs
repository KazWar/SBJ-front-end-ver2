﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.Layout;
using RMS.Web.Session;
using RMS.Web.Views;

namespace RMS.Web.Areas.App.Views.Shared.Components.AppTheme3Footer
{
    public class AppTheme3FooterViewComponent : RMSViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme3FooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
