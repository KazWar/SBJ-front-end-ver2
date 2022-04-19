using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Models.CampaignTypeEvents
{
    public class CampaignTypeRegistrationStatusesViewModel
    {
        public long RegistrationStatusId { get; set; }
        public long RegistrationCampaignTypeEventId { get; set; }
        public long RegistrationSortOrder { get; set; }
        public long CampaignTypeEventRegistrationStatusId { get; set; }
        public string RegistrationStatusDescription { get; set; }
    }
}
