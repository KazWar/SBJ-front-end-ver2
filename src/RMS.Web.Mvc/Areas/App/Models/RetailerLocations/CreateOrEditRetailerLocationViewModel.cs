using RMS.SBJ.RetailerLocations.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.RetailerLocations
{
    public class CreateOrEditRetailerLocationViewModel
    {
       public CreateOrEditRetailerLocationDto RetailerLocation { get; set; }

	   		public string RetailerName { get; set;}

		public string AddressAddressLine1 { get; set;}


       
	   public bool IsEditMode => RetailerLocation.Id.HasValue;
    }
}