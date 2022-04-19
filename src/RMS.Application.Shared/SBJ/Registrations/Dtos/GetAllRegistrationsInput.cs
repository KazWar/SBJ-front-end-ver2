using Abp.Application.Services.Dto;

namespace RMS.SBJ.Registrations.Dtos
{
    public class GetAllRegistrationsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string FirstNameFilter { get; set; }

        public string LastNameFilter { get; set; }

        public string StreetFilter { get; set; }

        public string HouseNrFilter { get; set; }

        public string PostalCodeFilter { get; set; }

        public string CityFilter { get; set; }

        public string EmailAddressFilter { get; set; }

        public string PhoneNumberFilter { get; set; }

        public string CompanyNameFilter { get; set; }

        public string GenderFilter { get; set; }

        public string SerialNumberFilter { get; set; }

        // TODO: remove unused things
        public long? MaxCountryIdFilter { get; set; }
        public long? MinCountryIdFilter { get; set; }

        public long? MaxCampaignIdFilter { get; set; }
        public long? MinCampaignIdFilter { get; set; }

        public string RegistrationStatusStatusCodeFilter { get; set; }

        public string FormLocaleDescriptionFilter { get; set; }

        public string CampaignDescriptionFilter { get; set; }

        public string RegistrationStatusFilter { get; set; }

        public bool ActiveCampaignsOnlyFilter { get; set; }
    }
}