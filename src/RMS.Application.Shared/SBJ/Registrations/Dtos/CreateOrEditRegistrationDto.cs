
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Registrations.Dtos
{
    public class CreateOrEditRegistrationDto : EntityDto<long?>
    {

		public string FirstName { get; set; }
		
		
		public string LastName { get; set; }
		
		
		public string Street { get; set; }
		
		
		public string HouseNr { get; set; }
		
		
		public string PostalCode { get; set; }
		
		
		public string City { get; set; }
		
		
		public string EmailAddress { get; set; }
		
		
		public string PhoneNumber { get; set; }
		
		
		public string CompanyName { get; set; }
		
		
		public string Gender { get; set; }
		
		
		public long CountryId { get; set; }
		
		
		public long CampaignId { get; set; }
		
		
		 public long RegistrationStatusId { get; set; }
		 
		 		 public long FormLocaleId { get; set; }
		 
		 
    }
}