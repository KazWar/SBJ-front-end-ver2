using RMS.SBJ.Retailers.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.Retailers
{
    public class CreateOrEditRetailerModalViewModel
    {
       public CreateOrEditRetailerDto Retailer { get; set; }

	   		public string CountryCountryCode { get; set;}


       
	   public bool IsEditMode => Retailer.Id.HasValue;
    }
}