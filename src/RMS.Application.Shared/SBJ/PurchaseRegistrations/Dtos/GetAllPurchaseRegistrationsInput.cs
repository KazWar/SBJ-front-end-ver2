using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.PurchaseRegistrations.Dtos
{
    public class GetAllPurchaseRegistrationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxQuantityFilter { get; set; }
		public int? MinQuantityFilter { get; set; }

		public decimal? MaxTotalAmountFilter { get; set; }
		public decimal? MinTotalAmountFilter { get; set; }

		public DateTime? MaxPurchaseDateFilter { get; set; }
		public DateTime? MinPurchaseDateFilter { get; set; }

		public string InvoiceImageFilter { get; set; }


		 public string RegistrationFirstNameFilter { get; set; }

		 		 public string ProductCtnFilter { get; set; }

		 		 public string HandlingLineCustomerCodeFilter { get; set; }

		 		 public string RetailerLocationNameFilter { get; set; }

		 
    }
}