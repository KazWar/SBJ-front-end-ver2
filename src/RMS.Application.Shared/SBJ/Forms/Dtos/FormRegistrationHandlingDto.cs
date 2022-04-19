using System.Collections.Generic;

namespace RMS.SBJ.Forms.Dtos
{
    public class FormRegistrationHandlingDto
    {
        public string Data { get; set; }
        public string UniqueCode { get; set; }
        public string UniqueCodeByCampaign { get; set; }
        public string CampaignCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Residence { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Product { get; set; }
        public string PurchaseDate { get; set; }
        public string Quantity { get; set; }
        public string TotalAmount { get; set; }
        public string SerialImagePath { get; set; }
        public string SerialNumber { get; set; }
        public string Locale { get; set; }
        public string CampaignId { get; set; }
        public string FileUploader { get; set; }
        public string CompanyName { get; set; }
        public string LegalForm { get; set; }
        public string BusinessNumber { get; set; }
        public string VatNumber { get; set; }
        public string Newsletter { get; set; }
        public string Privacy { get; set; }
        public string Policy { get; set; }
        public string PolicyAriel { get; set; }
        public string PolicyDreft { get; set; }
        public string PolicyNoFrost { get; set; }                     
        public string StorePurchased { get; set; }
        public string StoreNumber { get; set; }
        public string StoreName { get; set; }
        public string PlaceNameStore { get; set; }
        public string StoreNotAvalible { get; set; }
        public string OtherReason { get; set; }
        public string Remarks { get; set; }
        public DropdownListDto StorePicker { get; set; }
        public GetFormIbanBicDto IbanChecker { get; set; }
        public GetFormFieldValueListDto Country { get; set; }
        public GetFormFieldValueListDto Gender { get; set; }
        public GetFormFieldValueListDto AlreadyCustomer { get; set; }
        public GetFormFieldValueListDto DeviceReturnReason { get; set; }
        public GetFormFieldValueListDto EpenReturnReason { get; set; }
        public GetFormFieldValueListDto EpodReturnReason { get; set; }
        public GetFormFieldValueListDto CustomorReturnReason { get; set; }
        public GetFormFieldValueListDto DeclareAuthority { get; set; }
        public GetFormFieldValueListDto DeclareAuthorityRetour { get; set; }
        public GetFormFieldValueListDto DogProduct { get; set; }
        public GetFormFieldValueListDto CatProduct { get; set; }
        public GetFormFieldValueListDto CashBackProduct { get; set; }
        public GetFormFieldValueListDto CashBackPuppyKittenProduct { get; set; }
        public List<GetFormHandlingLineDto> ProductPremium { get; set; }
        public List<GetFormHandlingLineDto> ProductPremiumLite { get; set; }
        public List<ProductPremiumQuantityDto> ProductPremiumQuantity { get; set; }
        public List<string> InvoiceImagePath { get; set; }
        public List<string> SerialCodeImage { get; set; }
        public List<string> EanCodeImage { get; set; }
        public List<PurchaseRegistrationDataDto> PurchaseRegistration { get; set; }
        public List<PurchaseRegistrationDataDto> PurchaseRegistrationLite { get; set; }
        public List<PurchaseRegistrationDataDto> PurchaseRegistrationSerial { get; set; }
        public List<string> RetourStorageImage { get; set; }
    }
}
