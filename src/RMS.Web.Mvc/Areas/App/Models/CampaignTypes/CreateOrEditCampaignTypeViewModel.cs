using RMS.SBJ.CodeTypeTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.CampaignTypes
{
    public class CreateOrEditCampaignTypeModalViewModel
    {
       public CreateOrEditCampaignTypeDto CampaignType { get; set; }

	   
       
	   public bool IsEditMode => CampaignType.Id.HasValue;
    }
}