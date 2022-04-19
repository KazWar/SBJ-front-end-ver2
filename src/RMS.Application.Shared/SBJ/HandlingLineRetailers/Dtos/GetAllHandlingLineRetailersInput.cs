using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.HandlingLineRetailers.Dtos
{
    public class GetAllHandlingLineRetailersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string HandlingLineCustomerCodeFilter { get; set; }

		 		 public string RetailerNameFilter { get; set; }

		 
    }
}