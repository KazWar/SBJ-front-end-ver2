using RMS.SBJ.Forms.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.FormBlockFields
{
    public class CreateOrEditFormBlockFieldModalViewModel
    {
       public CreateOrEditFormBlockFieldDto FormBlockField { get; set; }

	   		public string FormFieldDescription { get; set;}

		public string FormBlockDescription { get; set;}


       
	   public bool IsEditMode => FormBlockField.Id.HasValue;
    }
}