using RMS.SBJ.Forms.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.ValueLists
{
    public class CreateOrEditValueListModalViewModel
    {
       public CreateOrEditValueListDto ValueList { get; set; }

	   
       
	   public bool IsEditMode => ValueList.Id.HasValue;
    }
}