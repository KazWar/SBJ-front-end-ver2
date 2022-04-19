using RMS.SBJ.CodeTypeTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.CampaignCategories
{
    public class CreateOrEditCampaignCategoryModalViewModel
    {
       public CreateOrEditCampaignCategoryDto CampaignCategory { get; set; }

	   
       
	   public bool IsEditMode => CampaignCategory.Id.HasValue;
    }
}