using RMS.SBJ.CampaignProcesses.Dtos;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.CampaignTypeEvents
{
    public class CreateOrEditCampaignTypeEventModalViewModel
    {
       public CreateOrEditCampaignTypeEventDto CampaignTypeEvent { get; set; }

	   		public string CampaignTypeName { get; set;}

		public string ProcessEventName { get; set;}


	   public bool IsEditMode => CampaignTypeEvent.Id.HasValue;
    }
}