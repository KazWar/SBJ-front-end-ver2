using RMS.SBJ.Forms.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.FormFields
{
    public class CreateOrEditFormFieldModalViewModel
    {
       public CreateOrEditFormFieldDto FormField { get; set; }

	   		public string FieldTypeDescription { get; set;}


       
	   public bool IsEditMode => FormField.Id.HasValue;
    }
}