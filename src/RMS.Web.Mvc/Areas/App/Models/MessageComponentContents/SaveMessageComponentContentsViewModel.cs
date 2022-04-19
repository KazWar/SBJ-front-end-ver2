using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.MessageComponentContents
{
    public sealed class SaveMessageComponentContentsViewModel
    {
        public long CampaignTypeEventRegistrationStatusId { get; set; }

        public List<ComponentEditor> MessageComponentDictionary { get; set; }

    }
}
