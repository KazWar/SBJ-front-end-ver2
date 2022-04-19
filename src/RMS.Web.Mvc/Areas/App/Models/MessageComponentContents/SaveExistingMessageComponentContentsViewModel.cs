using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.MessageComponentContents
{
    public sealed class SaveExistingMessageComponentContentsViewModel
    {
        public long Id { get; set; }
        public long CampaignTypeEventRegistrationStatusId { get; set; }
        public long MessageComponentId { get; set; }
        public string Content { get; set; }
        public long MessageContentTranslationId { get; set; }
        public long MessageComponentContentId { get; set; }
        public long LocaleId { get; set; }
    }
}
