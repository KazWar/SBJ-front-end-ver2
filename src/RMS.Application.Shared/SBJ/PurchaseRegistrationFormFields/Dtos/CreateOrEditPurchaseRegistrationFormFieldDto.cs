
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.PurchaseRegistrationFormFields.Dtos
{
    public class CreateOrEditPurchaseRegistrationFormFieldDto : EntityDto<long?>
    {

		public string Description { get; set; }
		
		
		 public long? FormFieldId { get; set; }
		 
		 
    }
}