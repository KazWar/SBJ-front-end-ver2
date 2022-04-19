using RMS.SBJ.PurchaseRegistrations.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.PurchaseRegistrations
{
    public class CreateOrEditPurchaseRegistrationViewModel
    {
       public CreateOrEditPurchaseRegistrationDto PurchaseRegistration { get; set; }

	   		public string RegistrationFirstName { get; set;}

		public string ProductCtn { get; set;}

		public string HandlingLineCustomerCode { get; set;}

		public string RetailerLocationName { get; set;}


       
	   public bool IsEditMode => PurchaseRegistration.Id.HasValue;
    }
}