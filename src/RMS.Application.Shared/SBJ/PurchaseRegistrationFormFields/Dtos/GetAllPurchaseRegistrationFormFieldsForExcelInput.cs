using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.PurchaseRegistrationFormFields.Dtos
{
    public class GetAllPurchaseRegistrationFormFieldsForExcelInput
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string FormFieldDescriptionFilter { get; set; }

		 
    }
}