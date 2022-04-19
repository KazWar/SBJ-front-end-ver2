using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Company.Dtos
{
    public class GetAllProjectManagersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string PhoneNumberFilter { get; set; }

		public string EmailAddressFilter { get; set; }


		 public string AddressPostalCodeFilter { get; set; }

		 
    }
}