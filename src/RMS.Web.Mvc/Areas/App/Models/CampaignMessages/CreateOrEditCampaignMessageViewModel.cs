using RMS.SBJ.CampaignProcesses.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.CampaignMessages
{
    public class CreateOrEditCampaignMessageModalViewModel
    {
       public CreateOrEditCampaignMessageDto CampaignMessage { get; set; }

	   		public string CampaignName { get; set;}

		public string MessageVersion { get; set;}


       
	   public bool IsEditMode => CampaignMessage.Id.HasValue;
    }
}