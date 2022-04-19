using Abp.Extensions;
using RMS.SBJ.CampaignProcesses.Dtos;

namespace RMS.Web.Areas.App.Models.MessageComponentContents
{
    public class CreateOrEditMessageComponentContentModalViewModel
    {
        public CreateOrEditMessageComponentContentDto MessageComponentContent { get; set; }
        public string MessageComponentIsActive { get; set; }
        public string CampaignTypeEventRegistrationStatusSortOrder { get; set; }
        public bool IsEditMode => MessageComponentContent.Id.HasValue;
    }
}