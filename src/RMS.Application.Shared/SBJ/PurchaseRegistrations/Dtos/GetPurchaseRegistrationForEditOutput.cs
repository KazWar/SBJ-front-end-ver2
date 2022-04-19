using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.PurchaseRegistrations.Dtos
{
    public class GetPurchaseRegistrationForEditOutput
    {
		public CreateOrEditPurchaseRegistrationDto PurchaseRegistration { get; set; }

		public string RegistrationFirstName { get; set;}

		public string ProductCtn { get; set;}

		public string HandlingLineCustomerCode { get; set;}

		public string RetailerLocationName { get; set;}


    }
}