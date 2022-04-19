using RMS.SBJ.SystemTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.SystemLevels
{
    public class CreateOrEditSystemLevelModalViewModel
    {
       public CreateOrEditSystemLevelDto SystemLevel { get; set; }

	   
       
	   public bool IsEditMode => SystemLevel.Id.HasValue;
    }
}