using RMS.SBJ.CodeTypeTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.MessageComponentTypes
{
    public class CreateOrEditMessageComponentTypeModalViewModel
    {
       public CreateOrEditMessageComponentTypeDto MessageComponentType { get; set; }

	   
       
	   public bool IsEditMode => MessageComponentType.Id.HasValue;
    }
}