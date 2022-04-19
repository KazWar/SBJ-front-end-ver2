
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.PurchaseRegistrations.Dtos
{
    public class PurchaseRegistrationDto : EntityDto<long>
    {
		public int Quantity { get; set; }

		public decimal TotalAmount { get; set; }

		public DateTime PurchaseDate { get; set; }

		public string InvoiceImage { get; set; }


		 public long RegistrationId { get; set; }

		 		 public long ProductId { get; set; }

		 		 public long HandlingLineId { get; set; }

		 		 public long RetailerLocationId { get; set; }

		 
    }
}