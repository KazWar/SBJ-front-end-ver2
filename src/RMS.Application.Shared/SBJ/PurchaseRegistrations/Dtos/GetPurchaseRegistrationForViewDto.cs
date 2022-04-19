namespace RMS.SBJ.PurchaseRegistrations.Dtos
{
    public class GetPurchaseRegistrationForViewDto
    {
		public PurchaseRegistrationDto PurchaseRegistration { get; set; }

		public string RegistrationFirstName { get; set;}

		public string ProductCtn { get; set;}

		public string HandlingLineCustomerCode { get; set;}

		public string RetailerLocationName { get; set;}


    }
}