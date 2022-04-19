using RMS.SBJ.Forms.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.FormFieldValueLists
{
    public class CreateOrEditFormFieldValueListModalViewModel
    {
       public CreateOrEditFormFieldValueListDto FormFieldValueList { get; set; }

	   		public string FormFieldDescription { get; set;}

		public string ValueListDescription { get; set;}


       
	   public bool IsEditMode => FormFieldValueList.Id.HasValue;
    }
}