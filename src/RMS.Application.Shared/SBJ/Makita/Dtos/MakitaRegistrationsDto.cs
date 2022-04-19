using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.SBJ.Makita.Dtos
{
    public class MakitaRegistrationsDto
    {
        public long RegistrationId { get; set; }
        public string ActionCode { get; set; }
        public string CompanyName { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LegalForm { get; set; }
        public string BusinessNumber { get; set; }
        public string VatNumber { get; set; }
        public string ZipCode { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string Residence { get; set; }
        public string Country { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Debtor_number { get; set; }
        public string Contact_id { get; set; }
        public string CampaignId { get; set; }
        public List<MakitaProductRegistrationsDto> ProductRegistrations { get; set; }
    }
}
