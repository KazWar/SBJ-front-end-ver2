using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.PurchaseRegistrationFieldDatas.Dtos
{
    public class GetAllPurchaseRegistrationFieldDatasInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string PurchaseRegistrationFormFieldDescriptionFilter { get; set; }

		 		 public string PurchaseRegistrationInvoiceImageFilter { get; set; }

		 
    }
}