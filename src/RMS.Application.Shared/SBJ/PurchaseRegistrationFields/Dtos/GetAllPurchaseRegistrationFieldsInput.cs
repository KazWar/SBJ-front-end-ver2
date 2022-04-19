using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.PurchaseRegistrationFields.Dtos
{
    public class GetAllPurchaseRegistrationFieldsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string FormFieldDescriptionFilter { get; set; }

		 
    }
}