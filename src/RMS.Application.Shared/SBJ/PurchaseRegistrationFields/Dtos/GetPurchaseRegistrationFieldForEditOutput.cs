using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.PurchaseRegistrationFields.Dtos
{
    public class GetPurchaseRegistrationFieldForEditOutput
    {
		public CreateOrEditPurchaseRegistrationFieldDto PurchaseRegistrationField { get; set; }

		public string FormFieldDescription { get; set;}


    }
}