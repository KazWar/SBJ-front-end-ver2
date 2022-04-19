using RMS.SBJ.Forms.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.FormBlocks
{
    public class CreateOrEditFormBlockModalViewModel
    {
       public CreateOrEditFormBlockDto FormBlock { get; set; }

	   		public string FormLocaleDescription { get; set;}


       
	   public bool IsEditMode => FormBlock.Id.HasValue;
    }
}