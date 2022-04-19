using RMS.SBJ.Forms.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.Forms
{
    public class CreateOrEditFormModalViewModel
    {
       public CreateOrEditFormDto Form { get; set; }

	   		public string SystemLevelDescription { get; set;}


       
	   public bool IsEditMode => Form.Id.HasValue;
    }
}