using RMS.SBJ.Forms.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.ListValues
{
    public class CreateOrEditListValueModalViewModel
    {
       public CreateOrEditListValueDto ListValue { get; set; }

	   		public string ValueListDescription { get; set;}


       
	   public bool IsEditMode => ListValue.Id.HasValue;
    }
}