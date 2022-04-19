using System;

namespace RMS.SBJ.Registrations.Dtos
{
    public class GetRegistrationForViewDto
    {
		public RegistrationDto Registration { get; set; }

		public string RegistrationStatusStatusCode { get; set;}

		public string FormLocaleDescription { get; set;}

        public string ExternalCode { get; set; }

        public string ProductCode { get; set; }

        public string CampaignDescription { get; set; }

        public string DateCreated { get; set; }
    }
}