using Abp.AspNetCore.Mvc.ViewComponents;

namespace RMS.Web.Views
{
    public abstract class RMSViewComponent : AbpViewComponent
    {
        protected RMSViewComponent()
        {
            LocalizationSourceName = RMSConsts.LocalizationSourceName;
        }
    }
}