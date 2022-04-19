using System.Collections.Generic;

namespace RMS.Web.Models.TenantSetup
{
    public class TenantSetupModel
    {
        public IEnumerable<TenantSettingModel> TenantSettings { get; set; }
    }
}
