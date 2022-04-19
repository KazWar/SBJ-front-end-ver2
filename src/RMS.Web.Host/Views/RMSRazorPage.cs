using Abp.AspNetCore.Mvc.Views;

namespace RMS.Web.Views
{
    public abstract class RMSRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected RMSRazorPage()
        {
            LocalizationSourceName = RMSConsts.LocalizationSourceName;
        }
    }
}
