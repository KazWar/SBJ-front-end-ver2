using RMS.SBJ.CodeTypeTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.Countries
{
    public class CreateOrEditCountryModalViewModel
    {
       public CreateOrEditCountryDto Country { get; set; }

	   
       
	   public bool IsEditMode => Country.Id.HasValue;
    }
}