using RMS.SBJ.CampaignProcesses.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.MessageContentTranslations
{
    public class CreateOrEditMessageContentTranslationModalViewModel
    {
       public CreateOrEditMessageContentTranslationDto MessageContentTranslation { get; set; }

	   		public string LocaleDescription { get; set;}

		public string MessageComponentContentContent { get; set;}


       
	   public bool IsEditMode => MessageContentTranslation.Id.HasValue;
    }
}