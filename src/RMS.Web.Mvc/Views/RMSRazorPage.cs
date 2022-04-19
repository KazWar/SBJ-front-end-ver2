using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using RMS.UiCustomization;
using RMS.UiCustomization.Dto;

namespace RMS.Web.Views
{
    public abstract class RMSRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        [RazorInject]
        public IUiThemeCustomizerFactory UiThemeCustomizerFactory { get; set; }

        protected RMSRazorPage()
        {
            LocalizationSourceName = RMSConsts.LocalizationSourceName;
        }

        public async Task<UiCustomizationSettingsDto> GetTheme()
        {
            var themeCustomizer = await UiThemeCustomizerFactory.GetCurrentUiCustomizer();
            var settings = await themeCustomizer.GetUiSettings();
            return settings;
        }

        public async Task<string> GetContainerClass()
        {
            var cssClass = "kt-container ";
            var theme = await GetTheme();
            
            if (theme.BaseSettings.Layout.LayoutType == "fluid")
            {
                cssClass += "kt-container--fluid";
            }

            return cssClass;
        }
    }
}
