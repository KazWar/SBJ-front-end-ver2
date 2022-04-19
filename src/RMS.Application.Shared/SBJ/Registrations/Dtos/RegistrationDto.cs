using Abp.Application.Services.Dto;

namespace RMS.SBJ.Registrations.Dtos
{
    public class RegistrationDto : EntityDto<long>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string Street { get; set; }

        public string HouseNr { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string CompanyName { get; set; }

        public string Bic { get; set; }

        public string Iban { get; set; }

        public string IncompleteFields { get; set; }

        public string RejectedFields { get; set; }

        public string Password { get; set; }

        public long CampaignId { get; set; }

        public long? CampaignFormId { get; set; }

        public long CountryId { get; set; }

        public long? LocaleId { get; set; }

        public long RegistrationStatusId { get; set; }

        public long? RejectionReasonId { get; set; }
    }
}