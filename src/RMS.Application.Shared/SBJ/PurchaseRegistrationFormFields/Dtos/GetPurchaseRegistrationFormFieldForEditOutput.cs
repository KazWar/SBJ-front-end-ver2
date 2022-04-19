using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.PurchaseRegistrationFormFields.Dtos
{
    public class GetPurchaseRegistrationFormFieldForEditOutput
    {
		public CreateOrEditPurchaseRegistrationFormFieldDto PurchaseRegistrationFormField { get; set; }

		public string FormFieldDescription { get; set;}


    }
}