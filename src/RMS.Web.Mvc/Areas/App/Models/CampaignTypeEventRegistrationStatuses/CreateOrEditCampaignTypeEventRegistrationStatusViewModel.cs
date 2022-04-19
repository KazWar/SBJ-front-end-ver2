using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.CampaignTypeEventRegistrationStatuses
{
    public class CreateOrEditCampaignTypeEventRegistrationStatusModalViewModel
    {
       public CreateOrEditCampaignTypeEventRegistrationStatusDto CampaignTypeEventRegistrationStatus { get; set; }

	   		public string CampaignTypeEventSortOrder { get; set;}

		public string RegistrationStatusDescription { get; set;}


	   public bool IsEditMode => CampaignTypeEventRegistrationStatus.Id.HasValue;
    }
}