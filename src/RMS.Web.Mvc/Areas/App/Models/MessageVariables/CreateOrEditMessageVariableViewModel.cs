using RMS.SBJ.CodeTypeTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.MessageVariables
{
    public class CreateOrEditMessageVariableModalViewModel
    {
       public CreateOrEditMessageVariableDto MessageVariable { get; set; }

	   
       
	   public bool IsEditMode => MessageVariable.Id.HasValue;
    }
}