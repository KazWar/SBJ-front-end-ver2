using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Company.Dtos
{
    public class GetAllAddressesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string AddressLine1Filter { get; set; }

		public string AddressLine2Filter { get; set; }

		public string PostalCodeFilter { get; set; }

		public string CityFilter { get; set; }


		 public string CountryCountryCodeFilter { get; set; }

		 
    }
}