using Abp.AutoMapper;
using RMS.MultiTenancy;
using RMS.MultiTenancy.Dto;
using RMS.Web.Areas.App.Models.Common;

namespace RMS.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}