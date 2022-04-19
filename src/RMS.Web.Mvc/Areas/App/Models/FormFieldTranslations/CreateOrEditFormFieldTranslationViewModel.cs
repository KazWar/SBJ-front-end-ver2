using RMS.SBJ.Forms.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.FormFieldTranslations
{
    public class CreateOrEditFormFieldTranslationModalViewModel
    {
       public CreateOrEditFormFieldTranslationDto FormFieldTranslation { get; set; }

	   		public string FormFieldDescription { get; set;}

		public string LocaleLanguageCode { get; set;}


       
	   public bool IsEditMode => FormFieldTranslation.Id.HasValue;
    }
}