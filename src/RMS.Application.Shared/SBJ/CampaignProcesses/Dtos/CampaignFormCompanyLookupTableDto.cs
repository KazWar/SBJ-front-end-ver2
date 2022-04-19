using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CampaignFormCompanyLookupTableDto
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }

        public long FormId { get; set; }

        public long LocaleId { get; set; }

    }
}
