using RMS.SBJ.CodeTypeTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.RegistrationStatuses
{
    public class CreateOrEditRegistrationStatusModalViewModel
    {
       public CreateOrEditRegistrationStatusDto RegistrationStatus { get; set; }

	   
       
	   public bool IsEditMode => RegistrationStatus.Id.HasValue;
    }
}