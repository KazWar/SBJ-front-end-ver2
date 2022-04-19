using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.RetailerLocations.Dtos
{
    public class GetAllRetailerLocationsForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }


		 public string RetailerNameFilter { get; set; }

		 		 public string AddressAddressLine1Filter { get; set; }

		 
    }
}