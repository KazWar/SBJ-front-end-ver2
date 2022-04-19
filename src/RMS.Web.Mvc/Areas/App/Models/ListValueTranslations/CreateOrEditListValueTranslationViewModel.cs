using RMS.SBJ.Forms.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.ListValueTranslations
{
    public class CreateOrEditListValueTranslationModalViewModel
    {
       public CreateOrEditListValueTranslationDto ListValueTranslation { get; set; }

	   		public string ListValueKeyValue { get; set;}

		public string LocaleLanguageCode { get; set;}


       
	   public bool IsEditMode => ListValueTranslation.Id.HasValue;
    }
}