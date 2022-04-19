using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Retailers.Dtos
{
    public class GetAllRetailersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string CodeFilter { get; set; }


		 public string CountryCountryCodeFilter { get; set; }

		 
    }
}