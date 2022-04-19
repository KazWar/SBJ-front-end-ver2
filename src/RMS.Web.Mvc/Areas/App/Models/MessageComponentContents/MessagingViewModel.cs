using RMS.SBJ.CampaignProcesses.Dtos;

namespace RMS.Web.Areas.App.Models.MessageComponentContents
{
    public class MessagingViewModel
    {
        public GetCampaignTypeEventForViewDto CampaignTypeEvent { get; set; }
        public GetCampaignTypeEventRegistrationStatusForViewDto CampaignTypeEventRegistrationStatus { get; set; }
        public string RegistrationStatusDescription { get; set; }
    }
}