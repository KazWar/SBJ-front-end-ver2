using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class VersionControlApiCallDto
    {
        public string VersionNumber { get; set; }

        public string FormData { get; set; }

        public string FormLocale { get; set; }
    }
}

