using RMS.SBJ.CodeTypeTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.ProcessEvents
{
    public class CreateOrEditProcessEventModalViewModel
    {
       public CreateOrEditProcessEventDto ProcessEvent { get; set; }

	   
       
	   public bool IsEditMode => ProcessEvent.Id.HasValue;
    }
}