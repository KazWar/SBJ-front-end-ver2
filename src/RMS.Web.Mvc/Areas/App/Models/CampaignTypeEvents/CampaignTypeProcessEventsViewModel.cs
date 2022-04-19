using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Models.CampaignTypeEvents
{
    public class CampaignTypeProcessEventsViewModel
    {
        public long CampaignTypeId { get; set; }
        public long ProcessEventId { get; set; }
        public string ProcessEventName { get; set; }
        public long CampaignTypeEventId { get; set; }
        public string ParentBoardId { get; set; }
        public long SortOrderId { get; set; }
        public CampaignTypeRegistrationStatusesViewModel[] RegistrationStatusPerProcessEvent { get; set; }
    }
}
