using Abp.AutoMapper;
using RMS.MultiTenancy.Dto;

namespace RMS.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
