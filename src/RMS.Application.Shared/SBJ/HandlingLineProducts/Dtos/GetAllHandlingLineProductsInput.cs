using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.HandlingLineProducts.Dtos
{
    public class GetAllHandlingLineProductsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string HandlingLineCustomerCodeFilter { get; set; }

		 		 public string ProductDescriptionFilter { get; set; }

		 
    }
}