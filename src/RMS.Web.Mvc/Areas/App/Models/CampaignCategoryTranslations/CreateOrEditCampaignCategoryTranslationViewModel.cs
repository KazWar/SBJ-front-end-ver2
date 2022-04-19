using RMS.SBJ.CodeTypeTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.CampaignCategoryTranslations
{
    public class CreateOrEditCampaignCategoryTranslationModalViewModel
    {
       public CreateOrEditCampaignCategoryTranslationDto CampaignCategoryTranslation { get; set; }

	   		public string LocaleDescription { get; set;}

		public string CampaignCategoryName { get; set;}


       
	   public bool IsEditMode => CampaignCategoryTranslation.Id.HasValue;
    }
}