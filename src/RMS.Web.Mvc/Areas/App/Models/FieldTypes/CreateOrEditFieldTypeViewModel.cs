using RMS.SBJ.Forms.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.FieldTypes
{
    public class CreateOrEditFieldTypeModalViewModel
    {
       public CreateOrEditFieldTypeDto FieldType { get; set; }

	   
       
	   public bool IsEditMode => FieldType.Id.HasValue;
    }
}