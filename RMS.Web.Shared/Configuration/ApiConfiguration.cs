using System;
using System.Collections.Generic;

namespace RMS.Web.Shared.Configuration
{

    public class ApiConfiguration
    {
        public Dictionary<BuildFlags, string> ApiRootAddresses { get; set; }
    }
}
