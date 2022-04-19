using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Models.CampaignTypeEvents
{
    public class SaveCampaignTypeEventViewModel
    {
        public CampaignTypeProcessEventsViewModel[] Events { get; set; }

        public long CampaignTypeId { get; set; }

        public long RegistrationCampaignTypeEventId { get; set; }
    }
}
