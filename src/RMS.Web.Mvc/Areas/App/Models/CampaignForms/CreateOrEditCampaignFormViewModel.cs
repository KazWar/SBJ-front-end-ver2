using RMS.SBJ.CampaignProcesses.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.CampaignForms
{
    public class CreateOrEditCampaignFormModalViewModel
    {
       public CreateOrEditCampaignFormDto CampaignForm { get; set; }

	   		public string CampaignName { get; set;}

		public string FormVersion { get; set;}


       
	   public bool IsEditMode => CampaignForm.Id.HasValue;
    }
}