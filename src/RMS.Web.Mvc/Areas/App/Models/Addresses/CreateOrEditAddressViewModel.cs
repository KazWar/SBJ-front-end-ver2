using RMS.SBJ.Company.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.Addresses
{
    public class CreateOrEditAddressModalViewModel
    {
       public CreateOrEditAddressDto Address { get; set; }

	   		public string CountryCountryCode { get; set;}


       
	   public bool IsEditMode => Address.Id.HasValue;
    }
}